#ifndef COMMANDS_H
#define COMMANDS_H

#define ADDRESS_GENERAL								0
#define ADDRESS_MASTER								1
#define ADDRESS_BEACON								2
#define ADDRESS_PC										0xff

#define IDX_SOURCE_ADDRESS						0
#define IDX_TARGET_ADDRESS						1
#define IDX_COMMAND										2
#define IDX_COMMAND_PARAMETER					3

#define PARAMETERS_MAX_SIZE						4

#define COMMANDS_COMMUNICATION_MASK		0x10
#define COMMANDS_PC_MASK							0x20
#define COMMANDS_RADIO_SETUP_MASK			0x40

typedef enum Commands_t
{
	COMMAND_IS_PRESENT = 0x10,
	COMMAND_RESEND = 0x11,
	
	COMMAND_GET_ADDRESS = 0x20,
	COMMAND_SET_ADDRESS = 0x21,
	
	COMMAND_BANDWIDTH = 0x40,
	COMMAND_OUTPUT_POWER = 0x41,
	COMMAND_SPREAD_FACTOR = 0x42,
	COMMAND_CODING_RATE = 0x43,
	COMMAND_RX_SYM_TIMEOUT = 0x44,
	COMMAND_RX_MS_TIMEOUT = 0x45,
	COMMAND_TX_TIMEOUT =0x46,
	COMMAND_PREAMBLE_SIZE = 0x47,
	COMMAND_PAYLOAD_MAX_SIZE = 0x48,
	COMMAND_VARIABLE_PAYLOAD = 0x49,
	COMMAND_PERFORM_CRC = 0x4a,
}Commands_t;

typedef enum Responses_t
{
	RESPONSE_ACK = 1,
	RESPONSE_NACK = 0xff
}Responses_t;

#endif /* COMMANDS_H */