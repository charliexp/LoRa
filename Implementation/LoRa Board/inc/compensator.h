/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __COMPENSATOR_H__
#define __COMPENSATOR_H__

/* Includes ------------------------------------------------------------------*/
#include <stdbool.h>
#include <stdint.h>

/* Exported defines ----------------------------------------------------------*/
#define I2C_TIMEOUT						1
#define MAX_ATTEMPTS					3
#define MAX_COMPENSATORS			4

#define COMPENSATOR_LOCATION	0x08080010

/* Exported types ------------------------------------------------------------*/
typedef enum Comp_error_e
{
	COMP_OK = 0,
	COMP_I2C_NAK = 0xC0,
	COMP_FEEDBACK_NOK = 0xC1,
}Comp_error_e;

/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void Comp_Init(void);
void Comp_Update(uint8_t pin, uint16_t value, uint8_t type);
Comp_error_e Comp_Set(uint8_t pin, bool state);

#endif /* __COMPENSATOR_H__*/
