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
 * Defines the data aquisition timeout. Value in [ms].
 */
#define APP_DAQ_TIMEOUT                           5000
/* Exported types ------------------------------------------------------------*/
typedef struct DAQTime_t
{
	uint8_t hour;
	uint8_t minute;
	uint8_t second;
}DAQTime_t;

typedef struct DAQ_Struct_t
{
	DAQTime_t time;
	uint16_t batteryLevel;			//Resolution: 0.01, Unit: V
}DAQ_Struct_t;

/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
extern DAQ_Struct_t DAQ_Data;

/* Exported functions --------------------------------------------------------*/
void DAQ_Init(void);
void DAQ_UpdateData(void);

#ifdef __cplusplus
}
#endif

#endif /* __DAQ_H__*/
