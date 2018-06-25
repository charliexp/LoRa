/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __LORA_H__
#define __LORA_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include "compensator.h"
#include "message.h"
#include "meter.h"
#include "radio.h"

/* Exported defines ----------------------------------------------------------*/
#ifdef GATEWAY
#define RADIO_MAX_CONNECTIONS					1
#endif

/* Exported types ------------------------------------------------------------*/
#ifdef GATEWAY
typedef struct EndNode_t
{
/* Message queue */
	Message_t messageQueue[FRAME_MAX_MESSAGES];
/* Number of messages queued */
	uint8_t queueLength;
/* Address */
	uint8_t address;
/* Compensators array */
	Compensator_t compensators[2];
/* Response pending */
	uint8_t responsePending;
/* Latest sample */
	Sample_t sample;
}EndNode_t;
#endif

typedef struct Radio_t
{
/* HW handle */
	Radio_s *radio;
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
#ifdef END_NODE
/* Message queue */
	Message_t messageQueue[FRAME_MAX_MESSAGES];
/* Number of messages queued */
	uint8_t queueLength;
#endif
#ifdef GATEWAY
/* Connected end nodes info */
	EndNode_t endNodes[RADIO_MAX_CONNECTIONS];
#endif
/* Timeout in seconds */
	uint16_t timeout;
/* Last frame sent for resend command */
	Frame_t lastFrameSent;
/* Last frame received */
	Frame_t lastFrameReceived;
}LoRaHandle_t;

/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
#ifdef GATEWAY
void LoRa_Broadcast(Message_t message);
void LoRa_ForwardFrame(Frame_t frame);
#endif
#ifdef END_NODE
void LoRa_QueueMessage(Message_t message);
#endif
uint8_t LoRa_GetAddress(void);
uint16_t LoRa_GetTransmissionRate(void);
void LoRa_Init(void);
void LoRa_MainLoop(void);
void LoRa_ProcessRequest(Frame_t frame);

#endif /* __LORA_H__ */
