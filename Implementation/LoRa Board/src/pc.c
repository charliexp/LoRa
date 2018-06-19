/* Includes ------------------------------------------------------------------*/
#include "hw.h"
#include "pc.h"
#include "vcom.h"
/* Private typedef -----------------------------------------------------------*/
typedef enum PC_State_t
{
	READY,
	REC_COMMAND,
	REC_LENGTH,
	REC_DATA,
}PC_State_t;

/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
extern UartHandle_t DBG_UartHandle;
extern uint8_t myAddress;

static uint8_t PC_Buffer[UART_BUFFSIZE];
static PC_State_t PC_State;
static Message_t PC_Message;

/* Private function prototypes -----------------------------------------------*/
/* Functions Definition ------------------------------------------------------*/

void PC_Init(void)
{
	PC_State = READY;
}

void PC_MainLoop(void)
{
	UartRxState_t returnValue;
	
	switch(PC_State)
	{
		case READY:
			returnValue = UART_ReceiveFixedLength(&DBG_UartHandle, PC_Buffer, 3);
			if (returnValue != UART_RX_PENDING)
			{
				if (returnValue == UART_RX_AVAILABLE)
				{
					PC_Message.source = PC_Buffer[IDX_SOURCE_ADDRESS];
					PC_Message.target = PC_Buffer[IDX_TARGET_ADDRESS];
					PC_Message.command = (Commands_t) PC_Buffer[IDX_COMMAND];
					PC_State = REC_COMMAND;
					//DBG_PRINTF("PC command received\r\n");
				}
				else if (returnValue == UART_RX_TIMEOUT)
				{
					PC_State = READY;
					//DBG_PRINTF("PC command timeout\r\n");
				}
			}
			break;
		case REC_COMMAND:
			returnValue = UART_ReceiveFixedLength(&DBG_UartHandle, PC_Buffer, 1);
			if (returnValue != UART_RX_PENDING)
			{
				if (returnValue == UART_RX_AVAILABLE)
				{
					PC_Message.paramLength = PC_Buffer[0];
					PC_State = REC_LENGTH;
					//DBG_PRINTF("PC length received\r\n");
				}
				else if (returnValue == UART_RX_TIMEOUT)
				{
					PC_State = READY;
					//DBG_PRINTF("PC length timeout\r\n");
				}
			}
			break;
		case REC_LENGTH:
			returnValue = UART_ReceiveFixedLength(&DBG_UartHandle, PC_Buffer, PC_Message.paramLength);
			if (returnValue != UART_RX_PENDING)
			{
				if (returnValue == UART_RX_AVAILABLE)
				{
					memcpy(PC_Message.rawParameter, PC_Buffer, PC_Message.paramLength);
					PC_State = REC_DATA;
					//DBG_PRINTF("PC length received\r\n");
				}
				else if (returnValue == UART_RX_TIMEOUT)
				{
					PC_State = READY;
					//DBG_PRINTF("PC length timeout\r\n");
				}
			}
			break;
		case REC_DATA:
			PC_ProcessMessage();
			break;
		default:
			PC_State = READY;
			break;
	}
}

void PC_ProcessMessage(void)
{
	Message_t reply;
	reply.source = myAddress;
	reply.target = ADDRESS_PC;
	reply.command = PC_Message.command;
	
	switch (PC_Message.command)
	{
		case COMMAND_GET_ADDRESS:
			reply.paramLength = 1;
			reply.rawParameter[0] = RESPONSE_ACK;
			PC_Send(reply);
			break;
		/*case COMMAND_SET_ADDRESS:
			setAddress(parameters[3]);
			response[0] = RESPONSE_ACK;
			PC_Send(source, myAddress, command, response, 6);
			break;*/
		default:
			reply.paramLength = 1;
			reply.rawParameter[0] = RESPONSE_NACK;
			PC_Send(reply);
			break;
	}
	
	PC_State = READY;
}

void PC_Send(Message_t message)
{
	uint8_t length = IDX_PARAMETER;
	uint8_t array[IDX_PARAMETER + message.paramLength];
	
	array[IDX_SOURCE_ADDRESS] = message.source;
	array[IDX_TARGET_ADDRESS] = message.target;
	array[IDX_COMMAND] = message.command;
	array[IDX_PARAM_LENGTH] = message.paramLength;
	memcpy(array + IDX_PARAMETER, message.rawParameter, message.paramLength);
	length += message.paramLength;
	
	UART_Send(&DBG_UartHandle, array, length);
}
