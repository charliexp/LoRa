/* Includes ------------------------------------------------------------------*/
#include <string.h>
#include "hw.h"
#include "radio.h"
#include "timeServer.h"
#include "delay.h"
#include "low_power.h"
#include "vcom.h"

#define LED_PERIOD_MS               200

#define LEDS_OFF   do{ \
                   LED_Off( LED_BLUE ) ;   \
                   LED_Off( LED_RED ) ;    \
                   LED_Off( LED_GREEN1 ) ; \
                   LED_Off( LED_GREEN2 ) ; \
                   } while(0) ;

#define RF_FREQUENCY          866500000 // Hz
#define RADIO_BUFFER_SIZE     64
#define UART_BUFFER_SIZE			7
#define DEVICE_ADDRESS				1
#define BEACON_ADDRESS				2
									 
//Indexes in Radio buffer
#define SOURCE_ADDRESS				0
#define TARGET_ADDRESS				1
#define COMMAND_CODE					2
#define COMMAND_PARAMETER			6

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

typedef enum
{
	RADIO_LOWPOWER,
	RADIO_RX,
	RADIO_RX_TIMEOUT,
	RADIO_RX_ERROR,
	RADIO_TX,
	RADIO_TX_TIMEOUT
}RadioStates_t;

typedef enum
{
    UART_IDLE,
		UART_RX
}UartStates_t;

typedef enum
{
	COMMAND_BANDWIDTH = 'a',
	COMMAND_OUTPUT_POWER = 'b',
	COMMAND_SPREAD_FACTOR = 'c',
	COMMAND_CODING_RATE = 'd',
	COMMAND_RX_SYM_TIMEOUT = 'e',
	COMMAND_RX_MS_TIMEOUT = 'f',
	COMMAND_TX_TIMEOUT = 'g',
	COMMAND_PREAMBLE_SIZE = 'h',
	COMMAND_PAYLOAD_MAX_SIZE = 'i',
	COMMAND_VARIABLE_PAYLOAD = 'j',
	COMMAND_PERFORM_CRC = 'k',
	COMMAND_IS_MASTER = 'y',
	COMMAND_IS_PRESENT = 'z'
}Commands_t;

typedef enum
{
	RESPONSE_ACK = 1,
	RESPONSE_NACK = 0xff
}Responses_t;

extern UART_HandleTypeDef UartHandle;

uint16_t BufferSize = RADIO_BUFFER_SIZE;
volatile uint8_t RadioRxBuffer[RADIO_BUFFER_SIZE];
volatile uint8_t RadioTxBuffer[RADIO_BUFFER_SIZE];
volatile uint8_t UartRxBuffer[UART_BUFFER_SIZE];
volatile uint8_t UartTxBuffer[UART_BUFFER_SIZE];
uint8_t RadioSetupRequired = 0;
bool BeaconCommandPending = false;

UartStates_t UartState = UART_IDLE;
RadioStates_t RadioState = RADIO_LOWPOWER;

int8_t RssiValue = 0;
int8_t SnrValue = 0;

 /* Led Timers objects*/
static  TimerEvent_t timerLed;

/* Private function prototypes -----------------------------------------------*/
/*!
 * Radio events function pointer
 */
static RadioEvents_t RadioEvents;

/*!
 * \brief Function to be executed on Radio Tx Done event
 */
void OnTxDone( void );

/*!
 * \brief Function to be executed on Radio Rx Done event
 */
void OnRxDone( uint8_t *payload, uint16_t size, int16_t rssi, int8_t snr );

/*!
 * \brief Function executed on Radio Tx Timeout event
 */
void OnTxTimeout( void );

/*!
 * \brief Function executed on Radio Rx Timeout event
 */
void OnRxTimeout( void );

/*!
 * \brief Function executed on Radio Rx Error event
 */
void OnRxError( void );

/*!
 * \brief Function executed on when led timer elapses
 */
static void OnledEvent( void );

/**
 * Main application entry point.
 */
