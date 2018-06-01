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
#define APP_DAQ_TIMEOUT                           5000
/* Exported types ------------------------------------------------------------*/
typedef struct Register_t
{
	char name[10];
	uint8_t length;
}Register_t;

typedef struct DAQTime_t
{
	uint8_t hour;
	uint8_t minute;
	uint8_t second;
}DAQTime_t;

/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void DAQ_Init(void);
void DAQ_UpdateData(void);
void DAQ_ReadData(Register_t reg, char *result, uint8_t *resultLength);
DAQTime_t DAQ_GetTime(void);

#ifdef __cplusplus
}
#endif

#endif /* __DAQ_H__*/
