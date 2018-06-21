/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __MESSAGE_H__
#define __MESSAGE_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include "hw.h"

/* Exported defines ----------------------------------------------------------*/
#define ADDRESS_BROADCAST							0xAA
#define ADDRESS_END_DEVICE						1
#define ADDRESS_PC										0xFF

/*
#define COMMANDS_COMMUNICATION_MASK		0x10
#define COMMANDS_SETUP_MASK						0x20
#define COMMANDS_SCADA_MASK						0x30
*/
#define ACK														0x01
#define NAK														0x00

#define FRAME_HEADER_SIZE							2
#define MESSAGE_HEADER_SIZE						2
#define MESSAGE_ARG_MAX_SIZE					4
#define MESSAGE_MAX_SIZE							(MESSAGE_HEADER_SIZE + MESSAGE_ARG_MAX_SIZE)
#define FRAME_MAX_MESSAGES						4
#define FRAME_MAX_SIZE								(FRAME_HEADER_SIZE + FRAME_MAX_MESSAGES * MESSAGE_MAX_SIZE)

/* Exported types ------------------------------------------------------------*/
typedef enum Command_t
{
	COMMAND_IS_PRESENT = 0x10,
	COMMAND_RESEND = 0x11,
	COMMAND_RESET = 0x12,
	
	COMMAND_SET_ADDRESS = 0x20,
	COMMAND_HAS_METER = 0x21,
	COMMAND_CHANGE_COMPENSATOR = 0x22,
	
	COMMAND_ACQUISITION = 0x30,
	COMMAND_TIMESTAMP = 0x31,
	COMMAND_ACTIVE_ENERGY = 0x32,
	COMMAND_REACTIVE_ENERGY = 0x33,
	COMMAND_REACTIVE_POWER = 0x34,
	COMMAND_SET_COMPENSATOR = 0x35,
}Command_t;

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
void Message_fromArray(Message_t *message, uint8_t *array, uint8_t length);
void Message_toArray(Message_t message, uint8_t *array, uint8_t *length);
uint8_t Message_frameLengthFromArray(uint8_t *array);
uint8_t Message_argLengthFromArray(uint8_t *array);
void Message_frameToArray(Frame_t frame, uint8_t *array, uint8_t *length);
void Message_arrayToFrame(uint8_t *array, uint8_t length, Frame_t *frame);

#ifdef __cplusplus
}
#endif

#endif /* __MESSAGE_H__*/
