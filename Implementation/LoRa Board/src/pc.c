/* Includes ------------------------------------------------------------------*/
#include "compensator.h"
#include "hw.h"
#include "lora.h"
#include "pc.h"

/* Private define ------------------------------------------------------------*/
#define UART_TIMEOUT						1

/* Private typedef -----------------------------------------------------------*/
typedef enum State_t
{
	PENDING,
	RECEIVED_FRAME_HEADER,
	RECEIVED_MESSAGE_HEADER,
	RECEIVED_FULL_MESSAGE,
}State_t;

typedef struct PCHandle_t
{
/* Uart handle */
	UART_HandleTypeDef uartHandle;
/* Uart receive buffer */
	uint8_t buffer[FRAME_MAX_SIZE];
/* Frame receive state */
	State_t state;
/* Timeout in seconds */
	uint16_t timeout;
}PCHandle_t;

/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
static PCHandle_t PC_Handle;

/* Private function prototypes -----------------------------------------------*/
static void PC_ProcessRequest(void);

/* Functions Definition ------------------------------------------------------*/
void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart)	
{
	switch (PC_Handle.state)
	{
		case PENDING:
			HAL_UART_Receive_IT(&PC_Handle.uartHandle,
				PC_Handle.buffer,
				FRAME_HEADER_SIZE);
			break;
		case RECEIVED_FRAME_HEADER:
			HAL_UART_Receive_IT(&PC_Handle.uartHandle,
				PC_Handle.buffer + FRAME_HEADER_SIZE,
				MESSAGE_HEADER_SIZE);
			break;
		case RECEIVED_MESSAGE_HEADER:
			HAL_UART_Receive_IT(&PC_Handle.uartHandle,
				PC_Handle.buffer + FRAME_HEADER_SIZE + MESSAGE_HEADER_SIZE,
				Message_ArgLengthFromArray(PC_Handle.buffer + FRAME_HEADER_SIZE));
			break;
		case RECEIVED_FULL_MESSAGE:
			PC_ProcessRequest();
			break;
	}
}	
	
void HAL_UART_ErrorCallback(UART_HandleTypeDef *huart)	
{
	PC_Handle.state = PENDING;
	HAL_UART_Receive_IT(&PC_Handle.uartHandle, PC_Handle.buffer, FRAME_HEADER_SIZE);
}

void PC_Init(void)
{
  PC_Handle.uartHandle.Instance = PC_USARTX;
  PC_Handle.uartHandle.Init.BaudRate = 115200;
  PC_Handle.uartHandle.Init.WordLength = UART_WORDLENGTH_8B;
  PC_Handle.uartHandle.Init.StopBits = UART_STOPBITS_1;
  PC_Handle.uartHandle.Init.Parity = UART_PARITY_NONE;
  PC_Handle.uartHandle.Init.HwFlowCtl = UART_HWCONTROL_NONE;
  PC_Handle.uartHandle.Init.Mode = UART_MODE_TX_RX;
	PC_Handle.state = PENDING;
	PC_Handle.timeout = UART_TIMEOUT;
  if(HAL_UART_Init(&PC_Handle.uartHandle) != HAL_OK)
  {
    /* Initialization Error */
    Error_Handler(); 
  }
	
	HAL_NVIC_SetPriority(PC_USARTX_IRQn, 0x1, 0);	
	HAL_NVIC_EnableIRQ(PC_USARTX_IRQn);
	HAL_UART_Receive_IT(&PC_Handle.uartHandle, PC_Handle.buffer, FRAME_HEADER_SIZE);
}

static void PC_ProcessRequest(void)
{
	Frame_t frame;
	Frame_t reply;
	Message_ArrayToFrame(PC_Handle.buffer, &frame);
	
	reply.endDevice = LoRa_GetAddress();
	reply.nrOfMessages = 1;
	reply.messages[0].command = frame.messages[0].command;
	
	if (frame.endDevice == LoRa_GetAddress())
	{
		switch (frame.messages[0].command)
		{
			case COMMAND_IS_PRESENT:
				reply.messages[0].argLength = 2;
				reply.messages[0].rawArgument[0] = (LoRa_GetTransmissionRate() >> 8) & 0xFF;
				reply.messages[0].rawArgument[1] = (LoRa_GetTransmissionRate() >> 0) & 0xFF;
				PC_Write(reply);
				break;
			case COMMAND_SET_ADDRESS:
				reply.endDevice = LoRa_GetAddress();
			case COMMAND_TRANSMISSION_RATE:
				LoRa_ProcessRequest(frame.messages[0]);
				reply.messages[0].argLength = 1;
				reply.messages[0].rawArgument[0] = ACK;
				PC_Write(reply);
				break;
			case COMMAND_SET_COMPENSATOR:
				Comp_ProcessRequest(frame.messages[0]);
				reply.messages[0].argLength = 1;
				reply.messages[0].rawArgument[0] = ACK;
				break;
			default:
				reply.messages[0].argLength = 1;
				reply.messages[0].rawArgument[0] = NAK;
				PC_Write(reply);
				break;
		}
	}
#ifdef GATEWAY
	else
	{
		LoRa_QueueMessage(frame.endDevice, Message_FromArray(PC_Handle.buffer + FRAME_HEADER_SIZE);
	}
#endif
}

void PC_Write(Frame_t frame)
{
	uint8_t array[FRAME_MAX_SIZE];
	uint8_t arrayLength = 0;
	
	Message_FrameToArray(frame, array, &arrayLength);
	HAL_UART_Transmit(&PC_Handle.uartHandle, array, arrayLength, PC_Handle.timeout);
}
