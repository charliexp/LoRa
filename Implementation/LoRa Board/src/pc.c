/* Includes ------------------------------------------------------------------*/
#include "compensator.h"
#include "hw.h"
#include "lora.h"
#include "pc.h"

/* Private define ------------------------------------------------------------*/
#define UART_TIMEOUT						1

#define ADDRESS_PC							0xFF

/* Private typedef -----------------------------------------------------------*/
typedef enum State_t
{
	PENDING_FRAME_HEADER,
	PENDING_MESSAGE_HEADER,
	PENDING_FULL_MESSAGE,
}State_t;

typedef struct PCHandle_t
{
/* Uart handle */
	UART_HandleTypeDef hw;
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
static PCHandle_t handle;

/* Private function prototypes -----------------------------------------------*/
static void PC_ProcessRequest(void);

/* Functions Definition ------------------------------------------------------*/
void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart)	
{
	uint8_t argLength;
	
	switch (handle.state)
	{
		case PENDING_FRAME_HEADER:
			handle.state = PENDING_MESSAGE_HEADER;
			HAL_UART_Receive_IT(&handle.hw, handle.buffer + FRAME_HEADER_SIZE, MESSAGE_HEADER_SIZE);
			break;
		case PENDING_MESSAGE_HEADER:
			argLength = Message_ArgLengthFromArray(handle.buffer + FRAME_HEADER_SIZE);
			if (argLength != 0)
			{
				handle.state = PENDING_FULL_MESSAGE;
				HAL_UART_Receive_IT(&handle.hw, handle.buffer + FRAME_HEADER_SIZE + MESSAGE_HEADER_SIZE, argLength);
			}
			else
			{
				handle.state = PENDING_FRAME_HEADER;
				PC_ProcessRequest();
				HAL_UART_Receive_IT(&handle.hw, handle.buffer, FRAME_HEADER_SIZE);
			}
			break;
		case PENDING_FULL_MESSAGE:
			handle.state = PENDING_FRAME_HEADER;
			PC_ProcessRequest();
			HAL_UART_Receive_IT(&handle.hw, handle.buffer, FRAME_HEADER_SIZE);
			break;
	}
}	
	
void HAL_UART_ErrorCallback(UART_HandleTypeDef *huart)	
{
	handle.state = PENDING_FRAME_HEADER;
	HAL_UART_Receive_IT(&handle.hw, handle.buffer, FRAME_HEADER_SIZE);
}

void PC_Init(void)
{
  handle.hw.Instance = PC_USARTX;
  handle.hw.Init.BaudRate = 115200;
  handle.hw.Init.WordLength = UART_WORDLENGTH_8B;
  handle.hw.Init.StopBits = UART_STOPBITS_1;
  handle.hw.Init.Parity = UART_PARITY_NONE;
  handle.hw.Init.HwFlowCtl = UART_HWCONTROL_NONE;
  handle.hw.Init.Mode = UART_MODE_TX_RX;
	handle.state = PENDING_FRAME_HEADER;
	handle.timeout = UART_TIMEOUT;
  if(HAL_UART_Init(&handle.hw) != HAL_OK)
  {
    /* Initialization Error */
    Error_Handler(); 
  }
	
	HAL_NVIC_SetPriority(PC_USARTX_IRQn, 0x1, 0);	
	HAL_NVIC_EnableIRQ(PC_USARTX_IRQn);
	HAL_UART_Receive_IT(&handle.hw, handle.buffer, FRAME_HEADER_SIZE);
}

static void PC_ProcessRequest(void)
{
	Frame_t frame;
	Frame_t reply;
	Message_ArrayToFrame(handle.buffer, &frame);
	
	reply.endDevice = LoRa_GetAddress();
	reply.nrOfMessages = 1;
	reply.messages[0].command = frame.messages[0].command;
	
	if (frame.endDevice == ADDRESS_PC)
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
				LoRa_ProcessRequest(frame);
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
		LoRa_QueueMessage(frame.endDevice, Message_FromArray(handle.buffer + FRAME_HEADER_SIZE);
	}
#endif
}

void USART2_IRQHandler(void)		
{		
	HAL_UART_IRQHandler(&handle.hw);
}

void PC_Write(Frame_t frame)
{
	uint8_t array[FRAME_MAX_SIZE];
	uint8_t arrayLength = 0;
	
	Message_FrameToArray(frame, array, &arrayLength);
	HAL_UART_Transmit(&handle.hw, array, arrayLength, handle.timeout * 1000);
}
