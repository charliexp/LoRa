/* Includes ------------------------------------------------------------------*/
#include "app.h"
#include "compensator.h"
#include "eeprom.h"
#include "lora.h"
#include "pc.h"
#include <string.h>

/* Private define ------------------------------------------------------------*/
#ifdef GATEWAY
#define RADIO_RX_TIMEOUT							10
#endif
#ifdef END_NODE
#define RADIO_RX_TIMEOUT							0
#endif

#define RADIO_TX_TIMEOUT							3
#define RADIO_FREQUENCY       				866500000
#define RADIO_MAX_OUTPUT_POWER				14
#define RADIO_INITIAL_OUTPUT_POWER		14
#define RADIO_MAX_BANDWIDTH						1
#define RADIO_INITIAL_BANDWIDTH				0
#define RADIO_MIN_SPREAD_FACTOR				7
#define RADIO_MAX_SPREAD_FACTOR				12
#define RADIO_INITIAL_SPREAD_FACTOR		RADIO_MIN_SPREAD_FACTOR
#define RADIO_CODING_RATE							4
#define RADIO_PREAMBLE_SIZE						8
#define RADIO_RX_SYMB_TIMEOUT					5
#ifdef GATEWAY
//TODO: commented are actual values
//#define	RADIO_INVERTED_IQ							true
//#define	RADIO_PERFORM_CRC							false
#define	RADIO_INVERTED_IQ							false
#define	RADIO_PERFORM_CRC							true
#endif
#ifdef END_NODE
#define	RADIO_INVERTED_IQ							false
#define	RADIO_PERFORM_CRC							true
#endif

#define ADDRESS_BROADCAST							0xAA

#define EEPROM_LOCATION								0x08080000

/* Private typedef -----------------------------------------------------------*/

/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
LoRaHandle_t loraHandle;

/* Private function prototypes -----------------------------------------------*/
#ifdef GATEWAY
static void LoRa_SignalTimeout(uint8_t address);
#endif
#ifdef END_NODE
static void LoRa_Write(void);
#endif
static void LoRa_OnTxDone(void);
static void LoRa_OnRxDone(uint8_t *payload, uint16_t size, int16_t rssi, int8_t snr);
static void LoRa_OnTxTimeout(void);
static void LoRa_OnRxTimeout(void);
static void LoRa_OnRxError(void);
static void LoRa_SignalError(void);
#ifdef END_NODE
static void LoRa_SignalRequest(Frame_t frame);
#endif

extern void App_ProcessRequest(Frame_t frame);

/* Functions Definition ------------------------------------------------------*/
#ifdef GATEWAY
void LoRa_Broadcast(Message_t message)
{
	Frame_t frame;
	uint8_t i;
	uint8_t array[FRAME_MAX_SIZE];
	uint8_t arrayLength;
	
	frame.endDevice = ADDRESS_BROADCAST;
	frame.nrOfMessages = 1;
	
	memcpy(&frame.messages[0],
		&message,
		MESSAGE_HEADER_SIZE + message.paramLength);
	
	for (i = 0; i < RADIO_MAX_CONNECTIONS; i++)
		if (loraHandle.endNodes[i].queueLength != 0)
			//Send this instead?
			return;
	
	Message_FrameToArray(frame, array, &arrayLength);
	PC_Write(frame);
	loraHandle.hw.radio->Send(array, arrayLength);
		
	for (i = 0; i < RADIO_MAX_CONNECTIONS; i++)
		loraHandle.endNodes[i].responsePending = true;
}

void LoRa_ForwardFrame(Frame_t frame)
{
	#ifdef GATEWAY
	switch (frame.messages[0].command)
	{
		case COMMAND_SET_COMPENSATOR:
			if (frame.messages[0].params[0] != ACK &&
				frame.messages[0].params[0] != NAK)
			loraHandle.endNodes[0].compensators[(frame.messages[0].params[0] >> 4) & 0x0F].state = (State_t) (frame.messages[0].params[0] & 0x0F);
			break;
		default:
			break;
	}
	#endif
	memcpy(&loraHandle.lastFrameSent, &frame, sizeof(Frame_t));
}
#endif

uint8_t LoRa_GetAddress(void)
{
	return loraHandle.address;
}

