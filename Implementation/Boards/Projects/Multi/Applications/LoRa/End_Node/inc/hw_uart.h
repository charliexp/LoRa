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
    RX_PENDING,
		RX_AVAILABLE
}UartRxState_t;

typedef enum UartTxState_t
{
		TX_AVAILABLE,
    TX_BUSY,
}UartTxState_t;

/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void UART_Init(void);
UartTxState_t UART_Send(uint8_t *buffer, uint8_t length);
UartRxState_t UART_Receive(uint8_t *buffer, uint8_t *length);
void UART_DeInit(void);

#ifdef __cplusplus
}
#endif

#endif /* __HW_UART_H__*/
