/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __COMPENSATOR_H__
#define __COMPENSATOR_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include <stdbool.h>
#include <stdint.h>
#include "message.h"

/* Exported defines ----------------------------------------------------------*/
/* Exported types ------------------------------------------------------------*/
typedef enum CompensatorType_t
{
	COMP_UNUSED = 0x00,
	COMP_INDUCTOR = 0x01,
	COMP_CAPACITOR = 0x02,
}CompensatorType_t;

typedef enum State_t
{
	COMP_IN,
	COMP_OUT,
	COMP_NOK
}State_t;

typedef struct Compensator_t
{
/* Failed attempts to set compensator */
	uint8_t failedAttempts;
/* Working state */
	State_t state;
/* Type */
	CompensatorType_t type;
/* Reactive power */
	uint16_t value;
}Compensator_t;

/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void Comp_Init(void);
void Comp_ProcessRequest(Message_t message);
void Comp_Set(uint8_t outputNumber, bool state);

#ifdef __cplusplus
}
#endif

#endif /* __COMPENSATOR_H__*/
