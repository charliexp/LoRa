/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __HW_UART_H__
#define __HW_UART_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include "hw.h"

/* Exported types ------------------------------------------------------------*/
typedef enum UartRxState_t
{
	UART_RX_PENDING,
	UART_RX_AVAILABLE,
	UART_RX_TIMEOUT
}UartRxState_t;

/* Exported constants --------------------------------------------------------*/
#define UART_BUFFSIZE 1200

/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void UART_Init(void);
void UART_Send(uint8_t *buffer, uint8_t length);
UartRxState_t UART_Receive(uint8_t *buffer, uint16_t *length, uint8_t terminatorChar);
void UART_DeInit(void);

#ifdef __cplusplus
}
#endif

#endif /* __HW_UART_H__*/
