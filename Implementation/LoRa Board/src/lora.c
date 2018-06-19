#include "lora.h"
#include "radio.h"
#include "stdlib.h"

#define RADIO_FREQUENCY       866500000

bool LoRaSetupRequired = false;

uint8_t LoRa_Bandwidth = 0;         // [0: 125 kHz,
																		//  1: 250 kHz,
																		//  2: 500 kHz,
																		//  3: Reserved]
uint8_t LoRa_OutputPower = 14;      // dBm
uint8_t LoRa_SpreadingFactor = 12;  // [SF7..SF12]
uint8_t LoRa_CodingRate = 4;        // [1: 4/5,
                                    //  2: 4/6,
                                    //  3: 4/7,
                                    //  4: 4/8]
uint8_t LoRa_RxSymTimeout = 5;      // Symbols
uint16_t LoRa_RxMsTimeout = 5000;   // Milliseconds
uint32_t LoRa_TxTimeout = 5000;  		// Milliseconds
uint8_t LoRa_PreambleSize = 8;      // Same for Tx and Rx
uint8_t LoRa_PayloadMaxSize = 7;
bool LoRa_VariablePayload = true;
bool LoRa_PerformCRC = true;
bool LoRa_Transmitting = false;

volatile uint8_t RadioRxBuffer[RADIO_BUFFER_SIZE];
volatile uint8_t RadioTxBuffer[RADIO_BUFFER_SIZE];
uint16_t BufferSize = RADIO_BUFFER_SIZE;

RadioNodeStruct_t RadioNodes[RADIO_MAX_NODES] = 
{
	{false, 2},
	{false, 3},
};
RadioStates_t RadioState = RADIO_LOWPOWER;

int8_t RSSI = 0;
int8_t SNR = 0;
static RadioEvents_t RadioEvents;

extern uint8_t myAddress;

static void OnTxDone( void )
{
	Radio.Sleep();
	RadioState = RADIO_TX;
}

static void OnRxDone( uint8_t *payload, uint16_t size, int16_t rssi, int8_t snr )
{
	Radio.Sleep();
	RadioState = RADIO_RX;
	BufferSize = size;
	memcpy((uint8_t *) RadioRxBuffer, payload, BufferSize );
	RSSI = rssi;
	SNR = snr;
}

static void OnTxTimeout( void )
{
  Radio.Sleep();
	RadioState = RADIO_TX_TIMEOUT;
}

static void OnRxTimeout( void )
{
  Radio.Sleep();
	RadioState = RADIO_RX_TIMEOUT;
}

static void OnRxError( void )
{
	Radio.Sleep();
	RadioState = RADIO_RX_ERROR;
}

void expectingResponseFrom(uint8_t target)
{
	uint8_t i;
	
	if (target == ADDRESS_GENERAL)
		for (i = 0; i < RADIO_MAX_NODES; i++)
			RadioNodes[i].responsePending = true;
	else
		RadioNodes[target - ADDRESS_BEACON].responsePending = true;
}

bool notAllNodesResponded(uint8_t lastResponseSource)
{
	uint8_t i;
	for (i = 0; i < RADIO_MAX_NODES; i++)
		if (RadioNodes[i].address > lastResponseSource && RadioNodes[i].responsePending)
			return true;
		
	return false;
}

void LoRa_init(void)
{
  RadioEvents.TxDone = OnTxDone;
  RadioEvents.RxDone = OnRxDone;
  RadioEvents.TxTimeout = OnTxTimeout;
  RadioEvents.RxTimeout = OnRxTimeout;
  RadioEvents.RxError = OnRxError;

  Radio.Init( &RadioEvents );

  Radio.SetChannel( RADIO_FREQUENCY );

  Radio.SetTxConfig( MODEM_LORA, LoRa_OutputPower, 0, LoRa_Bandwidth,
                                 LoRa_SpreadingFactor, LoRa_CodingRate,
																 LoRa_PreambleSize, !LoRa_VariablePayload,
																 LoRa_PerformCRC, 0, 0, false, LoRa_TxTimeout );
    
  Radio.SetRxConfig( MODEM_LORA, LoRa_Bandwidth, LoRa_SpreadingFactor,
																 LoRa_CodingRate, 0, LoRa_PreambleSize,
																 LoRa_RxSymTimeout, !LoRa_VariablePayload,
																 LoRa_PayloadMaxSize, LoRa_PerformCRC, 0, 0, false, true );
}

