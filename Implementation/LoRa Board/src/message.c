/* Includes ------------------------------------------------------------------*/
#include <string.h>
#include "message.h"

/* Private typedef -----------------------------------------------------------*/
#define IDX_FRAME_LENGTH		0
#define IDX_DEVADDR					1
#define IDX_FIRST_MESSAGE		FRAME_HEADER_SIZE

#define IDX_COMMAND					0
#define IDX_ARG_LENGTH			1
#define IDX_ARGUMENT				2

/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
/* Private function prototypes -----------------------------------------------*/
/* Functions Definition ------------------------------------------------------*/
void Message_fromArray(Message_t *message, uint8_t *array, uint8_t length)
{
	message->command = (Command_t) array[IDX_COMMAND];
	message->argLength = length - MESSAGE_HEADER_SIZE;
	memcpy(message->rawArgument, array + IDX_ARGUMENT, message->argLength);
}

void Message_toArray(Message_t message, uint8_t *array, uint8_t *length)
{
	array[IDX_COMMAND] = message.command;
	array[IDX_ARG_LENGTH] = message.argLength;
	memcpy(array + IDX_ARGUMENT, message.rawArgument, message.argLength);
	
	*length = message.argLength + MESSAGE_HEADER_SIZE;
}

uint8_t Message_frameLengthFromArray(uint8_t *array)
{
	return array[IDX_FRAME_LENGTH];
}

uint8_t Message_argLengthFromArray(uint8_t *array)
{
	return array[IDX_ARG_LENGTH];
}

void Message_frameToArray(Frame_t frame, uint8_t *array, uint8_t *length)
{
	uint8_t m;
	
	array[IDX_DEVADDR] = frame.endDevice;
	(*length) = IDX_FIRST_MESSAGE;
	for (m = 0; m < frame.nrOfMessages; m++)
	{
		array[*length + IDX_COMMAND] = frame.messages[m].command;
		array[*length + IDX_ARG_LENGTH] = frame.messages[m].argLength;
		memcpy(array + *length + IDX_ARGUMENT, frame.messages[m].rawArgument, frame.messages[m].argLength);
		*length += MESSAGE_HEADER_SIZE + frame.messages[m].argLength;
	}
	
	array[IDX_FRAME_LENGTH] = *length;
}

void Message_arrayToFrame(uint8_t *array, uint8_t length, Frame_t *frame)
{
	uint8_t i = IDX_FIRST_MESSAGE;
	
	frame->endDevice = array[IDX_DEVADDR];
	frame->nrOfMessages = 0;
	while (i < length)
	{
		frame->messages[frame->nrOfMessages].command = (Command_t) array[i + IDX_COMMAND];
		frame->messages[frame->nrOfMessages].argLength = array[i + IDX_ARG_LENGTH];
		memcpy(frame->messages[frame->nrOfMessages].rawArgument, array + i + IDX_ARGUMENT, frame->messages[frame->nrOfMessages].argLength);
		i += MESSAGE_HEADER_SIZE + frame->messages[frame->nrOfMessages].argLength;
		frame->nrOfMessages++;
	}
}
