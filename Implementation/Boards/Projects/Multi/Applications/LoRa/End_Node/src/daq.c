/* Includes ------------------------------------------------------------------*/
#include "daq.h"
#include "timeServer.h"

/* Private typedef -----------------------------------------------------------*/

/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
static uint8_t MeterIDReq[] = "/?!\r\n";
static uint8_t MeterIDResp[] = "/AEM4TPN7205802041100\r\n";
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
			;
		if (DAQState == DAQ_IDLE)
			if (!DAQ_RespOk(MeterIDResp))
				DAQState = DAQ_ERROR;
	}while (--retries > 0 && DAQState != DAQ_IDLE);
	
	return DAQState;
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
	{
		if (UART_Receive(DAQData, &DAQDataLength) == UART_RX_AVAILABLE)
			DAQState = DAQ_IDLE;
		else
			DAQState = DAQ_TIMEOUT;
	}
}
