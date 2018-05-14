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
#define APP_DAQ_DUTYCYCLE                           2000
/* Exported types ------------------------------------------------------------*/
typedef enum DAQState_t
{
    DAQ_READY,
		DAQ_ERROR
}DAQState_t;

/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
DAQState_t DAQ_Init(void);

#ifdef __cplusplus
}
#endif

#endif /* __DAQ_H__*/
