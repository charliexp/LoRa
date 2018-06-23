/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __HW_UART_H__
#define __HW_UART_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include "hw.h"

/* Exported defines ----------------------------------------------------------*/
#define UART_BUFFSIZE 1100

/* Exported types ------------------------------------------------------------*/
typedef enum UartRxState_t
{
	UART_RX_PENDING,
	UART_RX_AVAILABLE,
	UART_RX_TIMEOUT
}UartRxState_t;
typedef struct UartHandle_t
{
/* Uart Handle */
	UART_HandleTypeDef lowLevelHandle;
/* Receive state */
	UartRxState_t rxState;
/* Receive buffer */
	uint8_t rxBuffer[UART_BUFFSIZE];
/* Index of first byte not forwarded to upper layer */
	uint16_t rxProcessedIndex;
/* Index of last byte checked for terminator character */
	uint16_t rxCheckedIndex;
/* Index of byte to be received */
	uint16_t rxReceiveIndex;
/* Timestamp of last sent byte */
	uint32_t lastSendTime;
/* Timestamp of last read byte */
	uint32_t lastReceiveTime;
/* Receive timeout in seconds */
	uint16_t timeout;
}UartHandle_t;

/* Exported constants --------------------------------------------------------*/

/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void UART_Init(void);
void UART_Send(UartHandle_t *uart, uint8_t *buffer, uint8_t length);
UartRxState_t UART_Receive(UartHandle_t *uart, uint8_t *buffer, uint16_t length);
UartRxState_t UART_ReceiveUntilChar(UartHandle_t *uart, uint8_t *buffer, uint16_t *length, uint8_t terminatorChar, uint8_t interruptChar);
UartRxState_t UART_ReceiveFixedLength(UartHandle_t *uart, uint8_t *buffer, uint16_t length);
void UART_DeInit(void);

#ifdef __cplusplus
}
#endif

#endif /* __HW_UART_H__*/
