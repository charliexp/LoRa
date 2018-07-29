/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __METER_H__
#define __METER_H__

/* Includes ------------------------------------------------------------------*/
#include <stdint.h>
#include "message.h"

/* Exported defines ----------------------------------------------------------*/
/* Exported types ------------------------------------------------------------*/
typedef enum ReactivePower_t
{
	INDUCTIVE = 0x01,
	CAPACITIVE = 0x02,
}ReactivePower_t;

typedef struct Time_t
{
	uint8_t hour;
	uint8_t minute;
	uint8_t second;
	uint32_t timestamp;
}Time_t;

typedef struct Sample_t
{
	Time_t time;
/* Total values reported by meter */
	uint32_t activeEnergyTotal;
	uint32_t capacitiveEnergyTotal;
	uint32_t inductiveEnergyTotal;
/* Actual values used */
	uint32_t activeEnergy;			//kWh
	uint32_t reactiveEnergy;		//kVARh
	uint32_t capacitiveEnergy;	//kVARh
	uint32_t inductiveEnergy;		//kVARh
	ReactivePower_t powerType;	
	uint32_t activePower;				//kW
	uint32_t reactivePower;			//kVAR
#ifdef GATEWAY
	uint32_t apparentPower;			//Resolution: 0.001, Unit: kVA
	int8_t powerFactor;					//Resolution: 0.01
#endif
}Sample_t;

/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void Meter_Init(void);
void Meter_ProcessRequest(Message_t message);

#endif /* __METER_H__*/
