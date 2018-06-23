/* Includes ------------------------------------------------------------------*/
#include "hw.h"
#include "lora.h"
#include "meter.h"
#include <string.h>

/* Private define ------------------------------------------------------------*/
#define UART_TIMEOUT						5
#define MAX_ATTEMPTS						3
#define MAX_SAMPLES							6
#define CURRENT_SAMPLE					(Meter_handle.samples[MAX_SAMPLES - 1])
#define PREVIOUS_SAMPLE					(Meter_handle.samples[MAX_SAMPLES - 2])
#define OLDEST_SAMPLE						(Meter_handle.samples[0])

#define REG_MAX_LENGTH					8
#define RESP_ID_LENGTH					22
#define RESP_DATA_LENGTH				1071

#define ASCII_TO_BINARY(x)			(x - '0')

/* Private typedef -----------------------------------------------------------*/
typedef enum MeterState_t
{
	METER_OK,
	METER_NOK,
	METER_TIMEOUT,
}MeterState_t;

typedef enum ReactivePower_t
{
	INDUCTIVE = 0x01,
	CAPACITIVE = 0x02,
}ReactivePower_t;

typedef struct Time_t
{
	uint8_t hour;
	uint8_t minute;
	uint8_t second;
	uint32_t timestamp;
}Time_t;

typedef struct Sample_t
{
	Time_t time;
	uint32_t activeEnergy;			//Resolution: 0.001, Unit: kWh
	uint32_t reactiveEnergy;		//Resolution: 0.001, Unit: kVARh
	uint32_t capacitiveEnergy;	//Resolution: 0.001, Unit: kVARh
	uint32_t inductiveEnergy;		//Resolution: 0.001, Unit: kVARh
	ReactivePower_t powerType;	
	uint32_t activePower;				//Resolution: 0.001, Unit: kW
	uint32_t reactivePower;			//Resolution: 0.001, Unit: kVAR
#ifdef GATEWAY
	uint32_t apparentPower;			//Resolution: 0.001, Unit: kVA
	int8_t powerFactor;					//Resolution: 0.01
#endif
}Sample_t;

typedef enum RegisterTag_t
{
	METER_ID_REQ,
	DATA_REQ,
	TIME,
	ACTIVE_ENERGY,
	INDUCTIVE_ENERGY,
	CAPACITIVE_ENERGY,
}RegisterTag_t;

typedef struct Register_t
{
	RegisterTag_t tag;
	char name[REG_MAX_LENGTH];
	uint8_t length;
}Register_t;

typedef struct MeterHandle_t
{
/* Uart handle */
	UART_HandleTypeDef uartHandle;
/* Uart receive buffer */
	uint8_t buffer[RESP_DATA_LENGTH];
/* Latest samples */
	Sample_t samples[MAX_SAMPLES];
/* Continuous failed attempts to communicate */
	uint8_t failedAttempts;
/* Meter state */
	MeterState_t state;
/* Timeout in seconds */
	uint16_t timeout;
}MeterHandle_t;

/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
static const Register_t MeterIDReq = {METER_ID_REQ, "/?!\r\n", 5};
static const Register_t DataReq = {DATA_REQ, "\x06\x30\x34\x30\x0D\x0A", 6};
static const Register_t CurrentTime = {TIME, "0.9.1", 5};
static const Register_t ActiveEnergy = {ACTIVE_ENERGY, "1.8.0", 5};
static const Register_t InductiveEnergy = {INDUCTIVE_ENERGY, "130.8.0", 7};
static const Register_t CapacitiveEnergy = {CAPACITIVE_ENERGY, "131.8.0", 7};

/* Private variables ---------------------------------------------------------*/
static MeterHandle_t Meter_handle;

/* Public variables ----------------------------------------------------------*/
/* Private function prototypes -----------------------------------------------*/
static uint32_t Meter_ArrayToInt(uint8_t *array, uint8_t length);
static void Meter_ProcessRegister(Register_t reg, uint8_t *result);
static void Meter_Read(uint16_t length);
static void Meter_SetEnergies(void);
static void Meter_SetPowers(void);
static void Meter_SetTimestamp(void);
static void Meter_ShiftSamples(void);
static void Meter_SignalError(void);
static void Meter_Write(Register_t reg);

/* Functions Definition ------------------------------------------------------*/
static uint32_t Meter_ArrayToInt(uint8_t *array, uint8_t length)
{
	uint8_t i;
	uint32_t returnValue = 0;
	
	do
	{
		returnValue = returnValue * 10 + ASCII_TO_BINARY(array[i]);
	}while (++i < length);
	
	return returnValue;
}

