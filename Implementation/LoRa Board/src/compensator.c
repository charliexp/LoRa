/* Includes ------------------------------------------------------------------*/
#include "hw_i2c.h"

#include "compensator.h"
#include "eeprom.h"
#include "lora.h"

/* Private define ------------------------------------------------------------*/
#define I2C_ADDRESS											0x40
#define I2C_REG_INPUT										0
#define I2C_REG_OUTPUT									1
#define I2C_REG_POLARITY								2
#define I2C_REG_CONFIG									3

#define INPUTS_MASK											0xF0
#define OUTPUTS_MASK										0x0F

#define MAX_COMPENSATORS								4
#define MAX_ATTEMPTS										3

#define COMPENSATORS_EEPROM_LOCATION		0x08080010
#define COMPENSATOR_EEPROM_TYPE_OFFET		0
#define COMPENSATOR_EEPROM_VALUE_OFFET	2
#define COMPENSATOR_EEPROM_SIZE					4

/* Private typedef -----------------------------------------------------------*/
typedef enum CompensatorState_t
{
	COMP_OK,
	COMP_NOK,
}CompensatorState_t;

typedef struct Compensator_t
{
	uint8_t failedAttempts;
	CompensatorState_t state;
	CompensatorType_t type;
	uint16_t value;
}Compensator_t;

typedef struct CompensatorHandle_t
{
/* Index in array corresponds to output pin */
	Compensator_t outputs[MAX_COMPENSATORS];
/* Bit mask of all outputs sent to I2C */
	uint8_t i2c_output;
}CompensatorHandle_t;

/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
static CompensatorHandle_t Comp_handle;

/* Private function prototypes -----------------------------------------------*/
static void Comp_Change(uint8_t pin, uint16_t value, CompensatorType_t type);
static void Comp_CheckFeedback(void);
static void Comp_SignalError(Error_t type, uint8_t pin);

/* Public variables ----------------------------------------------------------*/
/* Functions Definition ------------------------------------------------------*/
static void Comp_Change(uint8_t pin, uint16_t value, CompensatorType_t type)
{
	Comp_handle.outputs[pin].state = COMP_OK;
	Comp_handle.outputs[pin].type = type;
	Comp_handle.outputs[pin].value = value;
	EEPROM_EraseWord(COMPENSATORS_EEPROM_LOCATION + COMPENSATOR_EEPROM_SIZE * pin);
	EEPROM_WriteByte(COMPENSATORS_EEPROM_LOCATION + COMPENSATOR_EEPROM_SIZE * pin + COMPENSATOR_EEPROM_TYPE_OFFET,
		Comp_handle.outputs[pin].type);
	EEPROM_WriteHalfWord(COMPENSATORS_EEPROM_LOCATION + COMPENSATOR_EEPROM_SIZE * pin + COMPENSATOR_EEPROM_VALUE_OFFET,
		Comp_handle.outputs[pin].value);
}

static void Comp_CheckFeedback(void)
{
	uint8_t i;
	uint8_t feedback;
	
	feedback = (I2C_Read(I2C_ADDRESS, I2C_REG_INPUT) >> 4) & OUTPUTS_MASK;
	for (i = 0; i < MAX_COMPENSATORS; i++)
		/* If a contact input is different from the expected output of the i-th compensator*/
		if (((feedback ^ Comp_handle.i2c_output) & OUTPUTS_MASK) == (1 << i))
			/* Mark it with error, and send error message */
		{
			Comp_handle.outputs[i].failedAttempts++;
			Comp_handle.outputs[i].state = COMP_NOK;
			Comp_SignalError(ERROR_COMPENSATOR_NOK, i);
		}
}

void Comp_Init(void)
{
	uint8_t i;
	
	/* Get compensator setup from EEPROM */
	for (i = 0; i < MAX_COMPENSATORS; i++)
	{
		Comp_handle.outputs[i].failedAttempts = 0;
		Comp_handle.outputs[i].state = COMP_OK;
		Comp_handle.outputs[i].type = (CompensatorType_t)(EEPROM_ReadByte(COMPENSATORS_EEPROM_LOCATION + COMPENSATOR_EEPROM_SIZE * i + COMPENSATOR_EEPROM_TYPE_OFFET));
		Comp_handle.outputs[i].value = (CompensatorType_t)(EEPROM_ReadHalfWord(COMPENSATORS_EEPROM_LOCATION + COMPENSATOR_EEPROM_SIZE * i + COMPENSATOR_EEPROM_VALUE_OFFET));
	}
	
	/* Set compensators initial state */
	/* All output pins low */
	Comp_handle.i2c_output = 0x00 | INPUTS_MASK;
	I2C_Write(I2C_ADDRESS, I2C_REG_OUTPUT, Comp_handle.i2c_output);
	/* Configure output pins */
	I2C_Write(I2C_ADDRESS, I2C_REG_CONFIG, ~OUTPUTS_MASK);
}

void Comp_ProcessRequest(Message_t message)
{
	switch (message.command)
	{
		case COMMAND_CHANGE_COMPENSATOR:
			Comp_Change((message.rawArgument[2] >> 4) & 0x0F,
				(message.rawArgument[0] << 8) || message.rawArgument[1],
				(CompensatorType_t)((message.rawArgument[2] >> 0) & 0x0F));
			break;
		case COMMAND_SET_COMPENSATOR:
			Comp_Set((message.rawArgument[0] >> 4) & 0x0F,
				(message.rawArgument[0] >> 0) & 0x0F);
			break;
		default:
			break;
	}
}

void Comp_Set(uint8_t pin, bool state)
{
	/* Check if compensator is working */
	if (Comp_handle.outputs[pin].state == COMP_OK ||
			Comp_handle.outputs[pin].failedAttempts < MAX_ATTEMPTS)
	{
		/* Set new state */
		if (state)
			Comp_handle.i2c_output |= (1 << pin);
		else
			Comp_handle.i2c_output &= ~(1 << pin);
		Comp_handle.i2c_output |= ~OUTPUTS_MASK;
		I2C_Write(I2C_ADDRESS, I2C_REG_OUTPUT, Comp_handle.i2c_output);
		
		/* Check if contact really closed */
		Comp_CheckFeedback();
	}
}

static void Comp_SignalError(Error_t type, uint8_t pin)
{
	Message_t message;
	
	message.command = COMMAND_ERROR;
	message.argLength = 1;
	message.rawArgument[0] = pin;
	
	LoRa_QueueMessage(message);
}
