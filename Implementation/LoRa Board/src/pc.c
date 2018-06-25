/* Includes ------------------------------------------------------------------*/
#include "compensator.h"
#include "hw.h"
#include "lora.h"
#include "pc.h"

/* Private define ------------------------------------------------------------*/
#define UART_TIMEOUT						1

#define ADDRESS_PC							0xFF

/* Private typedef -----------------------------------------------------------*/
typedef enum PCState_t
{
	PENDING_FRAME_HEADER,
	PENDING_MESSAGE_HEADER,
	PENDING_FULL_MESSAGE,
}PCState_t;

typedef struct PCHandle_t
{
/* Uart pcHandle */
	UART_HandleTypeDef hw;
/* Uart receive buffer */
	uint8_t buffer[FRAME_MAX_SIZE];
/* Frame receive state */
	PCState_t state;
/* Timeout in seconds */
	uint16_t timeout;
}PCHandle_t;

/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
static PCHandle_t pcHandle;

/* Private function prototypes -----------------------------------------------*/
static void PC_ProcessRequest(void);

/* Functions Definition ------------------------------------------------------*/
void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart)	
{
	uint8_t argLength;
	
	switch (pcHandle.state)
	{
		case PENDING_FRAME_HEADER:
			pcHandle.state = PENDING_MESSAGE_HEADER;
			HAL_UART_Receive_IT(&pcHandle.hw, pcHandle.buffer + FRAME_HEADER_SIZE, MESSAGE_HEADER_SIZE);
			break;
		case PENDING_MESSAGE_HEADER:
			argLength = Message_ArgLengthFromArray(pcHandle.buffer + FRAME_HEADER_SIZE);
			if (argLength != 0)
			{
				pcHandle.state = PENDING_FULL_MESSAGE;
				HAL_UART_Receive_IT(&pcHandle.hw, pcHandle.buffer + FRAME_HEADER_SIZE + MESSAGE_HEADER_SIZE, argLength);
			}
			else
			{
				pcHandle.state = PENDING_FRAME_HEADER;
				PC_ProcessRequest();
				HAL_UART_Receive_IT(&pcHandle.hw, pcHandle.buffer, FRAME_HEADER_SIZE);
			}
			break;
		case PENDING_FULL_MESSAGE:
			pcHandle.state = PENDING_FRAME_HEADER;
			PC_ProcessRequest();
			HAL_UART_Receive_IT(&pcHandle.hw, pcHandle.buffer, FRAME_HEADER_SIZE);
			break;
	}
}	
	
void HAL_UART_ErrorCallback(UART_HandleTypeDef *huart)	
{
	pcHandle.state = PENDING_FRAME_HEADER;
	HAL_UART_Receive_IT(&pcHandle.hw, pcHandle.buffer, FRAME_HEADER_SIZE);
}

void PC_Init(void)
{
  pcHandle.hw.Instance = PC_USARTX;
  pcHandle.hw.Init.BaudRate = 115200;
  pcHandle.hw.Init.WordLength = UART_WORDLENGTH_8B;
  pcHandle.hw.Init.StopBits = UART_STOPBITS_1;
  pcHandle.hw.Init.Parity = UART_PARITY_NONE;
  pcHandle.hw.Init.HwFlowCtl = UART_HWCONTROL_NONE;
  pcHandle.hw.Init.Mode = UART_MODE_TX_RX;
	pcHandle.state = PENDING_FRAME_HEADER;
	pcHandle.timeout = UART_TIMEOUT;
  if(HAL_UART_Init(&pcHandle.hw) != HAL_OK)
  {
    /* Initialization Error */
    Error_Handler(); 
  }
	
	HAL_NVIC_SetPriority(PC_USARTX_IRQn, 0x1, 0);	
	HAL_NVIC_EnableIRQ(PC_USARTX_IRQn);
	HAL_UART_Receive_IT(&pcHandle.hw, pcHandle.buffer, FRAME_HEADER_SIZE);
}

static void PC_ProcessRequest(void)
{
	Frame_t frame;
	Frame_t reply;
	Message_ArrayToFrame(pcHandle.buffer, &frame);
	
	if (frame.endDevice == LoRa_GetAddress())
	{
		reply.endDevice = LoRa_GetAddress();
		reply.nrOfMessages = 1;
		reply.messages[0].command = frame.messages[0].command;
	
		switch (frame.messages[0].command)
		{
			case COMMAND_SET_ADDRESS:
				reply.endDevice = LoRa_GetAddress();
				LoRa_ProcessRequest(frame);
				break;
			case COMMAND_CHANGE_COMPENSATOR:
			case COMMAND_SET_COMPENSATOR:
				Comp_ProcessRequest(frame.messages[0]);
				reply.messages[0].argLength = 1;
				reply.messages[0].rawArgument[0] = ACK;
				PC_Write(reply);
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
		LoRa_ForwardFrame(frame);
#endif
}

void USART2_IRQHandler(void)		
{		
	HAL_UART_IRQHandler(&pcHandle.hw);
}

void PC_Write(Frame_t frame)
{
	uint8_t array[FRAME_MAX_SIZE];
	uint8_t arrayLength = 0;
	
	Message_FrameToArray(frame, array, &arrayLength);
	HAL_UART_Transmit(&pcHandle.hw, array, arrayLength, pcHandle.timeout * 1000);
}