void LoRa_send(uint8_t target, uint8_t command, uint8_t* data, uint8_t length)
{
	uint8_t i;
	RadioTxBuffer[IDX_SOURCE_ADDRESS] = myAddress;
	RadioTxBuffer[IDX_TARGET_ADDRESS] = target;
	RadioTxBuffer[IDX_COMMAND] = command;
	for (i = 0; i < length; i++)
		RadioTxBuffer[IDX_COMMAND + 1 + i] = data[i];
	Radio.Send((uint8_t *) RadioTxBuffer, LoRa_PayloadMaxSize );
	RadioState = RADIO_LOWPOWER;
	if (target != ADDRESS_MASTER)
		expectingResponseFrom(target);
}

void LoRa_startReceiving(void)
{
	Radio.Rx(LoRa_RxMsTimeout);
	RadioState = RADIO_LOWPOWER;
}

void LoRa_receive(uint8_t* source, uint8_t* command, uint8_t* parameters, uint8_t* rssi, uint8_t* snr)
{
	uint8_t i;
	*source = RadioRxBuffer[IDX_SOURCE_ADDRESS];
	*command = RadioRxBuffer[IDX_COMMAND];
	for (i = 0; i < PARAMETERS_MAX_SIZE; i++)
		parameters[i] = RadioRxBuffer[IDX_COMMAND + 1 + i];
	RadioState = RADIO_LOWPOWER;
	*rssi = abs(RSSI);
	*snr = SNR;
	if (*source != ADDRESS_MASTER)
	{
		for (i = 0; i < RADIO_MAX_NODES; i++)
			if (RadioNodes[i].address == *source)
			{
				RadioNodes[i].responsePending = false;
				break;
			}
		if (notAllNodesResponded(*source))
			LoRa_startReceiving();
	}
}

uint8_t LoRa_whoTimedOut(void)
{
	uint8_t target = RadioTxBuffer[IDX_TARGET_ADDRESS];
	uint8_t i;
	
	for (i = 0; i < RADIO_MAX_NODES; i++)
		if (RadioNodes[i].responsePending)
		{
			RadioNodes[i].responsePending = false;
			target = RadioNodes[i].address;
			break;
		}
	RadioState = RADIO_LOWPOWER;
	return target;
}

void LoRa_updateParameters(void)
{
	Radio.SetTxConfig( MODEM_LORA, LoRa_OutputPower, 0, LoRa_Bandwidth,
																 LoRa_SpreadingFactor, LoRa_CodingRate,
																 LoRa_PreambleSize, !LoRa_VariablePayload,
																 LoRa_PerformCRC, 0, 0, false, LoRa_TxTimeout );
		
	Radio.SetRxConfig( MODEM_LORA, LoRa_Bandwidth, LoRa_SpreadingFactor,
																 LoRa_CodingRate, 0, LoRa_PreambleSize,
																 LoRa_RxSymTimeout, !LoRa_VariablePayload,
																 LoRa_PayloadMaxSize, LoRa_PerformCRC, 0, 0, false, true );
}

void LoRa_setBandwidth(uint8_t bandwidth)
{
	LoRa_Bandwidth = bandwidth;
}

void LoRa_setOutputPower(uint8_t outputPower)
{
	LoRa_OutputPower = outputPower;
}

void LoRa_setCodingRate(uint8_t codingRate)
{
	LoRa_CodingRate = codingRate;
}

void LoRa_setSpreadingFactor(uint8_t spreadingFactor)
{
	LoRa_SpreadingFactor = spreadingFactor;
}

void LoRa_setRxSymTimeout(uint8_t rxSymTimeout)
{
	LoRa_RxSymTimeout = rxSymTimeout;
}

void LoRa_setRxMsTimeout(uint32_t rxMsTimeout)
{
	LoRa_RxMsTimeout = rxMsTimeout;
}

void LoRa_setTxTimeout(uint32_t txTimeout)
{
	LoRa_TxTimeout = txTimeout;
}

void LoRa_setPreambleSize(uint8_t preambleSize)
{
	LoRa_PreambleSize = preambleSize;
}

void LoRa_setPayloadMaxSize(uint8_t payloadMaxSize)
{
	LoRa_PayloadMaxSize = payloadMaxSize;
}

void LoRa_setVariablePayload(bool variablePayload)
{
	LoRa_VariablePayload = variablePayload;
}

void LoRa_setPerformCRC(bool performCRC)
{
	LoRa_PerformCRC = performCRC;
}
