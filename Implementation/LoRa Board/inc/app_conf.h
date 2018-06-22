/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __APP_CONF_H__
#define __APP_CONF_H__

#ifdef __cplusplus
 extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include <stdbool.h>
#include <stdint.h>

/* Exported defines ----------------------------------------------------------*/
#define APP_INITIAL_SAMPLE_RATE			10
#define DAQ_SAMPLE_RATE							10

/* Exported types ------------------------------------------------------------*/
/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
extern uint8_t myAddress;
extern uint16_t appTransmissionRate;
/* Exported macros -----------------------------------------------------------*/
/* Exported functions ------------------------------------------------------- */ 

#ifdef __cplusplus
}
#endif

#endif /* __APP_CONF_H__*/
