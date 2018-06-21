/* Includes ------------------------------------------------------------------*/
#include "actuator.h"
#include "hw_i2c.h"

/* Private typedef -----------------------------------------------------------*/
/* Private define ------------------------------------------------------------*/
#define ACT_ADDRESS		0x40

#define REG_INPUT				0
#define REG_OUTPUT			1
#define REG_POLARITY		2
#define REG_CONFIG			3

#define OUTPUTS_MASK		0xf0

/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
uint8_t inductors;
uint8_t capacitors;
uint8_t outputState;

/* Private function prototypes -----------------------------------------------*/
/* Public variables ----------------------------------------------------------*/
/* Functions Definition ------------------------------------------------------*/
void ACT_Init(void)
{
	inductors = 0;
	capacitors = 0;
	outputState = 0;
	
	/* Configure inputs and outputs */
	/* All output pins low */
	I2C_Write(ACT_ADDRESS, REG_OUTPUT, OUTPUTS_MASK);
	/* Set pins as output */
	I2C_Write(ACT_ADDRESS, REG_CONFIG, OUTPUTS_MASK);
}

void ACT_UpdateData(void)
{
	I2C_Read(ACT_ADDRESS, REG_INPUT);
}

void ACT_SetContact(uint8_t contactNumber, bool state)
{
	if (state)
		outputState |= OUTPUTS_MASK | (1 << contactNumber);
	else
		outputState = (outputState & ~(1 << contactNumber)) | OUTPUTS_MASK;
	I2C_Write(ACT_ADDRESS, REG_OUTPUT, outputState);
}

void ACT_ChangeActuator(uint8_t contactNumber, ActuatorType_t actuatorType, bool enabled)
{
	switch(actuatorType)
	{
		case ACT_INDUCTOR:
			if (enabled)
				inductors |= (1 << contactNumber);
			else
				inductors &= ~(1 << contactNumber);
			break;
		case ACT_CAPACITOR:
			if (enabled)
				capacitors |= (1 << contactNumber);
			else
				capacitors &= ~(1 << contactNumber);
			break;
		default:
			break;
	}
}
