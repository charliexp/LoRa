#ifndef LORA_H
#define LORA_H

#include "commands.h"
#include "hw.h"

#define RADIO_BUFFER_SIZE     64

typedef enum RadioStates_t
{
	RADIO_LOWPOWER,
	RADIO_RX,
	RADIO_RX_TIMEOUT,
	RADIO_RX_ERROR,
	RADIO_TX,
	RADIO_TX_TIMEOUT
}RadioStates_t;

extern RadioStates_t RadioState;

void LoRa_init(void);
void LoRa_send(uint8_t target, uint8_t command, uint8_t* data, uint8_t length);
void LoRa_startReceiving(void);
void LoRa_receive(uint8_t* source, uint8_t* command, uint8_t* parameters);
uint8_t LoRa_whoTimedOut(void);
int8_t LoRa_getRSSI(void);
int8_t LoRa_getSNR(void);

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

#endif /* LORA_H */
