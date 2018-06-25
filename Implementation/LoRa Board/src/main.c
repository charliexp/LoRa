#include "hw.h"
#include "compensator.h"
#include "lora.h"
#include "meter.h"
#include "pc.h"

int main(void)
{
	HAL_Init();
	SystemClock_Config();
	DBG_Init();
	HW_Init();
	
	Comp_Init();
	LoRa_Init();
	Meter_Init();
	PC_Init();
	
  while(1)
  {
  }
}
