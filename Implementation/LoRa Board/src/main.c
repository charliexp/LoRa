#include "hw.h"
#include "compensator.h"
#include "lora.h"
#include "meter.h"
#include "pc.h"

static void PC_SignalPressence(void);

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
	
	PC_SignalPressence();
	
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

static void PC_SignalPressence(void)
{
	Frame_t frame;
	frame.endDevice = LoRa_GetAddress();
	frame.nrOfMessages = 1;
	frame.messages[0].command = COMMAND_IS_PRESENT;
	frame.messages[0].argLength = 2;
	frame.messages[0].rawArgument[0] = (LoRa_GetTransmissionRate() >> 8) & 0xFF;
	frame.messages[0].rawArgument[1] = (LoRa_GetTransmissionRate() >> 0) & 0xFF;
	PC_Write(frame);
}
