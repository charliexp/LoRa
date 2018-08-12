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
void Message_ArrayToFrame(uint8_t *array, Frame_t *frame)
{
	uint8_t i = IDX_FIRST_MESSAGE;
	
	frame->endDevice = array[IDX_DEVADDR];
	frame->nrOfMessages = 0;
	while (i < array[IDX_FRAME_LENGTH])
	{
		frame->messages[frame->nrOfMessages].command = (Command_t) array[i + IDX_COMMAND];
		frame->messages[frame->nrOfMessages].paramLength = array[i + IDX_ARG_LENGTH];
		memcpy(frame->messages[frame->nrOfMessages].params, array + i + IDX_ARGUMENT, frame->messages[frame->nrOfMessages].paramLength);
		i += MESSAGE_HEADER_SIZE + frame->messages[frame->nrOfMessages].paramLength;
		frame->nrOfMessages++;
	}
}

void Message_ArrayToMessage(Message_t *message, uint8_t *array)
{
	message->command = (Command_t) array[IDX_COMMAND];
	message->paramLength = array[IDX_ARG_LENGTH];
	memcpy(message->params, array + IDX_ARGUMENT, message->paramLength);
}

uint8_t Message_ArgLengthFromArray(uint8_t *array)
{
	return array[IDX_ARG_LENGTH];
}

void Message_FrameToArray(Frame_t frame, uint8_t *array, uint8_t *length)
{
	uint8_t m;
	
	array[IDX_DEVADDR] = frame.endDevice;
	(*length) = IDX_FIRST_MESSAGE;
	for (m = 0; m < frame.nrOfMessages; m++)
	{
		array[*length + IDX_COMMAND] = frame.messages[m].command;
		array[*length + IDX_ARG_LENGTH] = frame.messages[m].paramLength;
		memcpy(array + *length + IDX_ARGUMENT, frame.messages[m].params, frame.messages[m].paramLength);
		*length += MESSAGE_HEADER_SIZE + frame.messages[m].paramLength;
	}
	
	array[IDX_FRAME_LENGTH] = *length;
}
