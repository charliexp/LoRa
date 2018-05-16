/* Includes ------------------------------------------------------------------*/
#include "daq.h"
#include "timeServer.h"

/* Private typedef -----------------------------------------------------------*/

/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
static uint8_t MeterIDReq[] = "/?!\r\n";
static uint8_t MeterIDResp[] = "/AEM4TPN7205802041100\r\n";
static uint8_t MeterIDAck[] = {0x06, '0', '4', '0', '\r', '\n'};
static uint8_t ReadReqStart[] = {0x01, 'R', '1', 0x02};
static uint8_t ReadReqEnd = 0x03;

static TimerEvent_t DAQTimer;
static volatile DAQState_t DAQState;
static uint8_t DAQData[23];
static uint8_t DAQDataLength;

/* Private function prototypes -----------------------------------------------*/
/* DAQ timer callback function*/
static void DAQ_SendReq(uint8_t *request, uint8_t length);
static bool DAQ_RespOk(uint8_t *response);
static void OnDAQTimerEvent(void);

/* Functions Definition ------------------------------------------------------*/
DAQState_t DAQ_Init(void)
{
	int retries = 2;
	
	TimerInit( &DAQTimer, OnDAQTimerEvent );
	TimerSetValue( &DAQTimer,  APP_DAQ_TIMEOUT);
	
	do
	{
		DAQ_SendReq(MeterIDReq, 5);
		while (DAQState == DAQ_PENDING)
			if (UART_Receive(DAQData, &DAQDataLength) == UART_RX_AVAILABLE)
				DAQState = DAQ_IDLE;
		
		if (DAQState == DAQ_IDLE)
			if (!DAQ_RespOk(MeterIDResp))
				DAQState = DAQ_ERROR;
	}while (--retries > 0 && DAQState != DAQ_IDLE);
	
	HAL_Delay(3000);
	DAQ_SendReq(MeterIDAck, 6);
	
	HAL_Delay(5000);
	DAQ_ReadData(0x0001, 1, DAQData, &DAQDataLength);
	
	return DAQState;
}

void DAQ_ReadData(uint16_t address, uint8_t locations, uint8_t *response, uint8_t *length)
{
	uint8_t request[14];
	int i;
	
	for (i = 0; i < 4; i++)
		request[i] = ReadReqStart[i];
	request[4] = '0';
	request[5] = '.';
	request[6] = '9';
	request[7] = '.';
	request[8] = '1';
	request[9] = '(';
	request[10] = locations + '0';
	request[11] = ')';
	request[12] = ReadReqEnd;
	request[13] = 0xff;
	
	DAQ_SendReq(request, 13);
	while (DAQState == DAQ_PENDING)
		;
	
	for (i = 0; i < DAQDataLength; i++)
		response[i] = DAQData[i];
	*length = DAQDataLength;
}

void DAQ_SendReq(uint8_t *request, uint8_t length)
{
	DAQState = DAQ_PENDING;
	while (UART_Send(request, length) != UART_TX_AVAILABLE)
		;
  TimerStart(&DAQTimer);
}

bool DAQ_RespOk(uint8_t *response)
{
	int i;
	bool returnValue = true;
	
	for (i = 0; i < DAQDataLength; i++)
		if (DAQData[i] != response[i])
		{
			returnValue = false;
			break;
		}
		
	return returnValue;
}

static void OnDAQTimerEvent( void )
{
	if (DAQState == DAQ_PENDING)
			DAQState = DAQ_TIMEOUT;
}
