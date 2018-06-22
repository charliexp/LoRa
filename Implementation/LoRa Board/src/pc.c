/* Includes ------------------------------------------------------------------*/
#include "actuator.h"
#include "daq.h"
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
	UartHandle_t *uartHandle;
	PC_State_t state;
	uint8_t buffer[FRAME_MAX_SIZE];
	uint8_t bufferLength;
	Frame_t receivedFrame;
	bool connected;
}PC_Handle_t;

/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
extern UartHandle_t DBG_UartHandle;
extern uint8_t myAddress;

static PC_Handle_t PC_Handle;

/* Private function prototypes -----------------------------------------------*/
extern void setAddress(uint8_t newAddress);

/* Functions Definition ------------------------------------------------------*/

void PC_Init(void)
{
	PC_Handle.uartHandle = &DBG_UartHandle;
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
					PC_ProcessFrame(PC_Handle.receivedFrame);
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

void PC_ProcessFrame(Frame_t frame)
{
	Frame_t reply;
	
	reply.endDevice = myAddress;
	reply.nrOfMessages = 1;
	
	reply.messages[0].command = frame.messages[0].command;
	
	switch (frame.messages[0].command)
	{
		case COMMAND_IS_PRESENT:
			reply.messages[0].argLength = 2;
			reply.messages[0].rawArgument[0] = (appTransmissionRate >> 8) & 0xFF;
			reply.messages[0].rawArgument[1] = (appTransmissionRate >> 0) & 0xFF;
			PC_Send(reply);
			PC_Handle.connected = true;
			break;
		case COMMAND_SET_ADDRESS:
			setAddress(frame.messages[0].rawArgument[0]);
			reply.endDevice = myAddress;
			reply.messages[0].argLength = 1;
			reply.messages[0].rawArgument[0] = ACK;
			PC_Send(reply);
			break;
		case COMMAND_TRANSMISSION_RATE:
			appTransmissionRate = ((frame.messages[0].rawArgument[0] >> 8) & 0xFF) |
				((frame.messages[0].rawArgument[1] >> 0) & 0xFF);
			reply.messages[0].argLength = 1;
			reply.messages[0].rawArgument[0] = ACK;
			PC_Send(reply);
			break;
		case COMMAND_HAS_METER:
			DAQ_ProcessMessage(frame.messages[0]);
			break;
		case COMMAND_SET_COMPENSATOR:
			ACT_ProcessMessage(frame.messages[0]);
			break;
		default:
			reply.messages[0].argLength = 1;
			reply.messages[0].rawArgument[0] = NAK;
			PC_Send(reply);
			break;
	}
	PC_Handle.bufferLength = 0;
}

void PC_Send(Frame_t frame)
{
	uint8_t array[FRAME_MAX_SIZE];
	uint8_t arrayLength = 0;
	
	if (PC_Handle.connected)
	{
		Message_frameToArray(frame, array, &arrayLength);
		UART_Send(&DBG_UartHandle, array, arrayLength);
	}
}
