/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __ACTUATOR_H__
#define __ACTUATOR_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include "hw_i2c.h"

/* Exported defines ----------------------------------------------------------*/
/* Exported types ------------------------------------------------------------*/
typedef enum ActuatorType_t
{
	ACT_INDUCTOR,
	ACT_CAPACITOR
}ActuatorType_t;
	 
/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void ACT_Init(void);
void ACT_UpdateData(void);
void ACT_SetContact(uint8_t contactNumber, bool state);
void ACT_ChangeActuator(uint8_t contactNumber, ActuatorType_t actuatorType, bool enabled);

#ifdef __cplusplus
}
#endif

#endif /* __ACTUATOR_H__*/
