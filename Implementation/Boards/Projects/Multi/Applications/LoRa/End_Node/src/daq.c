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

/* Private function prototypes -----------------------------------------------*/
/* DAQ timer callback function*/
static void OnDAQTimerEvent(void);

/* Functions Definition ------------------------------------------------------*/
DAQState_t DAQ_Init(void)
{
	int i;
	uint8_t data[23];
	uint8_t dataLength;
	DAQState_t returnValue = DAQ_ERROR;
	
	TimerInit( &DAQTimer, OnDAQTimerEvent );
	TimerSetValue( &DAQTimer,  APP_DAQ_DUTYCYCLE);
  TimerStart( &DAQTimer);
	
	while (returnValue == DAQ_ERROR)
	{
		UART_Send(MeterIDReq, 5);
		while (UART_Receive(data, &dataLength) != RX_AVAILABLE)
			;
		returnValue = DAQ_READY;
		
		for (i = 0; i < 23; i++)
			if (data[i] != MeterIDResp[i])
			{
				returnValue = DAQ_ERROR;
				break;
			}
		HAL_Delay(2000);
	}
	
	UART_Send((uint8_t* )"Data meter initialized\r\n", 24);
	
	return DAQ_READY;
}

static void OnDAQTimerEvent( void )
{
  /*Wait for next DAQ slot*/
  TimerStart( &DAQTimer);
}
