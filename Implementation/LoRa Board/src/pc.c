/* Includes ------------------------------------------------------------------*/
#include "hw.h"
#include "pc.h"
#include "vcom.h"
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
	bool connected;
	uint8_t buffer[FRAME_MAX_SIZE];
	uint8_t bufferLength;
	PC_State_t state;
}PC_Handle_t;

/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
extern UartHandle_t DBG_UartHandle;
extern uint8_t myAddress;

static PC_Handle_t PC_Handle;
static Frame_t receivedFrame;

/* Private function prototypes -----------------------------------------------*/
/* Functions Definition ------------------------------------------------------*/

void PC_Init(void)
{
	PC_Handle.connected = false;
	PC_Handle.state = READY;
}

void PC_MainLoop(void)
{
	UartRxState_t returnValue;
	uint8_t argLength;
	
	switch(PC_Handle.state)
	{
		case READY:
			PC_Handle.bufferLength = 0;
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
			returnValue = UART_ReceiveFixedLength(&DBG_UartHandle, PC_Handle.buffer + FRAME_HEADER_SIZE, MESSAGE_HEADER_SIZE);
			if (returnValue == UART_RX_AVAILABLE)
			{
				PC_Handle.bufferLength += FRAME_HEADER_SIZE;
				PC_Handle.state = REC_MESSAGE_HEADER;
			}
			else if (returnValue == UART_RX_TIMEOUT)
				PC_Handle.state = READY;
			break;
		case REC_MESSAGE_HEADER:
			argLength = Message_argLengthFromArray(PC_Handle.buffer + FRAME_HEADER_SIZE);
			if (argLength != 0)
				returnValue = UART_ReceiveFixedLength(&DBG_UartHandle,
										PC_Handle.buffer + MESSAGE_HEADER_SIZE,
										argLength);
			else
				returnValue = UART_RX_AVAILABLE;
			if (returnValue == UART_RX_AVAILABLE)
			{
				PC_Handle.bufferLength += argLength;
				Message_arrayToFrame(PC_Handle.buffer, PC_Handle.bufferLength, &receivedFrame);
				PC_ProcessMessage();
				PC_Handle.state = READY;
			}
			else if (returnValue == UART_RX_TIMEOUT)
				PC_Handle.state = READY;
			break;
		default:
			PC_Handle.state = READY;
			break;
	}
}

void PC_ProcessMessage(void)
{
	Frame_t reply;
	
	reply.endDevice = myAddress;
	reply.nrOfMessages = 1;
	
	reply.messages[0].command = receivedFrame.messages[0].command;
	
	switch (receivedFrame.messages[0].command)
	{
		case COMMAND_IS_PRESENT:
			reply.messages[0].argLength = 1;
			reply.messages[0].rawArgument[0] = ACK;
			PC_Handle.connected = true;
			PC_Send(reply);
			break;/*
		case COMMAND_SET_ADDRESS:
			setAddress(parameters[3]);
			response[0] = RESPONSE_ACK;
			PC_Send(source, myAddress, command, response, 6);
			break;*/
		default:
			reply.messages[0].argLength = 1;
			reply.messages[0].rawArgument[0] = NAK;
			PC_Send(reply);
			break;
	}
}

void PC_Send(Frame_t frame)
{
	uint8_t array[FRAME_MAX_SIZE];
	uint8_t arrayLength = 0;
	
	Message_frameToArray(frame, array, &arrayLength);
	UART_Send(&DBG_UartHandle, array, arrayLength);
}

bool PC_Connected(void)
{
	return PC_Handle.connected;
}
