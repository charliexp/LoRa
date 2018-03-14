#ifndef LORA_H
#define LORA_H

#include "commands.h"
#include "hw.h"

#define RADIO_BUFFER_SIZE     64

typedef enum RadioStates_t
{
	RADIO_LOWPOWER = 0xf0,
	RADIO_RX = 0xf1,
	RADIO_RX_TIMEOUT = 0xf2,
	RADIO_RX_ERROR = 0xf3,
	RADIO_TX = 0xf4,
	RADIO_TX_TIMEOUT = 0xf5,
}RadioStates_t;

extern RadioStates_t RadioState;

void LoRa_init(void);
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

#endif /* LORA_H */
