/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __PC_H__
#define __PC_H__

/* Includes ------------------------------------------------------------------*/
#include "message.h"

/* Exported defines ----------------------------------------------------------*/
/* Exported types ------------------------------------------------------------*/
/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void PC_Init(void);
void PC_Write(Frame_t frame);

#endif /* __PC_H__ */
