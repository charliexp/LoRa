/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __LORA_H__
#define __LORA_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include "message.h"

/* Exported defines ----------------------------------------------------------*/
/* Exported types ------------------------------------------------------------*/
/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
uint8_t LoRa_GetAddress(void);
uint16_t LoRa_GetTransmissionRate(void);
void LoRa_Init(void);
void LoRa_ProcessRequest(Frame_t frame);
void LoRa_QueueMessage(Message_t message);




/*
#define RADIO_BUFFER_SIZE     64
#define RADIO_MAX_NODES				2

typedef enum RadioStates_t
{
	RADIO_LOWPOWER = 0xf0,
	RADIO_RX = 0xf1,
	RADIO_RX_TIMEOUT = 0xf2,
	RADIO_RX_ERROR = 0xf3,
	RADIO_TX = 0xf4,
	RADIO_TX_TIMEOUT = 0xf5,
}RadioStates_t;

typedef struct RadioNodeStruct_t
{
	bool responsePending;
	uint8_t address;
}RadioNodeStruct_t;

extern RadioNodeStruct_t RadioNodes[];
extern RadioStates_t RadioState;

void LoRa_ReplyMessage(Message_t message);

void LoRa_MainLoop(void);
void LoRa_send(uint8_t target, uint8_t command, uint8_t* data, uint8_t length);
void LoRa_startReceiving(void);
void LoRa_receive(uint8_t* source, uint8_t* command, uint8_t* parameters, uint8_t* rssi, uint8_t* snr);
uint8_t LoRa_whoTimedOut(void);
void LoRa_updateParameters(void);

void LoRa_setBandwidth(uint8_t bandwidth);
void LoRa_setOutputPower(uint8_t outputPower);
void LoRa_setCodingRate(uint8_t codingRate);
void LoRa_setSpreadingFactor(uint8_t spreadingFactor);
void LoRa_setRxSymTimeout(uint8_t rxSymTimeout);
void LoRa_setRxMsTimeout(uint32_t rxMsTimeout);
void LoRa_setTxTimeout(uint32_t txTimeout);
void LoRa_setPreambleSize(uint8_t preambleSize);
void LoRa_setPayloadMaxSize(uint8_t payloadMaxSize);
void LoRa_setVariablePayload(bool variablePayload);
void LoRa_setPerformCRC(bool performCRC);
*/
#endif /* __LORA_H__ */
