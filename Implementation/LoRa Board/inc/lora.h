/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __LORA_H__
#define __LORA_H__

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
void LoRa_ForwardFrame(Frame_t frame);
uint8_t LoRa_GetAddress(void);
uint16_t LoRa_GetTransmissionRate(void);
void LoRa_Init(void);
void LoRa_ProcessRequest(Frame_t frame);
void LoRa_QueueMessage(Message_t message);

#endif /* __LORA_H__ */
