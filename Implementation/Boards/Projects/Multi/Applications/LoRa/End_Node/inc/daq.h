/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __DAQ_H__
#define __DAQ_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include "hw_uart.h"

/* Exported defines ----------------------------------------------------------*/
/*!
 * Defines the application data aquisition duty cycle. 2s, value in [ms].
 */
#define APP_DAQ_TIMEOUT                           4000
/* Exported types ------------------------------------------------------------*/
typedef enum DAQState_t
{
	DAQ_IDLE,
	DAQ_PENDING,
	DAQ_TIMEOUT,
	DAQ_ERROR
}DAQState_t;

/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
DAQState_t DAQ_Init(void);
void DAQ_ReadData(uint16_t address, uint8_t locations, uint8_t *response, uint8_t *length);

#ifdef __cplusplus
}
#endif

#endif /* __DAQ_H__*/
