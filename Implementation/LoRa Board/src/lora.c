/* Includes ------------------------------------------------------------------*/
#include "compensator.h"
#include "eeprom.h"
#include "lora.h"
#include "meter.h"
#include "pc.h"
#include "radio.h"
#include <string.h>
#include "timeserver.h"

/* Private define ------------------------------------------------------------*/
#ifdef GATEWAY
#define RADIO_RX_TIMEOUT							5
#define RADIO_MAX_CONNECTIONS					1
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
typedef struct Radio_t
{
/* HW handle */
	Radio_s radio;
/* Events handle */
	RadioEvents_t events;
/* Frequency */
	uint32_t frequency;
/* Output power */
	uint8_t outputPower;
/* Bandwidth */
	uint8_t bandwidth;
/* Spreading factor */
	uint8_t spreadingFactor;
}Radio_t;

typedef struct LoRaHandle_t
{
/* Radio handle */
	Radio_t hw;
/* Node address */
	uint8_t address;
/* Message queue */
	Message_t messageQueue[FRAME_MAX_MESSAGES];
#ifdef GATEWAY
/* Transmission timer */
	TimerEvent_t timer;
/* Connected end nodes addresses */
	uint8_t endNodeAddresses[RADIO_MAX_CONNECTIONS];
#endif
/* Number of messages queued */
	uint8_t queueLength;
/* Timeout in seconds */
	uint16_t timeout;
/* Last frame sent */
	Frame_t lastFrame;
}LoRaHandle_t;

/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
static LoRaHandle_t handle;

/* Private function prototypes -----------------------------------------------*/
static void LoRa_OnTxDone(void);
static void LoRa_OnRxDone(uint8_t *payload, uint16_t size, int16_t rssi, int8_t snr);
static void LoRa_OnTxTimeout(void);
static void LoRa_OnRxTimeout(void);
static void LoRa_OnRxError(void);
static void LoRa_SignalError(void);
static void LoRa_Write(void);
	
/* Functions Definition ------------------------------------------------------*/
uint8_t LoRa_GetAddress(void)
{
	return handle.address;
}

void LoRa_Init(void)
{
	#ifdef GATEWAY
	uint8_t i;
	
	for (i = 0; i < RADIO_MAX_CONNECTIONS; i++)
		handle.endNodeAddresses[i] = *((uint8_t *) EEPROM_LOCATION + i);
	#endif
	#ifdef END_NODE
	handle.address = *((uint8_t *) EEPROM_LOCATION);
	#endif
	handle.hw.frequency = RADIO_FREQUENCY;
	handle.hw.events.TxDone = LoRa_OnTxDone;
	handle.hw.events.RxDone = LoRa_OnRxDone;
	handle.hw.events.TxTimeout = LoRa_OnTxTimeout;
	handle.hw.events.RxTimeout = LoRa_OnRxTimeout;
	handle.hw.events.RxError = LoRa_OnRxError;
	handle.hw.outputPower = RADIO_INITIAL_OUTPUT_POWER;
	handle.hw.bandwidth = RADIO_INITIAL_BANDWIDTH;
	handle.hw.spreadingFactor = RADIO_INITIAL_SPREAD_FACTOR;
	handle.timeout = RADIO_RX_TIMEOUT;
	
	Radio.Init(&handle.hw.events);
	Radio.SetChannel(handle.hw.frequency);
	Radio.SetTxConfig(MODEM_LORA, handle.hw.outputPower, 0, handle.hw.bandwidth,
		handle.hw.spreadingFactor , RADIO_CODING_RATE,
		RADIO_PREAMBLE_SIZE, true,
		RADIO_PERFORM_CRC, false, 0, RADIO_INVERTED_IQ, RADIO_TX_TIMEOUT * 1000);
	Radio.SetRxConfig(MODEM_LORA, handle.hw.bandwidth, handle.hw.spreadingFactor ,
		RADIO_CODING_RATE, 0, RADIO_PREAMBLE_SIZE,
		RADIO_RX_SYMB_TIMEOUT, false,
		FRAME_MAX_SIZE, RADIO_PERFORM_CRC, false, 0, RADIO_INVERTED_IQ, true);
		
	#ifdef GATEWAY
	//set up tx timer
	#endif
	//TODO: start receiving
}

static void LoRa_OnRxDone(uint8_t *payload, uint16_t size, int16_t rssi, int8_t snr)
{
	Frame_t frame;
	
	Radio.Sleep();
	#ifdef GATEWAY
	//Check if more need to be received
	#endif
	#ifdef END_NODE
	Message_ArrayToFrame(payload, &frame);
	LoRa_ProcessRequest(frame);
	#endif
	//TODO: add those
	//RSSI = rssi;
	//SNR = snr;
}

static void LoRa_OnRxError(void)
{
	Radio.Sleep();
	//TODO request resend
}

static void LoRa_OnRxTimeout(void)
{
  Radio.Sleep();
	#ifdef GATEWAY
	//TODO
	#endif
}

static void LoRa_OnTxDone(void)
{
	Radio.Rx(handle.timeout * 1000);
}

static void LoRa_OnTxTimeout(void)
{
  Radio.Sleep();
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
			switch (frame.messages[i].rawArgument[0])
			{
				case ERROR_RESEND:
					memcpy(&reply, &handle.lastFrame, FRAME_MAX_SIZE);
					break;
				case ERROR_RESET:
					//TODO
					break;
			}
			break;
		case COMMAND_SET_ADDRESS:
			handle.address = frame.messages[i].rawArgument[0];
			EEPROM_WriteByte(EEPROM_LOCATION, handle.address);
			reply.argLength = 1;
			reply.rawArgument[0] = ACK;
			break;
		case COMMAND_CHANGE_COMPENSATOR:
		case COMMAND_SET_COMPENSATOR:
			Comp_ProcessRequest(frame.messages[i]);
			reply.argLength = 1;
			reply.rawArgument[0] = ACK;
			break;
		case COMMAND_ACQUISITION:
			Meter_ProcessRequest(frame.messages[i]);
			break;
		default:
			reply.argLength = 1;
			reply.rawArgument[0] = NAK;
			LoRa_QueueMessage(reply);
			break;
		}
		#endif
	}
	//memcpy(&handle.lastFrame, &reply, FRAME_MAX_SIZE);
	
	if (handle.queueLength != 0)
		LoRa_Write();
}

void LoRa_QueueMessage(Message_t message)
{
	memcpy(&handle.messageQueue[handle.queueLength],
		&message,
		MESSAGE_HEADER_SIZE + message.argLength);
	handle.queueLength++;
}

static void LoRa_SignalError(void)
{
	Frame_t frame;
	
	frame.endDevice = handle.address;
	frame.nrOfMessages = 1;
	
	frame.messages[0].command = COMMAND_ERROR;
	frame.messages[0].argLength = 1;
	frame.messages[0].rawArgument[0] = ERROR_LORA_NOK;
	
	PC_Write(frame);
}

static void LoRa_Write(void)
{
	Frame_t frame;
	uint8_t i;
	uint8_t array[FRAME_MAX_SIZE];
	uint8_t arrayLength;
	
	frame.endDevice = handle.address;
	frame.nrOfMessages = handle.queueLength;
	
	for (i = 0; i < handle.queueLength; i++)
	{
		memcpy(&frame.messages[i],
			&handle.messageQueue[i],
			MESSAGE_HEADER_SIZE + handle.messageQueue[i].argLength);
	}
	
	PC_Write(frame);
	Message_FrameToArray(frame, array, &arrayLength);
	//handle.hw.radio.Send(array, arrayLength);
	handle.queueLength = 0;
}
