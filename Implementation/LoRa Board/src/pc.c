/* Includes ------------------------------------------------------------------*/
#include "compensator.h"
#include "meter.h"
#include "hw.h"
#include "pc.h"

/* Private typedef -----------------------------------------------------------*/
typedef enum PC_State_t
{
	READY,
	REC_FRAME_HEADER,
	REC_MESSAGE_HEADER,
	REC_MESSAGE,
}PC_State_t;

typedef struct PC_Handle_t
{
	//UartHandle_t *uartHandle;
	PC_State_t state;
	uint8_t buffer[FRAME_MAX_SIZE];
	uint8_t bufferLength;
	Frame_t receivedFrame;
}PC_Handle_t;

/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
extern uint32_t myAddress;

//static PC_Handle_t PC_Handle;

/* Private function prototypes -----------------------------------------------*/

/* Functions Definition ------------------------------------------------------*/
/*
void PC_Init(void)
{
  DBG_UartHandle.lowLevelHandle.Instance        = PC_USARTX;
  
  DBG_UartHandle.lowLevelHandle.Init.BaudRate   = 115200;
  DBG_UartHandle.lowLevelHandle.Init.WordLength = UART_WORDLENGTH_8B;
  DBG_UartHandle.lowLevelHandle.Init.StopBits   = UART_STOPBITS_1;
  DBG_UartHandle.lowLevelHandle.Init.Parity     = UART_PARITY_NONE;
  DBG_UartHandle.lowLevelHandle.Init.HwFlowCtl  = UART_HWCONTROL_NONE;
  DBG_UartHandle.lowLevelHandle.Init.Mode       = UART_MODE_TX_RX;
  
  if(HAL_UART_Init(&DBG_UartHandle.lowLevelHandle) != HAL_OK)
  {*/
    /* Initialization Error */
  /*  Error_Handler(); 
  }
	
	PC_Handle.uartHandle = &DBG_UartHandle;
	PC_Handle.state = READY;
}

void PC_MainLoop(void)
{
	UartRxState_t returnValue;
	uint8_t argLength;
	
	switch(PC_Handle.state)
	{
		case READY:
			returnValue = UART_ReceiveFixedLength(&DBG_UartHandle, PC_Handle.buffer, FRAME_HEADER_SIZE);
			if (returnValue == UART_RX_AVAILABLE)
			{
				PC_Handle.bufferLength += FRAME_HEADER_SIZE;
				PC_Handle.state = REC_FRAME_HEADER;
			}
			else if (returnValue == UART_RX_TIMEOUT)
				PC_Handle.state = READY;
			break;
		case REC_FRAME_HEADER:
			returnValue = UART_ReceiveFixedLength(&DBG_UartHandle, PC_Handle.buffer + PC_Handle.bufferLength, MESSAGE_HEADER_SIZE);
			if (returnValue == UART_RX_AVAILABLE)
			{
				PC_Handle.bufferLength += MESSAGE_HEADER_SIZE;
				PC_Handle.state = REC_MESSAGE_HEADER;
			}
			else if (returnValue == UART_RX_TIMEOUT)
				PC_Handle.state = READY;
			break;
		case REC_MESSAGE_HEADER:
			argLength = Message_argLengthFromArray(PC_Handle.buffer + FRAME_HEADER_SIZE);
			if (argLength != 0)
				returnValue = UART_ReceiveFixedLength(&DBG_UartHandle,
										PC_Handle.buffer + PC_Handle.bufferLength,
										argLength);
			else
				returnValue = UART_RX_AVAILABLE;
			if (returnValue == UART_RX_AVAILABLE)
			{
				PC_Handle.bufferLength += argLength;
				if (PC_Handle.bufferLength == Message_frameLengthFromArray(PC_Handle.buffer))
				{
					Message_arrayToFrame(PC_Handle.buffer, PC_Handle.bufferLength, &PC_Handle.receivedFrame);
					//PC_ProcessFrame(PC_Handle.receivedFrame);
					PC_Handle.state = READY;
				}
				else
					PC_Handle.state = REC_FRAME_HEADER;
			}
			else if (returnValue == UART_RX_TIMEOUT)
				PC_Handle.state = READY;
			break;
		default:
			PC_Handle.state = READY;
			break;
	}
}

void PC_Send(Frame_t frame)
{
	uint8_t array[FRAME_MAX_SIZE];
	uint8_t arrayLength = 0;
	
	Message_frameToArray(frame, array, &arrayLength);
	UART_Send(&DBG_UartHandle, array, arrayLength);
}*/
