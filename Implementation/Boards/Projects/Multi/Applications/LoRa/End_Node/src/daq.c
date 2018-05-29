/* Includes ------------------------------------------------------------------*/
#include "daq.h"

/* Private typedef -----------------------------------------------------------*/
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
static uint8_t MeterIDResp[] = "/AEM4TPN7205802041100\r\n";
static uint8_t DataReq[] = {ACK, '0', '4', '0', CR, LF};
static uint8_t DataResp[] = {SOH, 'P', '0', STX, 
																'(', 'C', 'O', 'M', 'U', 'N', 'I', 'C', 'A', ')',
																ETX, 'q'};
static uint8_t ReadReqStart[] = {SOH, 'R', '1', STX};

static uint8_t DAQData[UART_BUFFSIZE];
static uint16_t DAQDataLength;

/* Private function prototypes -----------------------------------------------*/
static void DAQ_SendReq(uint8_t *request, uint16_t length);
static void DAQ_WaitForResp(uint16_t *length, uint8_t terminatorChar);
static bool DAQ_RespOk(uint8_t *response);

/* Functions Definition ------------------------------------------------------*/
void DAQ_Init(void)
{
}

void DAQ_GetData(void)
{
	uint16_t i;
	
	DAQ_SendReq(MeterIDReq, 5);
	DAQ_WaitForResp(&DAQDataLength, CR);
	
	for (i = 0; i < DAQDataLength; i++)
		PRINTF("%c", DAQData[i]);
	
	DAQ_SendReq(DataReq, 6);
	DAQ_WaitForResp(&DAQDataLength, ETX);
	
	for (i = 0; i < DAQDataLength; i++)
		PRINTF("%c", DAQData[i]);
}

void DAQ_ReadData(uint16_t address, uint8_t locations, uint8_t *response, uint8_t *length)
{
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

static void DAQ_SendReq(uint8_t *request, uint16_t length)
{
	//Add checksum
	if (request[0] != '/' && request[0] != ACK)
		request[length++] = DAQ_CalculateBCC(request);
			
	UART_Send(request, length);
}

static void DAQ_WaitForResp(uint16_t *length, uint8_t terminatorChar)
{
	UartRxState_t result;
	do
	{
		result = UART_Receive(DAQData, length, terminatorChar);
	}while (result == UART_RX_PENDING);
	
	if (result == UART_RX_TIMEOUT)
		PRINTF("Timeout contor");
}

static bool DAQ_RespOk(uint8_t *response)
{
	int i;
	bool returnValue = true;
	
	if (response[0] == '/')
	{
		for (i = 0; i < DAQDataLength; i++)
			if (DAQData[i] != response[i])
			{
				returnValue = false;
				break;
			}
	}
	else
		if (DAQ_CalculateBCC(DAQData) != DAQData[DAQDataLength - 1])
			returnValue = false;
		
	return returnValue;
}
