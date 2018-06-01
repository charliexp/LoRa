/* Includes ------------------------------------------------------------------*/
#include "daq.h"

/* Private typedef -----------------------------------------------------------*/
typedef struct Register_t
{
	char name[10];
	uint8_t length;
}Register_t;

typedef enum DAQState_t
{
	DAQ_INITIALIZED,
	DAQ_ACTIVE,
	DAQ_TIMEOUT
}DAQState_t;

/* Private define ------------------------------------------------------------*/
#define SOH 0x01
#define STX	0x02
#define ETX	0x03
#define ACK	0x06
#define CR	0x0d
#define LF	0x0a

/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
static uint8_t MeterIDReq[] = "/?!\r\n";
static uint8_t DataReq[] = {ACK, '0', '4', '0', CR, LF};

static Register_t BatteryVoltage = { "C.6.3", 5 };
static Register_t CurrentTime = { "0.9.1", 5 };

static DAQState_t DAQState;
static uint8_t DAQData[UART_BUFFSIZE];
static uint16_t DAQDataLength;

static DAQTime_t DAQ_LastValidTime = {0, 0, 0};

/* Private function prototypes -----------------------------------------------*/
static void DAQ_SendReq(uint8_t *request, uint16_t length);
static UartRxState_t DAQ_WaitForResp(uint16_t *length, uint8_t terminatorChar);
static uint8_t DAQ_CalculateBCC(uint8_t *message);
static void DAQ_ReadData(Register_t reg, char *result, uint8_t *resultLength);

/* Functions Definition ------------------------------------------------------*/
void DAQ_Init(void)
{
	DAQState = DAQ_INITIALIZED;
	DAQDataLength = 0;
}

void DAQ_UpdateData(void)
{
	DAQ_SendReq(MeterIDReq, 5);
	DAQ_WaitForResp(&DAQDataLength, CR);
	
	DAQ_SendReq(DataReq, 6);
	if (DAQ_WaitForResp(&DAQDataLength, ETX) != UART_RX_TIMEOUT)
		DAQState = DAQ_ACTIVE;
	else
		DAQState = DAQ_TIMEOUT;
}

DAQTime_t DAQ_GetTime(void)
{
	char value[10];
	uint8_t valueLength;
	uint8_t hour, minute, second;
	
	if (DAQState == DAQ_ACTIVE)
	{
		DAQ_ReadData(CurrentTime, value, &valueLength);
		
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
					DAQ_LastValidTime.hour = hour;
					DAQ_LastValidTime.minute = minute;
					DAQ_LastValidTime.second = second;
				}
			}
		}
	}
	
	return DAQ_LastValidTime;
}

static void DAQ_SendReq(uint8_t *request, uint16_t length)
{
	//Add checksum
	if (request[0] != '/' && request[0] != ACK)
		request[length++] = DAQ_CalculateBCC(request);
			
	UART_Send(request, length);
}

static UartRxState_t DAQ_WaitForResp(uint16_t *length, uint8_t terminatorChar)
{
	UartRxState_t result;
	do
	{
		result = UART_Receive(DAQData, length, terminatorChar);
	}while (result == UART_RX_PENDING);
	
	if (result == UART_RX_TIMEOUT)
		PRINTF("Timeout contor\r\n");
	
	return result;
}

static uint8_t DAQ_CalculateBCC(uint8_t *message)
{
	uint8_t sum = 0;
	//Skip SOH/STX
	message += 1;
	do
	{
		sum ^= *(message);
	}while (*message++ != ETX);
	
	return sum;
}

static void DAQ_ReadData(Register_t reg, char *result, uint8_t *resultLength)
{
	uint16_t i = 0, j = 0, k = 0;
	
	while (DAQData[i] != '!' || i != DAQDataLength - 1)
	{
		while ((DAQData[i] == reg.name[j]) && (j != reg.length - 1))
		{
			i++;
			j++;
		}
		if (j == reg.length - 1)
		{
			//Found register
			i += 2;
			while (DAQData[i] != ')' && DAQData[i] != '*')
			{
				//Found value
				result[k] = DAQData[i];
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