void Meter_Init(void)
{
	/* Init UART */
	Meter_handle.uartHandle.Instance = METER_USARTX;
	Meter_handle.uartHandle.Init.BaudRate = 9600;
	Meter_handle.uartHandle.Init.WordLength = UART_WORDLENGTH_8B;
	Meter_handle.uartHandle.Init.StopBits = UART_STOPBITS_1;
	Meter_handle.uartHandle.Init.Parity = UART_PARITY_EVEN;
	Meter_handle.uartHandle.Init.HwFlowCtl = UART_HWCONTROL_NONE;
	Meter_handle.uartHandle.Init.Mode = UART_MODE_TX_RX;
	Meter_handle.timeout = UART_TIMEOUT;
	if(HAL_UART_Init(&Meter_handle.uartHandle) != HAL_OK)
	{
		/* Initialization Error */
		Error_Handler(); 
	}
}

static void Meter_ProcessRegister(Register_t reg, uint8_t *result)
{
	uint16_t i = 0, j = 0, k = 0;
	
	while (Meter_handle.buffer[i] != '!' && i < RESP_DATA_LENGTH - 1)
	{
		while ((Meter_handle.buffer[i] == reg.name[j]) && (j != reg.length - 1) && i < RESP_DATA_LENGTH - 1)
		{
			i++;
			j++;
		}
		if (j == reg.length - 1)
		{
			/* Found register */
			i += 2;
			while (Meter_handle.buffer[i] != ')' && Meter_handle.buffer[i] != '*' && i < RESP_DATA_LENGTH - 1)
			{
				/* Skip decimal point character */
				if (Meter_handle.buffer[i] == '.')
					i++;
				
				/* Found value */
				result[k] = Meter_handle.buffer[i];
				i++;
				k++;
			}
			break;
		}
		else
		{
			i++;
			j = 0;
		}
	}
}

void Meter_ProcessRequest(Message_t message)
{
	Message_t reponse;
	uint8_t previousFailedAttempts = Meter_handle.failedAttempts;
	
	switch (message.command)
	{
		case COMMAND_ACQUISITION:
			if (Meter_handle.state == METER_OK)
			{
				Meter_Write(MeterIDReq);
				Meter_Read(RESP_ID_LENGTH);
			}
			else
				return;
			
			if (Meter_handle.state == METER_OK &&
				Meter_handle.failedAttempts - previousFailedAttempts == 0)
			{
				Meter_Write(DataReq);
				Meter_Read(RESP_DATA_LENGTH);
			}
			else
				return;
				
			if (Meter_handle.state == METER_OK &&
				Meter_handle.failedAttempts - previousFailedAttempts == 0)
			{
				Meter_ShiftSamples();
				Meter_SetTimestamp();
				Meter_SetEnergies();
				Meter_SetPowers();
			
				reponse.command = COMMAND_TIMESTAMP;
				reponse.argLength = 3;
				reponse.rawArgument[0] = CURRENT_SAMPLE.time.hour;
				reponse.rawArgument[1] = CURRENT_SAMPLE.time.minute;
				reponse.rawArgument[2] = CURRENT_SAMPLE.time.second;
				LoRa_QueueMessage(reponse);
				
				reponse.command = COMMAND_ACTIVE_ENERGY;
				reponse.argLength = 3;
				reponse.rawArgument[0] = (CURRENT_SAMPLE.activeEnergy >> 16) & 0xFF;
				reponse.rawArgument[1] = (CURRENT_SAMPLE.activeEnergy >> 8) & 0xFF;
				reponse.rawArgument[2] = (CURRENT_SAMPLE.activeEnergy >> 0) & 0xFF;
				LoRa_QueueMessage(reponse);
				
				reponse.command = COMMAND_REACTIVE_ENERGY;
				reponse.argLength = 3;
				reponse.rawArgument[0] = (CURRENT_SAMPLE.reactiveEnergy >> 16) & 0xFF;
				reponse.rawArgument[1] = (CURRENT_SAMPLE.reactiveEnergy >> 8) & 0xFF;
				reponse.rawArgument[2] = (CURRENT_SAMPLE.reactiveEnergy >> 0) & 0xFF;
				LoRa_QueueMessage(reponse);
				
				reponse.command = COMMAND_ACTIVE_POWER;
				reponse.argLength = 3;
				reponse.rawArgument[0] = (CURRENT_SAMPLE.activePower >> 16) & 0xFF;
				reponse.rawArgument[1] = (CURRENT_SAMPLE.activePower >> 8) & 0xFF;
				reponse.rawArgument[2] = (CURRENT_SAMPLE.activePower >> 0) & 0xFF;
				LoRa_QueueMessage(reponse);
				
				reponse.command = COMMAND_REACTIVE_POWER;
				reponse.argLength = 3;
				reponse.rawArgument[0] = (CURRENT_SAMPLE.reactivePower >> 16) & 0xFF;
				reponse.rawArgument[1] = (CURRENT_SAMPLE.reactivePower >> 8) & 0xFF;
				reponse.rawArgument[2] = (CURRENT_SAMPLE.reactivePower >> 0) & 0xFF;
				LoRa_QueueMessage(reponse);
			}
			break;
		default:
			break;
	}
}

