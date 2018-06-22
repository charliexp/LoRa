/* Includes ------------------------------------------------------------------*/
#include "daq.h"
#include "hw_uart.h"
#include "pc.h"
#include "timeServer.h"

/* Private typedef -----------------------------------------------------------*/
typedef struct Register_t
{
	char name[10];
	uint8_t length;
}Register_t;

typedef enum DAQ_State_t
{
	READY,
	SENT_ID_REQ,
	REC_ID,
	SENT_DATA_REQ,
	REC_DATA,
	IDLE,
}DAQ_State_t;

/* Private define ------------------------------------------------------------*/
#define DATA_LENGTH		1071

#define SOH 			0x01
#define STX				0x02
#define ETX				0x03
#define ASCII_ACK	0x06
#define CR				0x0d
#define LF				0x0a

/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
static const uint8_t MeterIDReq[] = "/?!\r\n";
static const uint8_t DataReq[] = {ASCII_ACK, '0', '4', '0', CR, LF};

static const Register_t BatteryLevelRegister = { "C.6.3", 5 };
static const Register_t CurrentTimeRegister = { "0.9.1", 5 };
static const Register_t ActiveEnergyRegister = { "1.8.0", 5 };
static const Register_t InductiveEnergyRegister = { "130.8.0", 7 };
static const Register_t CapacitiveEnergyRegister = { "131.8.0", 7 };

/* Private variables ---------------------------------------------------------*/
extern UartHandle_t DAQ_UartHandle;

static uint8_t DAQ_Buffer[UART_BUFFSIZE];
static uint8_t DAQ_ValidBuffer[DATA_LENGTH];
static uint16_t DAQ_BufferLength;
static uint32_t DAQ_TimestampOldValue;
static uint32_t DAQ_ActiveOldValue;
static uint32_t DAQ_InductiveOldValue;
static uint32_t DAQ_CapacitiveOldValue;
static DAQ_State_t DAQ_State;
static TimerEvent_t DAQ_Timer;

/* Public variables ----------------------------------------------------------*/
DAQ_Struct_t DAQ_Data;

/* Private function prototypes -----------------------------------------------*/
static void DAQ_SendReq(const uint8_t *request, uint16_t length);
static UartRxState_t DAQ_CheckForResp(const uint8_t terminatorChar);
static uint8_t DAQ_CalculateBCC(const uint8_t *message);
static void DAQ_ReadRegister(const Register_t reg, char *result, uint8_t *resultLength);
static void DAQ_GetTime(void);
static void DAQ_GetBatteryLevel(void);
static void DAQ_GetPowers(void);
static void DAQ_TimerEvent(void);

/* Functions Definition ------------------------------------------------------*/
void DAQ_Init(void)
{
	DAQ_Data.haveMeter = true;
	DAQ_BufferLength = 0;
	DAQ_State = IDLE;
	
	TimerInit(&DAQ_Timer, DAQ_TimerEvent);
	TimerSetValue(&DAQ_Timer, DAQ_SAMPLE_RATE * 1000); 
  DAQ_TimerEvent();
}

void DAQ_Start(void)
{
	DAQ_State = READY;
}

void DAQ_MainLoop(void)
{
	UartRxState_t returnValue;
	
	switch(DAQ_State)
	{
		case READY:
			DAQ_SendReq(MeterIDReq, 5);
			DAQ_State = SENT_ID_REQ;
			break;
		case SENT_ID_REQ:
			returnValue = DAQ_CheckForResp(CR);
			if (returnValue == UART_RX_AVAILABLE)
				DAQ_State = REC_ID;
			else if (returnValue == UART_RX_TIMEOUT)
				DAQ_State = READY;
			break;
		case REC_ID:
			DAQ_SendReq(DataReq, 6);
			DAQ_State = SENT_DATA_REQ;
			break;
		case SENT_DATA_REQ:
			returnValue = DAQ_CheckForResp(ETX);
			if (returnValue == UART_RX_AVAILABLE)
			{
				memcpy(DAQ_ValidBuffer, DAQ_Buffer, DATA_LENGTH);
				DAQ_State = REC_DATA;
			}
			else if (returnValue == UART_RX_TIMEOUT)
				DAQ_State = READY;
			break;
		case REC_DATA:
			DAQ_State = IDLE;
			break;
		case IDLE:
			break;
		default:
			DAQ_State = IDLE;
			break;
	}
}

