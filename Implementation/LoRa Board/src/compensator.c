/* Includes ------------------------------------------------------------------*/
#include "compensator.h"
#include "eeprom.h"
#include "hw.h"
#ifdef TEST
#include <stdlib.h>
#endif

/* Private define ------------------------------------------------------------*/
#define I2C_ADDRESS						0x40
#define REG_INPUT							0
#define REG_OUTPUT						1
#define REG_POLARITY					2
#define REG_CONFIG						3

#define INPUTS_MASK						0xF0
#define OUTPUTS_MASK					0x0F

#define EEPROM_TYPE_OFFSET		0
#define EEPROM_PIN_OFFSET			1
#define EEPROM_VALUE_OFFSET		2
#define EEPROM_SIZE						4

#define ERROR_TIMEOUT					0xFF

/* Private typedef -----------------------------------------------------------*/
typedef enum Comp_type_e
{
	COMP_UNUSED = 0x00,
	COMP_INDUCTOR = 0x01,
	COMP_CAPACITOR = 0x02,
}Comp_type_e;

typedef enum Comp_state_e
{
	COMP_DISCONNECTED = 0,
	COMP_CONNECTED = 1,
}Comp_state_e;

typedef struct Comp_t
{
/* Type */
	Comp_type_e type;
/* Output pin */
	uint8_t pin;
/* Reactive power */
	uint16_t value;
/* Failed attempts to set compensator */
	uint8_t failedAttempts;
/* Working state */
	Comp_state_e state;
}Comp_t;

typedef struct Comp_handle_t
{
/* I2C comp_handle */
	I2C_HandleTypeDef hw;
/* Bit mask of all outputs sent to I2C */
	uint8_t i2c_output;
/* Timeout in seconds */
	uint16_t timeout;
/* Array of existing */
	Comp_t outputs[MAX_COMPENSATORS];
/* Number of existing compensators */
	uint8_t outputsLength;
}Comp_handle_t;

/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
Comp_handle_t comp_handle;

/* Private functions -----------------------------------------------*/
static void Comp_Save(uint8_t idx)
{
	EEPROM_EraseWord(COMPENSATOR_LOCATION + EEPROM_SIZE * idx);
	EEPROM_WriteByte(COMPENSATOR_LOCATION + EEPROM_SIZE * idx + EEPROM_TYPE_OFFSET, comp_handle.outputs[idx].type);
	EEPROM_WriteByte(COMPENSATOR_LOCATION + EEPROM_SIZE * idx + EEPROM_PIN_OFFSET, comp_handle.outputs[idx].pin);
	EEPROM_WriteHalfWord(COMPENSATOR_LOCATION + EEPROM_SIZE * idx + EEPROM_VALUE_OFFSET, comp_handle.outputs[idx].value);
}

static void Comp_Load()
{
	uint8_t i;
	Comp_t output;
	
	/* Get compensator setup from EEPROM */
	for (i = 0; i < MAX_COMPENSATORS; i++)
	{
		output.type = (Comp_type_e)(EEPROM_ReadByte(COMPENSATOR_LOCATION + EEPROM_SIZE * i + EEPROM_TYPE_OFFSET));
		if (output.type != COMP_UNUSED)
		{
			output.pin = (Comp_type_e)(EEPROM_ReadByte(COMPENSATOR_LOCATION + EEPROM_SIZE * i + EEPROM_PIN_OFFSET));
			output.value = (Comp_type_e)(EEPROM_ReadHalfWord(COMPENSATOR_LOCATION + EEPROM_SIZE * i + EEPROM_VALUE_OFFSET));
			output.failedAttempts = 0;
			output.state = COMP_DISCONNECTED;
			memcpy(&comp_handle.outputs[i], &output, sizeof(output));
		}
		else
			break;
	}
	
	comp_handle.outputsLength = i;
}

static Comp_error_e Comp_Read(uint8_t reg, uint8_t *data)
{
	HAL_StatusTypeDef returnValue;
	
	returnValue = HAL_I2C_Mem_Read(&comp_handle.hw, I2C_ADDRESS, reg, I2C_MEMADD_SIZE_8BIT, data, 1, comp_handle.timeout * 1000);
	if (returnValue == HAL_OK)
		return COMP_OK;
	else
		return COMP_I2C_NAK;
}

static Comp_error_e Comp_Write(uint8_t reg, uint8_t data)
{
	HAL_StatusTypeDef returnValue;
	
	returnValue = HAL_I2C_Mem_Write(&comp_handle.hw, I2C_ADDRESS, reg, I2C_MEMADD_SIZE_8BIT, &data, 1, comp_handle.timeout * 1000);
	if (returnValue == HAL_OK)
		return COMP_OK;
	else
		return COMP_I2C_NAK;
}

