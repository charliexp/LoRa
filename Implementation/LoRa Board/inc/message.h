/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __MESSAGE_H__
#define __MESSAGE_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include <stdint.h>

/* Exported defines ----------------------------------------------------------*/
#define ACK														0x55
#define NAK														0x66

#define MESSAGE_HEADER_SIZE						2
#define MESSAGE_ARG_MAX_SIZE					4
#define MESSAGE_MAX_SIZE							(MESSAGE_HEADER_SIZE + MESSAGE_ARG_MAX_SIZE)
#define FRAME_HEADER_SIZE							2
#define FRAME_MAX_MESSAGES						5
#define FRAME_MAX_SIZE								(FRAME_HEADER_SIZE + FRAME_MAX_MESSAGES * MESSAGE_MAX_SIZE)

/* Exported types ------------------------------------------------------------*/
typedef enum Command_t
{
	COMMAND_ERROR = 0x10,
	
	COMMAND_SET_ADDRESS = 0x20,
	COMMAND_CHANGE_COMPENSATOR = 0x21,
	
	COMMAND_ACQUISITION = 0x30,
	COMMAND_TIMESTAMP = 0x31,
	COMMAND_ACTIVE_ENERGY = 0x32,
	COMMAND_REACTIVE_ENERGY = 0x33,
	COMMAND_ACTIVE_POWER = 0x34,
	COMMAND_REACTIVE_POWER = 0x35,
	COMMAND_SET_COMPENSATOR = 0x36,
}Command_t;

typedef enum Error_t
{
	ERROR_RESEND = 0x40,
	ERROR_RESET = 0x41,
	ERROR_METER_NOK = 0x42,
	ERROR_COMPENSATOR_NOK = 0x43,
	ERROR_LORA_NOK = 0x44,
	ERROR_LORA_TIMEOUT = 0x45,
}Error_t;

typedef struct Message_t
{
	Command_t command;
	uint8_t argLength;
	uint8_t rawArgument[MESSAGE_ARG_MAX_SIZE];
}Message_t;

typedef struct Frame_t
{
	uint8_t endDevice;
	Message_t messages[FRAME_MAX_MESSAGES];
	uint8_t nrOfMessages;
}Frame_t;

/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void Message_ArrayToFrame(uint8_t *array, Frame_t *frame);
void Message_ArrayToMessage(Message_t *message, uint8_t *array);
uint8_t Message_ArgLengthFromArray(uint8_t *array);
void Message_FrameToArray(Frame_t frame, uint8_t *array, uint8_t *length);

#ifdef __cplusplus
}
#endif

#endif /* __MESSAGE_H__*/