void DAQ_ReadData(void)
{
	DAQ_GetTime();
	DAQ_GetBatteryLevel();
	DAQ_GetPowers();
}

void DAQ_ProcessMessage(Message_t message)
{
	Frame_t reply;
	
	reply.endDevice = myAddress;
	reply.nrOfMessages = 1;
	
	reply.messages[0].command = message.command;
	
	switch (message.command)
	{
		case COMMAND_HAS_METER:
			DAQ_Data.haveMeter = message.rawArgument[0];
			if (DAQ_Data.haveMeter)
				TimerStart(&DAQ_Timer);
			else
				TimerStop(&DAQ_Timer);
			reply.messages[0].argLength = 1;
			reply.messages[0].rawArgument[0] = ACK;
			break;
		default:
			reply.messages[0].argLength = 1;
			reply.messages[0].rawArgument[0] = NAK;
			break;
	}
	
	PC_Send(reply);
}

static void DAQ_TimerEvent(void)
{
	DAQ_ReadData();
	DAQ_Start();
	
	if (DAQ_Data.haveMeter)
		TimerStart(&DAQ_Timer);
}

static void DAQ_SendReq(const uint8_t *request, uint16_t length)
{
	uint8_t bcc;
	UART_Send(&DAQ_UartHandle, (uint8_t *)request, length);
	
	//Add checksum
	if (request[0] != '/' && request[0] != ACK)
	{
		bcc = DAQ_CalculateBCC(request);
		UART_Send(&DAQ_UartHandle, &bcc, length);
	}
}

static UartRxState_t DAQ_CheckForResp(const uint8_t terminatorChar)
{
	UartRxState_t result;
	
	result = UART_ReceiveUntilChar(&DAQ_UartHandle, DAQ_Buffer, &DAQ_BufferLength, terminatorChar);
	
	return result;
}

static uint8_t DAQ_CalculateBCC(const uint8_t *message)
{
	uint8_t *pointer = (uint8_t *)message;
	uint8_t sum = 0;
	//Skip SOH/STX
	pointer += 1;
	do
	{
		sum ^= *(pointer);
	}while (*pointer++ != ETX);
	
	return sum;
}

static void DAQ_ReadRegister(const Register_t reg, char *result, uint8_t *resultLength)
{
	uint16_t i = 0, j = 0, k = 0;
	
	while (DAQ_ValidBuffer[i] != '!' && i < DATA_LENGTH - 1)
	{
		while ((DAQ_ValidBuffer[i] == reg.name[j]) && (j != reg.length - 1) && i < DATA_LENGTH - 1)
		{
			i++;
			j++;
		}
		if (j == reg.length - 1)
		{
			//Found register
			i += 2;
			while (DAQ_ValidBuffer[i] != ')' && DAQ_ValidBuffer[i] != '*' && i < DATA_LENGTH - 1)
			{
				//Found value
				result[k] = DAQ_ValidBuffer[i];
				i++;
				k++;
			}
			*resultLength = k;
			break;
		}
		else
		{
			i++;
			j = 0;
		}
	}
}

static uint32_t DAQ_GetTimestamp(void)
{
	return DAQ_Data.time.hour * 3600 + DAQ_Data.time.minute * 60 + DAQ_Data.time.second;
}

