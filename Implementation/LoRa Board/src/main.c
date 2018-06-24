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
	
	Frame_t frame;
	frame.endDevice = 0xAA;
	frame.nrOfMessages = 1;
	frame.messages[0].command = COMMAND_ACQUISITION;
	frame.messages[0].argLength = 0;
	
  while(1)
  {
		LoRa_ProcessRequest(frame);
		HAL_Delay(5000);
  }
}
