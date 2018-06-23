/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __METER_H__
#define __METER_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include <stdint.h>
#include "message.h"

/* Exported defines ----------------------------------------------------------*/
/* Exported types ------------------------------------------------------------*/
/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void Meter_Init(void);
void Meter_MainLoop(void);
void Meter_ProcessRequest(Message_t message);

#ifdef __cplusplus
}
#endif

#endif /* __METER_H__*/
