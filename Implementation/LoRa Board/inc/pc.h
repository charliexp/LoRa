/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __PC_H__
#define __PC_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include "message.h"

/* Exported defines ----------------------------------------------------------*/
/* Exported types ------------------------------------------------------------*/
/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void PC_Init(void);
void PC_MainLoop(void);
void PC_ProcessMessage(void);
void PC_Send(Frame_t frame);
bool PC_Connected(void);

#ifdef __cplusplus
}
#endif

#endif /* __PC_H__ */
