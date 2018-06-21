/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __HW_I2C_H__
#define __HW_I2C_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include "app_conf.h"
#include "hw.h"

/* Exported types ------------------------------------------------------------*/
/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void I2C_Init(void);
void I2C_DeInit(void);
uint8_t I2C_Read(uint8_t deviceAddress, uint8_t registerAddress);
void I2C_Write(uint8_t deviceAddress, uint8_t registerAddress, uint8_t value);

#ifdef __cplusplus
}
#endif

#endif /* __HW_I2C_H__*/
