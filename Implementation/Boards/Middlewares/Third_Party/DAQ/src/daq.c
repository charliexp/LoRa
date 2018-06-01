/* Includes ------------------------------------------------------------------*/
#include "daq.h"

/* Private typedef -----------------------------------------------------------*/
typedef struct Register_t
{
	char name[10];
	uint8_t length;
}Register_t;

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

static Register_t BatteryLevelRegister = { "C.6.3", 5 };
static Register_t CurrentTimeRegister = { "0.9.1", 5 };

static uint8_t DAQData[UART_BUFFSIZE];
static uint16_t DAQDataLength;

/* Private function prototypes -----------------------------------------------*/
static void DAQ_SendReq(uint8_t *request, uint16_t length);
static UartRxState_t DAQ_WaitForResp(uint16_t *length, uint8_t terminatorChar);
static uint8_t DAQ_CalculateBCC(uint8_t *message);
static void DAQ_ReadData(Register_t reg, char *result, uint8_t *resultLength);
static void DAQ_GetTime(void);
static void DAQ_GetBatteryLevel(void);

/* Public variables ----------------------------------------------------------*/
DAQ_Struct_t DAQ_Data = {0, 0, 0, 0};

/* Functions Definition ------------------------------------------------------*/
void DAQ_Init(void)
{
	DAQDataLength = 0;
}

void DAQ_UpdateData(void)
{
	DAQ_SendReq(MeterIDReq, 5);
	DAQ_WaitForResp(&DAQDataLength, CR);
	
	DAQ_SendReq(DataReq, 6);
	if (DAQ_WaitForResp(&DAQDataLength, ETX) != UART_RX_TIMEOUT)
	{
		DAQ_GetTime();
		DAQ_GetBatteryLevel();
	}
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
	
	while (DAQData[i] != '!' && i < DAQDataLength - 1)
	{
		while ((DAQData[i] == reg.name[j]) && (j != reg.length - 1) && i < DAQDataLength - 1)
		{
			i++;
			j++;
		}
		if (j == reg.length - 1)
		{
			//Found register
			i += 2;
			while (DAQData[i] != ')' && DAQData[i] != '*' && i < DAQDataLength - 1)
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

static void DAQ_GetTime(void)
{
	char value[10];
	uint8_t valueLength;
	uint8_t hour, minute, second;
	
	DAQ_ReadData(CurrentTimeRegister, value, &valueLength);
	
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
	
	DAQ_ReadData(BatteryLevelRegister, value, &valueLength);
	
	if (value[0] >= '0' && value[0] <= '9' &&
			value[2] >= '0' && value[2] <= '9' &&
			value[3] >= '0' && value[3] <= '9')
	{
		DAQ_Data.batteryLevel = (value[0] - '0') * 100 +
														(value[2] - '0') * 10 +
														(value[3] - '0');
	}
}