static void DAQ_GetTime(void)
{
	char value[10];
	uint8_t valueLength;
	uint8_t hour, minute, second;
	
	DAQ_ReadRegister(CurrentTimeRegister, value, &valueLength);
	
	if (value[0] >= '0' && value[0] <= '9' &&
			value[1] >= '0' && value[1] <= '9')
	{
		hour = (value[0] - '0') * 10 + (value[1] - '0');
		if (value[3] >= '0' && value[3] <= '9' &&
				value[4] >= '0' && value[4] <= '9')
		{
			minute = (value[3] - '0') * 10 + (value[4] - '0');
			if (value[6] >= '0' && value[6] <= '9' &&
					value[7] >= '0' && value[7] <= '9')
			{
				second = (value[6] - '0') * 10 + (value[7] - '0');
				DAQ_Data.time.hour = hour;
				DAQ_Data.time.minute = minute;
				DAQ_Data.time.second = second;
			}
		}
	}
}

static void DAQ_GetBatteryLevel(void)
{
	char value[10];
	uint8_t valueLength;
	
	DAQ_ReadRegister(BatteryLevelRegister, value, &valueLength);
	
	if (value[0] >= '0' && value[0] <= '9' &&
			value[2] >= '0' && value[2] <= '9' &&
			value[3] >= '0' && value[3] <= '9')
	{
		DAQ_Data.batteryLevel = (value[0] - '0') * 100 +
														(value[2] - '0') * 10 +
														(value[3] - '0');
	}
}

static void DAQ_GetPowers(void)
{
	char value[10];
	uint8_t valueLength;
	uint8_t i;
	uint32_t newValue;
	uint32_t timeDifference;
	
	newValue = DAQ_GetTimestamp();
	
	/* Time difference */
	if (newValue < DAQ_TimestampOldValue)
		timeDifference = newValue + 24 * 3600 - DAQ_TimestampOldValue;
	else
		timeDifference = newValue - DAQ_TimestampOldValue;
	DAQ_TimestampOldValue = DAQ_GetTimestamp();

	/* Active energy */
	i = 0;
	newValue = 0;
	DAQ_ReadRegister(ActiveEnergyRegister, value, &valueLength);
	if (valueLength == 9)
	{
		do
		{
			if (value[i] != '.')
				newValue = newValue * 10 + (value[i] - '0');
		}while (++i < valueLength);
		DAQ_Data.activeEnergy = newValue - DAQ_ActiveOldValue;
		DAQ_ActiveOldValue = newValue;
	}
	
	/* Capacitive energy */
	i = 0;
	newValue = 0;
	DAQ_ReadRegister(CapacitiveEnergyRegister, value, &valueLength);
	if (valueLength == 9)
	{
		do
		{
			if (value[i] != '.')
				newValue = newValue * 10 + (value[i] - '0');
		}while (++i < valueLength);
		DAQ_Data.capacitiveEnergy = newValue - DAQ_CapacitiveOldValue;
		DAQ_Data.inductive = false;
		DAQ_CapacitiveOldValue = newValue;
	}
	
	/* Inductive energy */
	i = 0;
	newValue = 0;
	DAQ_ReadRegister(InductiveEnergyRegister, value, &valueLength);
	if (valueLength == 9)
	{
		do
		{
			if (value[i] != '.')
				newValue = newValue * 10 + (value[i] - '0');
		}while (++i < valueLength);
		DAQ_Data.inductiveEnergy = newValue - DAQ_InductiveOldValue;
		DAQ_Data.inductive = true;
		DAQ_InductiveOldValue = newValue;
	}
	
	/* Powers */
	if (timeDifference != 0)
	{
		DAQ_Data.activePower = DAQ_Data.activeEnergy * 3600 / timeDifference;
		if (DAQ_Data.inductive)
				DAQ_Data.reactivePower = DAQ_Data.inductiveEnergy * 3600 / timeDifference;
		else
				DAQ_Data.reactivePower = DAQ_Data.capacitiveEnergy * 3600 / timeDifference;
		DAQ_Data.apparentPower = sqrt(DAQ_Data.activePower * DAQ_Data.activePower +
																	DAQ_Data.reactivePower * DAQ_Data.reactivePower);
		DAQ_Data.powerFactor = DAQ_Data.activePower * 100 / DAQ_Data.apparentPower;
	}
}