void LoRa_Init(void)
{
	#ifdef GATEWAY
	uint8_t i;
	
	for (i = 0; i < RADIO_MAX_CONNECTIONS; i++)
		loraHandle.endNodes[i].address = *((uint8_t *) EEPROM_LOCATION + i);
	#endif
	#ifdef END_NODE
	loraHandle.address = *((uint8_t *) EEPROM_LOCATION);
	#endif
	loraHandle.hw.frequency = RADIO_FREQUENCY;
	loraHandle.hw.radio = (Radio_s *) &Radio;
	loraHandle.hw.events.TxDone = LoRa_OnTxDone;
	loraHandle.hw.events.RxDone = LoRa_OnRxDone;
	loraHandle.hw.events.TxTimeout = LoRa_OnTxTimeout;
	loraHandle.hw.events.RxTimeout = LoRa_OnRxTimeout;
	loraHandle.hw.events.RxError = LoRa_OnRxError;
	loraHandle.hw.outputPower = RADIO_INITIAL_OUTPUT_POWER;
	loraHandle.hw.bandwidth = RADIO_INITIAL_BANDWIDTH;
	loraHandle.hw.spreadingFactor = RADIO_INITIAL_SPREAD_FACTOR;
	loraHandle.timeout = RADIO_RX_TIMEOUT;
	
	loraHandle.hw.radio->Init(&loraHandle.hw.events);
	loraHandle.hw.radio->SetChannel(loraHandle.hw.frequency);
	loraHandle.hw.radio->SetTxConfig(MODEM_LORA, loraHandle.hw.outputPower, 0, loraHandle.hw.bandwidth,
		loraHandle.hw.spreadingFactor , RADIO_CODING_RATE,
		RADIO_PREAMBLE_SIZE, true,
		RADIO_PERFORM_CRC, false, 0, RADIO_INVERTED_IQ, RADIO_TX_TIMEOUT * 1000);
	loraHandle.hw.radio->SetRxConfig(MODEM_LORA, loraHandle.hw.bandwidth, loraHandle.hw.spreadingFactor ,
		RADIO_CODING_RATE, 0, RADIO_PREAMBLE_SIZE,
		RADIO_RX_SYMB_TIMEOUT, false,
		FRAME_MAX_SIZE, RADIO_PERFORM_CRC, false, 0, RADIO_INVERTED_IQ, true);
		
		
	#ifdef GATEWAY
	loraHandle.endNodes[0].compensators[0].state = COMP_OUT;
	loraHandle.endNodes[0].compensators[0].type = COMP_INDUCTOR;
	loraHandle.endNodes[0].compensators[0].value = 60;
		
	loraHandle.endNodes[0].compensators[1].state = COMP_OUT;
	loraHandle.endNodes[0].compensators[1].type = COMP_CAPACITOR;
	loraHandle.endNodes[0].compensators[1].value = 60;
	#endif
	#ifdef END_NODE
	loraHandle.hw.radio->Rx(loraHandle.timeout * 1000);
	#endif
}

void LoRa_MainLoop(void)
{
	if (loraHandle.lastFrameReceived.nrOfMessages != 0)
	{
		LoRa_ProcessRequest(loraHandle.lastFrameReceived);
		/* Invalidate message */
		loraHandle.lastFrameReceived.nrOfMessages = 0;
	}
	if (loraHandle.lastFrameSent.nrOfMessages != 0)
	{
		uint8_t array[FRAME_MAX_SIZE];
		uint8_t arrayLength;
		
		Message_FrameToArray(loraHandle.lastFrameSent, array, &arrayLength);
		PC_Write(loraHandle.lastFrameSent);
		loraHandle.hw.radio->Send(array, arrayLength);
		/* Invalidate message */
		loraHandle.lastFrameSent.nrOfMessages = 0;
	}
}

static void LoRa_OnRxDone(uint8_t *payload, uint16_t size, int16_t rssi, int8_t snr)
{
	Frame_t frame;
	
	loraHandle.hw.radio->Sleep();
	#ifdef GATEWAY
	//Check if more need to be received
	Message_ArrayToFrame(payload, &frame);
	LoRa_ProcessRequest(frame);
	#endif
	#ifdef END_NODE
	Message_ArrayToFrame(payload, &frame);
	LoRa_SignalRequest(frame);
	#endif
	//TODO: add those
	//RSSI = rssi;
	//SNR = snr;
}

static void LoRa_OnRxError(void)
{
	#ifdef GATEWAY
	//TODO: request resend?
	loraHandle.hw.radio->Sleep();
	#endif
	#ifdef END_NODE
	loraHandle.hw.radio->Rx(loraHandle.timeout * 1000);
	#endif
}

