/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __DAQ_H__
#define __DAQ_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include "app_conf.h"
#include "message.h"

/* Exported defines ----------------------------------------------------------*/
#define DAQ_TIMEOUT                           5000

/* Exported types ------------------------------------------------------------*/
typedef struct DAQTime_t
{
	uint8_t hour;
	uint8_t minute;
	uint8_t second;
}DAQTime_t;

typedef struct DAQ_Struct_t
{
	bool haveMeter;
	DAQTime_t time;
	uint16_t batteryLevel;			//Resolution: 0.01, Unit: V
	uint32_t activeEnergy;			//Resolution: 0.001, Unit: kWh
	uint32_t capacitiveEnergy;	//Resolution: 0.001, Unit: kVARh
	uint32_t inductiveEnergy;		//Resolution: 0.001, Unit: kVARh
	uint32_t reactiveEnergy;		//Resolution: 0.001, Unit: kVARh
	bool inductive;
	uint32_t activePower;				//Resolution: 0.001, Unit: kW
	uint32_t reactivePower;			//Resolution: 0.001, Unit: kVAR
	uint32_t apparentPower;			//Resolution: 0.001, Unit: kVA
	int8_t powerFactor;					//Resolution: 0.01
}DAQ_Struct_t;

/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
extern DAQ_Struct_t DAQ_Data;

/* Exported functions --------------------------------------------------------*/
void DAQ_Init(void);
void DAQ_Start(void);
void DAQ_MainLoop(void);
void DAQ_ReadData(void);
void DAQ_ProcessMessage(Message_t message);

#ifdef __cplusplus
}
#endif

#endif /* __DAQ_H__*/