static void Meter_Read(uint16_t length)
{
	HAL_StatusTypeDef returnValue;
	
	returnValue = HAL_UART_Receive(&Meter_handle.uartHandle, Meter_handle.buffer, length, Meter_handle.timeout);
	if (returnValue != HAL_OK)
		Meter_SignalError();
}

static void Meter_SetEnergies(void)
{
	uint8_t value[REG_MAX_LENGTH];
	
	Meter_ProcessRegister(ActiveEnergy, value);
	CURRENT_SAMPLE.activeEnergy = Meter_ArrayToInt(value, REG_MAX_LENGTH) - OLDEST_SAMPLE.activeEnergy;
	
	Meter_ProcessRegister(InductiveEnergy, value);
	CURRENT_SAMPLE.inductiveEnergy = Meter_ArrayToInt(value, REG_MAX_LENGTH) - OLDEST_SAMPLE.inductiveEnergy;
	
	Meter_ProcessRegister(CapacitiveEnergy, value);
	CURRENT_SAMPLE.capacitiveEnergy = Meter_ArrayToInt(value, REG_MAX_LENGTH) - OLDEST_SAMPLE.capacitiveEnergy;
}

static void Meter_SetPowers(void)
{
	uint16_t timeDifference = CURRENT_SAMPLE.time.timestamp - OLDEST_SAMPLE.time.timestamp;
	
	CURRENT_SAMPLE.activePower = CURRENT_SAMPLE.activeEnergy * 3600 / timeDifference;
	
	if (CURRENT_SAMPLE.inductiveEnergy >= CURRENT_SAMPLE.capacitiveEnergy)
	{
		CURRENT_SAMPLE.powerType = INDUCTIVE;
		CURRENT_SAMPLE.reactivePower = CURRENT_SAMPLE.inductiveEnergy * 3600 / timeDifference;
	}
	else
	{
		CURRENT_SAMPLE.powerType = CAPACITIVE;
		CURRENT_SAMPLE.reactivePower = CURRENT_SAMPLE.capacitiveEnergy * 3600 / timeDifference;
		CURRENT_SAMPLE.reactivePower = 0 - CURRENT_SAMPLE.reactivePower;
	}
	
#ifdef GATEWAY
	CURRENT_SAMPLE.apparentPower = sqrt(CURRENT_SAMPLE.activePower * CURRENT_SAMPLE.activePower +
		CURRENT_SAMPLE.reactivePower * CURRENT_SAMPLE.reactivePower);
	CURRENT_SAMPLE.powerFactor = CURRENT_SAMPLE.activePower * 100 / CURRENT_SAMPLE.apparentPower;
#endif
}

static void Meter_SetTimestamp(void)
{
	uint8_t value[REG_MAX_LENGTH];
	
	Meter_ProcessRegister(CurrentTime, value);
	CURRENT_SAMPLE.time.hour = ASCII_TO_BINARY(value[0]) * 10 + ASCII_TO_BINARY(value[1]);
	CURRENT_SAMPLE.time.minute = ASCII_TO_BINARY(value[3]) * 10 + ASCII_TO_BINARY(value[4]);
	CURRENT_SAMPLE.time.second = ASCII_TO_BINARY(value[6]) * 10 + ASCII_TO_BINARY(value[7]);
	CURRENT_SAMPLE.time.timestamp = CURRENT_SAMPLE.time.hour * 3600 +
		CURRENT_SAMPLE.time.minute * 60 +
		CURRENT_SAMPLE.time.second;
}

static void Meter_ShiftSamples(void)
{
	uint8_t i;
	
	for (i = 0; i < MAX_SAMPLES - 1; i++)
		memcpy(&Meter_handle.samples[i], &Meter_handle.samples[i + 1], sizeof(Sample_t));
}

static void Meter_SignalError(void)
{
	Message_t message;
	
	Meter_handle.failedAttempts++;
	if (Meter_handle.failedAttempts == MAX_ATTEMPTS)
		Meter_handle.state = METER_NOK;
	
	message.command = COMMAND_ERROR;
	message.argLength = 1;
	message.rawArgument[0] = ERROR_METER_NOK;
	LoRa_QueueMessage(message);
}
	
static void Meter_Write(Register_t reg)
{
	HAL_StatusTypeDef returnValue;
	
	returnValue = HAL_UART_Transmit(&Meter_handle.uartHandle, (uint8_t*) reg.name, reg.length, Meter_handle.timeout);
	if (returnValue != HAL_OK)
		Meter_SignalError();
}