int main( void )
{
  HAL_Init( );
  
  SystemClock_Config( );
  
  DBG_Init( );

  HW_Init( );  
  
  /* Led Timers*/
  TimerInit(&timerLed, OnledEvent);   
  TimerSetValue( &timerLed, LED_PERIOD_MS);

  TimerStart(&timerLed );

  // Radio initialization
  RadioEvents.TxDone = OnTxDone;
  RadioEvents.RxDone = OnRxDone;
  RadioEvents.TxTimeout = OnTxTimeout;
  RadioEvents.RxTimeout = OnRxTimeout;
  RadioEvents.RxError = OnRxError;

  Radio.Init( &RadioEvents );

  Radio.SetChannel( RF_FREQUENCY );

  Radio.SetTxConfig( MODEM_LORA, LoRa_OutputPower, 0, LoRa_Bandwidth,
                                 LoRa_SpreadingFactor, LoRa_CodingRate,
																 LoRa_PreambleSize, !LoRa_VariablePayload,
																 LoRa_PerformCRC, 0, 0, false, LoRa_TxTimeout );
    
  Radio.SetRxConfig( MODEM_LORA, LoRa_Bandwidth, LoRa_SpreadingFactor,
																 LoRa_CodingRate, 0, LoRa_PreambleSize,
																 LoRa_RxSymTimeout, !LoRa_VariablePayload,
																 LoRa_PayloadMaxSize, LoRa_PerformCRC, 0, 0, false, true );

	RadioTxBuffer[SOURCE_ADDRESS] = DEVICE_ADDRESS;
	RadioTxBuffer[TARGET_ADDRESS] = BEACON_ADDRESS;
	RadioTxBuffer[COMMAND_CODE] = COMMAND_IS_PRESENT;
	Radio.Send( (uint8_t *) RadioTxBuffer, LoRa_PayloadMaxSize );
	
	HAL_UART_Receive_IT(&UartHandle, (uint8_t *) UartRxBuffer, UART_BUFFER_SIZE);
                                  
  while( 1 )
  {
		switch (UartState)
		{
			case UART_RX:
				if (UartRxBuffer[TARGET_ADDRESS] == DEVICE_ADDRESS)
				{
					switch (UartRxBuffer[COMMAND_CODE])
					{
						case COMMAND_BANDWIDTH:
							PRINTF("Bandwidth: %u\n\r", UartRxBuffer[COMMAND_PARAMETER]);
							LoRa_Bandwidth = UartRxBuffer[COMMAND_PARAMETER];
							RadioSetupRequired = 1;
							break;
						case COMMAND_OUTPUT_POWER:
							PRINTF("Output Power: %u\n\r", UartRxBuffer[COMMAND_PARAMETER]);
							LoRa_OutputPower = UartRxBuffer[COMMAND_PARAMETER];
							RadioSetupRequired = 1;
							break;
						case COMMAND_CODING_RATE:
							PRINTF("Coding Rate: %u\n\r", UartRxBuffer[COMMAND_PARAMETER]);
							LoRa_CodingRate = UartRxBuffer[COMMAND_PARAMETER];
							RadioSetupRequired = 1;
							break;
						case COMMAND_SPREAD_FACTOR:
							PRINTF("Spreading Factor: %u\n\r", UartRxBuffer[COMMAND_PARAMETER]);
							LoRa_SpreadingFactor = UartRxBuffer[COMMAND_PARAMETER];
							RadioSetupRequired = 1;
							break;
						case COMMAND_RX_SYM_TIMEOUT:
							PRINTF("Rx Timeout (sym): %u\n\r", UartRxBuffer[COMMAND_PARAMETER]);
							LoRa_RxSymTimeout = UartRxBuffer[COMMAND_PARAMETER];
							RadioSetupRequired = 1;
							break;
						case COMMAND_RX_MS_TIMEOUT:
							PRINTF("Rx Timeout (ms): %u\n\r", (uint32_t) UartRxBuffer[COMMAND_PARAMETER - 3] << 24 |
																								(uint32_t) UartRxBuffer[COMMAND_PARAMETER - 2] << 16 |
																								(uint32_t) UartRxBuffer[COMMAND_PARAMETER - 1] << 8 |
																													 UartRxBuffer[COMMAND_PARAMETER]);
							LoRa_RxMsTimeout = (uint32_t) UartRxBuffer[COMMAND_PARAMETER - 3] << 24 |
															 (uint32_t) UartRxBuffer[COMMAND_PARAMETER - 2] << 16 |
															 (uint32_t) UartRxBuffer[COMMAND_PARAMETER - 1] << 8 |
																					UartRxBuffer[COMMAND_PARAMETER];
							RadioSetupRequired = 1;
							break;
						case COMMAND_TX_TIMEOUT:
							PRINTF("Tx Timeout (ms): %u\n\r", (uint32_t) UartRxBuffer[COMMAND_PARAMETER - 3] << 24 |
																								(uint32_t) UartRxBuffer[COMMAND_PARAMETER - 2] << 16 |
																								(uint32_t) UartRxBuffer[COMMAND_PARAMETER - 1] << 8 |
																													 UartRxBuffer[COMMAND_PARAMETER]);
							LoRa_TxTimeout = (uint32_t) UartRxBuffer[COMMAND_PARAMETER - 3] << 24 |
															 (uint32_t) UartRxBuffer[COMMAND_PARAMETER - 2] << 16 |
															 (uint32_t) UartRxBuffer[COMMAND_PARAMETER - 1] << 8 |
																					UartRxBuffer[COMMAND_PARAMETER];
							RadioSetupRequired = 1;
							break;
						case COMMAND_PREAMBLE_SIZE:
							PRINTF("Preamble Size: %u\n\r", UartRxBuffer[COMMAND_PARAMETER]);
							LoRa_PreambleSize = UartRxBuffer[COMMAND_PARAMETER];
							RadioSetupRequired = 1;
							break;
						case COMMAND_PAYLOAD_MAX_SIZE:
							PRINTF("Payload Max Size: %u\n\r", UartRxBuffer[COMMAND_PARAMETER]);
							LoRa_PayloadMaxSize = UartRxBuffer[COMMAND_PARAMETER];
							RadioSetupRequired = 1;
							break;
						case COMMAND_VARIABLE_PAYLOAD:
							PRINTF("Variable Payload: %u\n\r", UartRxBuffer[COMMAND_PARAMETER]);
							LoRa_VariablePayload = UartRxBuffer[COMMAND_PARAMETER];
							RadioSetupRequired = 1;
							break;
						case COMMAND_PERFORM_CRC:
							PRINTF("Perform CRC: %u\n\r", UartRxBuffer[COMMAND_PARAMETER]);
							LoRa_PerformCRC = UartRxBuffer[COMMAND_PARAMETER];
							RadioSetupRequired = 1;
							break;
						default:
							PRINTF("Received unknown command: %c\n\r", UartRxBuffer[COMMAND_CODE]);
							break;
					}
				}
				else
				{
					if (UartRxBuffer[TARGET_ADDRESS] == 0 && UartRxBuffer[COMMAND_CODE] == COMMAND_IS_MASTER)
							PRINTF("I am a master\n\r");
					else
					{
						PRINTF("Forwarding command %c to: %d\n\r", UartRxBuffer[COMMAND_CODE], UartRxBuffer[TARGET_ADDRESS]);
						memcpy((uint8_t *) RadioTxBuffer, (uint8_t *) UartRxBuffer, LoRa_PayloadMaxSize);
						BeaconCommandPending = true;
					}
				}
				UartState = UART_IDLE;
				break;
			case UART_IDLE:
			default:
				break;
		}
		
		if (RadioSetupRequired && RadioState != RADIO_LOWPOWER)
		{
			Radio.SetTxConfig( MODEM_LORA, LoRa_OutputPower, 0, LoRa_Bandwidth,
												 LoRa_SpreadingFactor, LoRa_CodingRate,
												 LoRa_PreambleSize, !LoRa_VariablePayload,
												 LoRa_PerformCRC, 0, 0, false, LoRa_TxTimeout );
				
			Radio.SetRxConfig( MODEM_LORA, LoRa_Bandwidth, LoRa_SpreadingFactor,
												 LoRa_CodingRate, 0, LoRa_PreambleSize,
												 LoRa_RxSymTimeout, !LoRa_VariablePayload,
												 LoRa_PayloadMaxSize, LoRa_PerformCRC, 0, 0, false, true );
			RadioSetupRequired = 0;
		}
		
    switch( RadioState )
    {
			case RADIO_RX:
				if (RadioRxBuffer[COMMAND_PARAMETER] == RESPONSE_ACK)
					PRINTF("Beacon %u ACK\n\r", RadioRxBuffer[SOURCE_ADDRESS]);
				else if (RadioRxBuffer[COMMAND_PARAMETER] == RESPONSE_NACK)
					PRINTF("Beacon %u NACK\n\r", RadioRxBuffer[SOURCE_ADDRESS]);
				
				if (BeaconCommandPending)
				{
					BeaconCommandPending = false;
				}
				else
				{
					RadioTxBuffer[TARGET_ADDRESS] = BEACON_ADDRESS;
					RadioTxBuffer[COMMAND_CODE] = COMMAND_IS_PRESENT;
				}
				Radio.Send( (uint8_t *) RadioTxBuffer, LoRa_PayloadMaxSize );
				
				RadioState = RADIO_LOWPOWER;
				break;
			case RADIO_RX_TIMEOUT:
				PRINTF("Beacon not responding\n\r");
				RadioTxBuffer[TARGET_ADDRESS] = BEACON_ADDRESS;
				RadioTxBuffer[COMMAND_CODE] = COMMAND_IS_PRESENT;
				Radio.Send( (uint8_t *) RadioTxBuffer, LoRa_PayloadMaxSize );
				RadioState = RADIO_LOWPOWER;
				break;
			case RADIO_RX_ERROR:
				Radio.Send( (uint8_t *) RadioTxBuffer, LoRa_PayloadMaxSize );
				RadioState = RADIO_LOWPOWER;
				break;
			case RADIO_TX:
				Radio.Rx( LoRa_RxMsTimeout );
				RadioState = RADIO_LOWPOWER;
				break;
			case RADIO_TX_TIMEOUT:
				Radio.Send( (uint8_t *) RadioTxBuffer, LoRa_PayloadMaxSize );
				RadioState = RADIO_LOWPOWER;
			case RADIO_LOWPOWER:
			default:
							// Set low power
				break;
    }
  }
}

