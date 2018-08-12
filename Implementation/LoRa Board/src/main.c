#include "hw.h"
#include "compensator.h"
#include "lora.h"
#include "meter.h"
#include "pc.h"
#include "timeserver.h"

#ifdef GATEWAY
#define ACQUISITION_RATE										30

#define CAPACITIVE_POWER_FACTOR_THRESHOLD		100
#define INDUCTIVE_POWER_FACTOR_THRESHOLD		92

extern LoRaHandle_t loraHandle;
static TimerEvent_t timer;

static void FillSampleWithValues(Sample_t *sample);
static bool IsCompensationNeeded(Sample_t *sample);
static void Compensate(EndNode_t *node);

static void BroadcastAcquisition(void)
{
	Message_t message;
	message.command = COMMAND_ACQUISITION;
	message.paramLength = 0;
	LoRa_Broadcast(message);
	TimerStart(&timer);
}

void App_ProcessRequest(Frame_t frame)
{
	uint8_t i;
	EndNode_t *endNode;
	
	for (i = 0; i < RADIO_MAX_CONNECTIONS; i++)
		if (loraHandle.endNodes[i].address == frame.endDevice)
		{
			endNode = &loraHandle.endNodes[i];
			break;
		}
	
	for (i = 0; i < frame.nrOfMessages; i++)
	{
		switch (frame.messages[i].command)
		{
			case COMMAND_TIMESTAMP:
				endNode->sample.time.hour = frame.messages[i].params[0];
				endNode->sample.time.minute = frame.messages[i].params[1];
				endNode->sample.time.second = frame.messages[i].params[2];
				break;
			case COMMAND_ACTIVE_ENERGY:
				endNode->sample.activeEnergy = (frame.messages[i].params[0] << 16) |
					(frame.messages[i].params[1] << 8) |
					(frame.messages[i].params[2] << 0);
				break;
			case COMMAND_REACTIVE_ENERGY:
				endNode->sample.reactiveEnergy = (frame.messages[i].params[0] << 16) |
					(frame.messages[i].params[1] << 8) |
					(frame.messages[i].params[2] << 0);
				break;
			case COMMAND_ACTIVE_POWER:
				endNode->sample.activePower = (frame.messages[i].params[0] << 16) |
					(frame.messages[i].params[1] << 8) |
					(frame.messages[i].params[2] << 0);
				break;
			case COMMAND_REACTIVE_POWER:
				endNode->sample.reactivePower = (frame.messages[i].params[0] << 16) |
					(frame.messages[i].params[1] << 8) |
					(frame.messages[i].params[2] << 0);
				FillSampleWithValues(&endNode->sample);
				if (IsCompensationNeeded(&endNode->sample))
					Compensate(endNode);
				break;
		}
	}
}

static void FillSampleWithValues(Sample_t *sample)
{
	if (sample->reactiveEnergy & 0x800000)
	{
		sample->powerType = CAPACITIVE;
		sample->reactiveEnergy |= 0xFF000000;
		sample->reactivePower |= 0xFF000000;
	}
	else
		sample->powerType = INDUCTIVE;
	sample->apparentPower = sqrt(sample->activePower * sample->activePower +
		sample->reactivePower * sample->reactivePower);
	sample->powerFactor = sample->activePower * 100 / sample->apparentPower;
}

static bool IsCompensationNeeded(Sample_t *sample)
{
	if (sample->powerType == CAPACITIVE)
		return sample->powerFactor <= CAPACITIVE_POWER_FACTOR_THRESHOLD;
	else
		return sample->powerFactor <= INDUCTIVE_POWER_FACTOR_THRESHOLD;
}

static void Compensate(EndNode_t *node)
{
	Frame_t frame;
	
	frame.endDevice = node->address;
	frame.nrOfMessages = 1;
	frame.messages[0].command = COMMAND_SET_COMPENSATOR;
	frame.messages[0].paramLength = 1;
	
	if (node->sample.powerType == CAPACITIVE)
	{
		if (node->compensators[1].state == COMP_IN)
		{
			frame.messages[0].params[0] = 0x10;
			LoRa_ForwardFrame(frame);
			node->compensators[1].state = COMP_OUT;
		}
		else if (node->compensators[0].state == COMP_OUT) 
		{
			frame.messages[0].params[0] = 0x01;
			LoRa_ForwardFrame(frame);
			node->compensators[0].state = COMP_IN;
		}
	}
	else
	{
		if (node->compensators[0].state == COMP_IN)
		{
			frame.messages[0].params[0] = 0x00;
			LoRa_ForwardFrame(frame);
			node->compensators[0].state = COMP_OUT;
		}
		else if (node->compensators[1].state == COMP_OUT) 
		{
			frame.messages[0].params[0] = 0x11;
			LoRa_ForwardFrame(frame);
			node->compensators[1].state = COMP_IN;
		}
	}
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
		LoRa_MainLoop();
  }
}
