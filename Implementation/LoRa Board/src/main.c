#include "hw.h"
#include "compensator.h"
#include "lora.h"
#include "meter.h"
#include "pc.h"
#include "timeserver.h"

#ifdef GATEWAY
#define ACQUISITION_RATE							30

extern LoRaHandle_t handle;
static TimerEvent_t timer;

static void FillSampleWithValues(Sample_t *sample);
static void IsCompensationNeeded(Sample_t *sample);

static void BroadcastAcquisition(void)
{
	Message_t message;
	message.command = COMMAND_ACQUISITION;
	message.argLength = 0;
	LoRa_Broadcast(message);
	TimerStart(&timer);
}

void App_ProcessRequest(Frame_t frame)
{
	uint8_t i;
	EndNode_t *endNode;
	
	for (i = 0; i < RADIO_MAX_CONNECTIONS; i++)
		if (handle.endNodes[i].address == frame.endDevice)
		{
			endNode = &handle.endNodes[i];
			break;
		}
	
	for (i = 0; i < frame.nrOfMessages; i++)
	{
		switch (frame.messages[i].command)
		{
			case COMMAND_TIMESTAMP:
				endNode->sample.time.hour = frame.messages[i].rawArgument[0];
				endNode->sample.time.minute = frame.messages[i].rawArgument[1];
				endNode->sample.time.second = frame.messages[i].rawArgument[2];
				break;
			case COMMAND_ACTIVE_ENERGY:
				endNode->sample.activeEnergy = (frame.messages[i].rawArgument[0] << 16) |
					(frame.messages[i].rawArgument[1] << 8) |
					(frame.messages[i].rawArgument[2] << 0);
				break;
			case COMMAND_REACTIVE_ENERGY:
				endNode->sample.reactiveEnergy = (frame.messages[i].rawArgument[0] << 16) |
					(frame.messages[i].rawArgument[1] << 8) |
					(frame.messages[i].rawArgument[2] << 0);
				break;
			case COMMAND_ACTIVE_POWER:
				endNode->sample.activePower = (frame.messages[i].rawArgument[0] << 16) |
					(frame.messages[i].rawArgument[1] << 8) |
					(frame.messages[i].rawArgument[2] << 0);
				break;
			case COMMAND_REACTIVE_POWER:
				endNode->sample.reactivePower = (frame.messages[i].rawArgument[0] << 16) |
					(frame.messages[i].rawArgument[1] << 8) |
					(frame.messages[i].rawArgument[2] << 0);
				FillSampleWithValues(&handle.endNodes[frame.endDevice].sample);
				break;
		}
	}
}

void FillSampleWithValues(Sample_t *sample)
{
}

void IsCompensationNeeded(Sample_t *sample)
{
}
#endif

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
	
	#ifdef GATEWAY
	TimerInit(&timer, BroadcastAcquisition);
	TimerSetValue(&timer, ACQUISITION_RATE * 1000);
	BroadcastAcquisition();
	#endif
	
  while(1)
  {
		#ifdef END_NODE
		LoRa_MainLoop();
		#endif
  }
}
