/* Includes ------------------------------------------------------------------*/
#include "daq.h"
#include "timeServer.h"

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

static TimerEvent_t DAQTimer;
static volatile DAQState_t DAQState;
static uint8_t DAQData[23];
static uint8_t DAQDataLength;

/* Private function prototypes -----------------------------------------------*/
/* DAQ timer callback function*/
static void DAQ_SendReq(uint8_t *request, uint8_t length);
static void DAQ_WaitForResp(void);
static bool DAQ_RespOk(uint8_t *response);
static void OnDAQTimerEvent(void);

/* Functions Definition ------------------------------------------------------*/
DAQState_t DAQ_Init(void)
{
	TimerInit( &DAQTimer, OnDAQTimerEvent );
	TimerSetValue( &DAQTimer,  APP_DAQ_TIMEOUT);
	
	DAQ_SendReq(MeterIDReq, 5);
	DAQ_WaitForResp();
	
	if (DAQState != DAQ_TIMEOUT)
		if (!DAQ_RespOk(MeterIDResp))
			DAQState = DAQ_ERROR;
	
	DAQ_SendReq(DataReq, 6);
	DAQ_WaitForResp();
	if (DAQState != DAQ_TIMEOUT)
		if (!DAQ_RespOk(DataResp))
			DAQState = DAQ_ERROR;
	
	return DAQState;
}

void DAQ_ReadData(uint16_t address, uint8_t locations, uint8_t *response, uint8_t *length)
{
	uint8_t request[12];
	int i;
	
	for (i = 0; i < 4; i++)
		request[i] = ReadReqStart[i];
	request[4] = '1';
	request[5] = '2';
	request[6] = '3';
	request[7] = '4';
	request[8] = '(';
	request[9] = locations + '0';
	request[10] = ')';
	request[11] = ETX;
	
	DAQ_SendReq(request, 12);
	DAQ_WaitForResp();
	
	for (i = 0; i < DAQDataLength; i++)
		response[i] = DAQData[i];
	*length = DAQDataLength;
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

static void DAQ_SendReq(uint8_t *request, uint8_t length)
{
	//Add checksum
	if (request[0] != '/' && request[0] != ACK)
		request[length++] = DAQ_CalculateBCC(request);
			
	DAQState = DAQ_PENDING;
	while (UART_Send(request, length) != UART_TX_AVAILABLE)
		;
  TimerStart(&DAQTimer);
}

static void DAQ_WaitForResp(void)
{
	while (DAQState == DAQ_PENDING)
		if (UART_Receive(DAQData, &DAQDataLength) == UART_RX_AVAILABLE)
		{
			DAQState = DAQ_IDLE;
			TimerStop(&DAQTimer);
		}
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

static void OnDAQTimerEvent( void )
{
	if (DAQState == DAQ_PENDING)
			DAQState = DAQ_TIMEOUT;
}
