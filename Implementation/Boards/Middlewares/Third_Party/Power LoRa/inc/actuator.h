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
/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void ACT_Init(void);
void ACT_UpdateData(void);
void ACT_SetContact(uint8_t contactNumber, bool contactState);

#ifdef __cplusplus
}
#endif

#endif /* __ACTUATOR_H__*/