static void LoRa_OnRxTimeout(void)
{
  loraHandle.hw.radio->Sleep();
	#ifdef GATEWAY
	//TODO
	uint8_t i;
	for (i = 0; i < RADIO_MAX_CONNECTIONS; i++)
		if (loraHandle.endNodes[i].responsePending)
		{
			loraHandle.endNodes[i].responsePending = false;
			LoRa_SignalTimeout(loraHandle.endNodes[i].address);
		}
	#endif
}

static void LoRa_OnTxDone(void)
{
	loraHandle.hw.radio->Rx(loraHandle.timeout * 1000);
}

static void LoRa_OnTxTimeout(void)
{
  loraHandle.hw.radio->Sleep();
	LoRa_SignalError();
}

void LoRa_ProcessRequest(Frame_t frame)
{
	uint8_t i;
	#ifdef END_NODE
	Message_t reply;
	#endif
	
	PC_Write(frame);
	for (i = 0; i < frame.nrOfMessages; i++)
	{
		#ifdef END_NODE
		reply.command = frame.messages[i].command;
		switch (frame.messages[i].command)
		{
		case COMMAND_ERROR:
			switch (frame.messages[i].params[0])
			{
				case ERROR_RESEND:
					memcpy(&reply, &loraHandle.lastFrameSent, FRAME_MAX_SIZE);
					break;
				case ERROR_RESET:
					//TODO
					break;
			}
			break;
		case COMMAND_SET_ADDRESS:
			loraHandle.address = frame.messages[i].params[0];
			EEPROM_WriteByte(EEPROM_LOCATION, loraHandle.address);
			reply.paramLength = 1;
			reply.params[0] = ACK;
			break;
		case COMMAND_CHANGE_COMPENSATOR:
		case COMMAND_SET_COMPENSATOR:
			Comp_ProcessRequest(frame.messages[i]);
			break;
		case COMMAND_ACQUISITION:
			Meter_ProcessRequest(frame.messages[i]);
			break;
		default:
			reply.paramLength = 1;
			reply.params[0] = NAK;
			LoRa_QueueMessage(reply);
			break;
		}
		#endif
		#ifdef GATEWAY
		App_ProcessRequest(frame);
		#endif
	}
	//memcpy(&loraHandle.lastFrameSent, &reply, FRAME_MAX_SIZE);
	
	#ifdef END_NODE
	if (loraHandle.queueLength != 0)
		LoRa_Write();
	else
		loraHandle.hw.radio->Rx(loraHandle.timeout * 1000);
	#endif
}

#ifdef END_NODE
void LoRa_QueueMessage(Message_t message)
{
	memcpy(&loraHandle.messageQueue[loraHandle.queueLength],
		&message,
		MESSAGE_HEADER_SIZE + message.paramLength);
	loraHandle.queueLength++;
}
#endif

static void LoRa_SignalError(void)
{
	Frame_t frame;
	
	frame.endDevice = loraHandle.address;
	frame.nrOfMessages = 1;
	
	frame.messages[0].command = COMMAND_ERROR;
	frame.messages[0].paramLength = 1;
	frame.messages[0].params[0] = ERROR_LORA_NOK;
	
	PC_Write(frame);
}

#ifdef END_NODE
static void LoRa_SignalRequest(Frame_t frame)
{
	memcpy(&loraHandle.lastFrameReceived, &frame, sizeof(Frame_t));
}
#endif

#ifdef GATEWAY
static void LoRa_SignalTimeout(uint8_t address)
{
	Frame_t frame;
	
	frame.endDevice = address;
	frame.nrOfMessages = 1;
	
	frame.messages[0].command = COMMAND_ERROR;
	frame.messages[0].paramLength = 1;
	frame.messages[0].params[0] = ERROR_LORA_TIMEOUT;
	
	PC_Write(frame);
}
#endif

#ifdef END_NODE
static void LoRa_Write(void)
{
	Frame_t frame;
	uint8_t i;
	uint8_t array[FRAME_MAX_SIZE];
	uint8_t arrayLength;
	
	frame.endDevice = loraHandle.address;
	frame.nrOfMessages = loraHandle.queueLength;
	
	for (i = 0; i < loraHandle.queueLength; i++)
	{
		memcpy(&frame.messages[i],
			&loraHandle.messageQueue[i],
			MESSAGE_HEADER_SIZE + loraHandle.messageQueue[i].paramLength);
	}
	
	PC_Write(frame);
	Message_FrameToArray(frame, array, &arrayLength);
	loraHandle.hw.radio->Send(array, arrayLength);
	loraHandle.queueLength = 0;
}
#endif