void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart)
{
	UartState = UART_RX;
	HAL_UART_Receive_IT(&UartHandle, (uint8_t *) UartRxBuffer, UART_BUFFER_SIZE);
}

void OnTxDone( void )
{
	Radio.Sleep( );
	RadioState = RADIO_TX;
}

void OnRxDone( uint8_t *payload, uint16_t size, int16_t rssi, int8_t snr )
{
	Radio.Sleep( );
	RadioState = RADIO_RX;
	BufferSize = size;
	memcpy((uint8_t *) RadioRxBuffer, payload, BufferSize );
	RssiValue = rssi;
	SnrValue = snr;
  
	PRINTF("RssiValue=%d dBm, SnrValue=%d\n\r", rssi, snr);
}

void OnTxTimeout( void )
{
  Radio.Sleep( );
	RadioState = RADIO_TX_TIMEOUT;
}

void OnRxTimeout( void )
{
  Radio.Sleep( );
	RadioState = RADIO_RX_TIMEOUT;
}

void OnRxError( void )
{
	Radio.Sleep( );
	RadioState = RADIO_RX_ERROR;
}

static void OnledEvent( void )
{
  LED_Toggle( LED_BLUE ) ; 
  LED_Toggle( LED_RED1 ) ; 
  LED_Toggle( LED_RED2 ) ; 
  LED_Toggle( LED_GREEN ) ;   

  TimerStart(&timerLed );
}

