/* Includes ------------------------------------------------------------------*/
#include "compensator.h"
#include "eeprom.h"
#include "hw.h"
#include "lora.h"

/* Private define ------------------------------------------------------------*/
#define I2C_TIMEOUT						1
#define MAX_ATTEMPTS					3
#define MAX_COMPENSATORS			4

#define I2C_ADDRESS						0x40
#define REG_INPUT							0
#define REG_OUTPUT						1
#define REG_POLARITY					2
#define REG_CONFIG						3

#define INPUTS_MASK						0xF0
#define OUTPUTS_MASK					0x0F

#define EEPROM_LOCATION				0x08080010
#define EEPROM_TYPE_OFFET			0
#define EEPROM_VALUE_OFFET		2
#define EEPROM_SIZE						4

#define ERROR_TIMEOUT					0xFF

/* Private typedef -----------------------------------------------------------*/
typedef enum State_t
{
	COMP_OK,
	COMP_NOK
}State_t;

typedef struct Compensator_t
{
	uint8_t failedAttempts;
	State_t state;
	CompensatorType_t type;
	uint16_t value;
}Compensator_t;

typedef struct CompensatorHandle_t
{
/* I2C handle */
	I2C_HandleTypeDef i2cHandle;
/* Index in array corresponds to output pin */
	Compensator_t outputs[MAX_COMPENSATORS];
/* Bit mask of all outputs sent to I2C */
	uint8_t i2c_output;
/* Timeout in seconds */
	uint16_t timeout;
}CompensatorHandle_t;

/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
static CompensatorHandle_t Comp_handle;

/* Private function prototypes -----------------------------------------------*/
static void Comp_Change(uint8_t pin, uint16_t value, CompensatorType_t type);
static void Comp_CheckFeedback(void);
static void Comp_Read(uint8_t reg, uint8_t *data);
static void Comp_SignalError(uint8_t pin);
static void Comp_Write(uint8_t reg, uint8_t data);

/* Functions Definition ------------------------------------------------------*/
static void Comp_Change(uint8_t pin, uint16_t value, CompensatorType_t type)
{
	Comp_handle.outputs[pin].state = COMP_OK;
	Comp_handle.outputs[pin].type = type;
	Comp_handle.outputs[pin].value = value;
	EEPROM_EraseWord(EEPROM_LOCATION + EEPROM_SIZE * pin);
	EEPROM_WriteByte(EEPROM_LOCATION + EEPROM_SIZE * pin + EEPROM_TYPE_OFFET,
		Comp_handle.outputs[pin].type);
	EEPROM_WriteHalfWord(EEPROM_LOCATION + EEPROM_SIZE * pin + EEPROM_VALUE_OFFET,
		Comp_handle.outputs[pin].value);
}

static void Comp_CheckFeedback(void)
{
	uint8_t i;
	uint8_t feedback;
	
	Comp_Read(REG_INPUT, &feedback);
	
	feedback = (feedback >> 4) & OUTPUTS_MASK;
	for (i = 0; i < MAX_COMPENSATORS; i++)
		/* If a contact input is different from the expected output of the i-th compensator*/
		if (((feedback ^ Comp_handle.i2c_output) & OUTPUTS_MASK) == (1 << i))
			/* Mark it with error, and send error message */
		{
			Comp_handle.outputs[i].failedAttempts++;
			Comp_handle.outputs[i].state = COMP_NOK;
			Comp_SignalError(i);
		}
}

void Comp_Init(void)
{
	uint8_t i;
	
	/* Init I2C */
	Comp_handle.i2cHandle.Instance = COMP_I2C;
	Comp_handle.i2cHandle.Init.AddressingMode = I2C_ADDRESSINGMODE_7BIT;
	Comp_handle.i2cHandle.Init.DualAddressMode = I2C_DUALADDRESS_DISABLE;
	Comp_handle.i2cHandle.Init.GeneralCallMode = I2C_GENERALCALL_DISABLE;
	Comp_handle.i2cHandle.Init.NoStretchMode = I2C_NOSTRETCH_DISABLE;
	Comp_handle.i2cHandle.Init.Timing = 0x10420F13;	
	Comp_handle.timeout = I2C_TIMEOUT;
	if(HAL_I2C_Init(&Comp_handle.i2cHandle) != HAL_OK)
	{
		/* Initialization Error */
		Error_Handler(); 
	}
	
	/* Get compensator setup from EEPROM */
	for (i = 0; i < MAX_COMPENSATORS; i++)
	{
		Comp_handle.outputs[i].failedAttempts = 0;
		Comp_handle.outputs[i].state = COMP_OK;
		Comp_handle.outputs[i].type = (CompensatorType_t)(EEPROM_ReadByte(EEPROM_LOCATION + EEPROM_SIZE * i + EEPROM_TYPE_OFFET));
		Comp_handle.outputs[i].value = (CompensatorType_t)(EEPROM_ReadHalfWord(EEPROM_LOCATION + EEPROM_SIZE * i + EEPROM_VALUE_OFFET));
	}
	
	/* Set compensators initial state */
	/* All output pins low */
	Comp_handle.i2c_output = 0x00 | INPUTS_MASK;
	Comp_Write(REG_OUTPUT, Comp_handle.i2c_output);
	/* Configure output pins */
	Comp_Write(REG_CONFIG, Comp_handle.i2c_output);
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

static void Comp_Read(uint8_t reg, uint8_t *data)
{
	HAL_StatusTypeDef returnValue;
	
	returnValue = HAL_I2C_Mem_Read(&Comp_handle.i2cHandle, I2C_ADDRESS, reg, I2C_MEMADD_SIZE_8BIT, data, 1, Comp_handle.timeout);
	if (returnValue != HAL_OK)
		Comp_SignalError(ERROR_TIMEOUT);
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
		Comp_Write(REG_OUTPUT, Comp_handle.i2c_output);
		
		/* Check if contact really closed */
		Comp_CheckFeedback();
	}
}

static void Comp_SignalError(uint8_t pin)
{
	Message_t message;
	
	message.command = COMMAND_ERROR;
	message.argLength = 2;
	message.rawArgument[0] = ERROR_COMPENSATOR_NOK;
	message.rawArgument[1] = pin;
	
	LoRa_QueueMessage(message);
}

static void Comp_Write(uint8_t reg, uint8_t data)
{
	HAL_StatusTypeDef returnValue;
	
	returnValue = HAL_I2C_Mem_Write(&Comp_handle.i2cHandle, I2C_ADDRESS, reg, I2C_MEMADD_SIZE_8BIT, &data, 1, Comp_handle.timeout);
	if (returnValue != HAL_OK)
		Comp_SignalError(ERROR_TIMEOUT);
}
