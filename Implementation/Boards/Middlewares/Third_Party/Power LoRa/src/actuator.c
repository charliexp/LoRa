/* Includes ------------------------------------------------------------------*/
#include "actuator.h"

/* Private typedef -----------------------------------------------------------*/
/* Private define ------------------------------------------------------------*/
#define ACT_ADDRESS		0x40

#define REG_INPUT				0
#define REG_OUTPUT			1
#define REG_POLARITY		2
#define REG_CONFIG			3

#define OUTPUTS_MASK		0xf0
#define OUTPUT_PIN(x)		(1 << (x))

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
	inductors = (uint8_t) ~OUTPUTS_MASK;
	capacitors = 0;
	outputState = 0;
	
	//configure inputs and outputs
	//all output pins 0
	I2C_Write(ACT_ADDRESS, REG_OUTPUT, OUTPUTS_MASK);
	//set pins as output
	I2C_Write(ACT_ADDRESS, REG_CONFIG, OUTPUTS_MASK);
}

void ACT_UpdateData(void)
{
	I2C_Read(ACT_ADDRESS, REG_INPUT);
}

void ACT_SetContact(uint8_t contactNumber, bool contactState)
{
	if (contactState)
		outputState |= OUTPUTS_MASK | (1 << contactNumber);
	else
		outputState = (outputState & ~(1 << contactNumber)) | OUTPUTS_MASK;
	I2C_Write(ACT_ADDRESS, REG_OUTPUT, outputState);
}