static uint8_t Comp_GetIndex(uint8_t pin)
{
	uint8_t i;
	/* Find index of requested compensator if it exists */
	for (i = 0; i < comp_handle.outputsLength; i++)
		if (comp_handle.outputs[i].pin == pin)
		{
			break;
		}

	return i;
}

static Comp_error_e Comp_CheckFeedback(void)
{
	Comp_error_e returnValue = COMP_OK;
	uint8_t i;
	uint8_t feedback;
	
	returnValue = Comp_Read(REG_INPUT, &feedback);
	
	#ifdef TEST
	if (rand() % 100 > TEST_CHANCE_TO_FAIL)
		returnValue = COMP_OK;
	#endif
	
	if (returnValue == COMP_OK)
	{
		feedback = (feedback >> 4) & OUTPUTS_MASK;
		for (i = 0; i < comp_handle.outputsLength; i++)
		{
			if (((feedback ^ comp_handle.i2c_output) & (1 << i)) != 0)
			/* If a contact input is different from the expected output of the i-th compensator*/
			{
				/* Mark it with error, and send error message */
				comp_handle.outputs[i].failedAttempts++;
				returnValue = COMP_FEEDBACK_NOK;
			}
			else
			{
				comp_handle.outputs[i].failedAttempts = 0;
				comp_handle.outputs[i].state = COMP_DISCONNECTED;
			}
		}
	}
	
	return returnValue;
}

/* Public Functions ----------------------------------------------------------*/
void Comp_Init(void)
{
	/* Init I2C */
	comp_handle.hw.Instance = COMP_I2C;
	comp_handle.hw.Init.AddressingMode = I2C_ADDRESSINGMODE_7BIT;
	comp_handle.hw.Init.DualAddressMode = I2C_DUALADDRESS_DISABLE;
	comp_handle.hw.Init.GeneralCallMode = I2C_GENERALCALL_DISABLE;
	comp_handle.hw.Init.NoStretchMode = I2C_NOSTRETCH_DISABLE;
	comp_handle.hw.Init.Timing = 0x10420F13;
	comp_handle.timeout = I2C_TIMEOUT;
	if(HAL_I2C_Init(&comp_handle.hw) != HAL_OK)
	{
		/* Initialization Error */
		Error_Handler(); 
	}
	
	/* Init compensator struct */
	comp_handle.outputsLength = 0;
	Comp_Load();
	
	/* Set compensators initial state */
	/* All output pins low */
	comp_handle.i2c_output = 0x00 | INPUTS_MASK;
	Comp_Write(REG_OUTPUT, comp_handle.i2c_output);
	/* Configure pins as outputs */
	Comp_Write(REG_CONFIG, comp_handle.i2c_output);
}

void Comp_Update(uint8_t pin, uint16_t value, uint8_t type)
{
	uint8_t i;
	i = Comp_GetIndex(pin);
		
	/* New compensator */
	if (i == comp_handle.outputsLength)
	{
		comp_handle.outputsLength++;
		comp_handle.outputs[i].pin = pin;
		comp_handle.outputs[i].state = COMP_DISCONNECTED;
	}
	
	/* Save its parameters */
	comp_handle.outputs[i].type = (Comp_type_e)type;
	comp_handle.outputs[i].value = value;
	Comp_Save(i);
}

Comp_error_e Comp_Set(uint8_t pin, bool state)
{
	Comp_error_e returnValue;
	uint8_t i;
	i = Comp_GetIndex(pin);
		
	/* Compensator exists */
	if (i != comp_handle.outputsLength)
	{
		/* Set new state */
		if (state)
		{
			comp_handle.outputs[i].state = COMP_CONNECTED;
			comp_handle.i2c_output |= (1 << pin);
		}
		else
		{
			comp_handle.outputs[i].state = COMP_DISCONNECTED;
			comp_handle.i2c_output &= ~(1 << pin);
		}
		comp_handle.i2c_output |= ~OUTPUTS_MASK;
		returnValue = Comp_Write(REG_OUTPUT, comp_handle.i2c_output);
		
		#ifdef TEST
		if (rand() % 100 > TEST_CHANCE_TO_FAIL)
			returnValue = COMP_OK;
		#endif
	
		if (returnValue == COMP_OK)
			/* Check if contact really closed */
			returnValue = Comp_CheckFeedback();
	}
	
	#ifdef TEST
	if (rand() % 100 > TEST_CHANCE_TO_FAIL)
		returnValue = COMP_OK;
	#endif
	
	return returnValue;
}
