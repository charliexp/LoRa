/*
 / _____)             _              | |
( (____  _____ ____ _| |_ _____  ____| |__
 \____ \| ___ |    (_   _) ___ |/ ___)  _ \
 _____) ) ____| | | || |_| ____( (___| | | |
(______/|_____)_|_|_| \__)_____)\____)_| |_|
    (C)2013 Semtech

Description: Ping-Pong implementation

License: Revised BSD License, see LICENSE.TXT file include in the project

Maintainer: Miguel Luis and Gregory Cristian
*/
/******************************************************************************
  * @file    main.c
  * @author  MCD Application Team
  * @version V1.1.2
  * @date    08-September-2017
  * @brief   this is the main!
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; Copyright (c) 2017 STMicroelectronics International N.V. 
  * All rights reserved.</center></h2>
  *
  * Redistribution and use in source and binary forms, with or without 
  * modification, are permitted, provided that the following conditions are met:
  *
  * 1. Redistribution of source code must retain the above copyright notice, 
  *    this list of conditions and the following disclaimer.
  * 2. Redistributions in binary form must reproduce the above copyright notice,
  *    this list of conditions and the following disclaimer in the documentation
  *    and/or other materials provided with the distribution.
  * 3. Neither the name of STMicroelectronics nor the names of other 
  *    contributors to this software may be used to endorse or promote products 
  *    derived from this software without specific written permission.
  * 4. This software, including modifications and/or derivative works of this 
  *    software, must execute solely and exclusively on microcontroller or
  *    microprocessor devices manufactured by or for STMicroelectronics.
  * 5. Redistribution and use of this software other than as permitted under 
  *    this license is void and will automatically terminate your rights under 
  *    this license. 
  *
  * THIS SOFTWARE IS PROVIDED BY STMICROELECTRONICS AND CONTRIBUTORS "AS IS" 
  * AND ANY EXPRESS, IMPLIED OR STATUTORY WARRANTIES, INCLUDING, BUT NOT 
  * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
  * PARTICULAR PURPOSE AND NON-INFRINGEMENT OF THIRD PARTY INTELLECTUAL PROPERTY
  * RIGHTS ARE DISCLAIMED TO THE FULLEST EXTENT PERMITTED BY LAW. IN NO EVENT 
  * SHALL STMICROELECTRONICS OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
  * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
  * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
  * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
  * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
  * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
  * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  *
  ******************************************************************************
  */

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
#define UART_BUFFER_SIZE			5
#define DEVICE_ADDRESS				2
#define MASTER_ADDRESS				1

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
uint16_t LoRa_RxMsTimeout = 2000;   // Milliseconds
uint32_t LoRa_TxTimeout = 3000000;  // Milliseconds
uint8_t LoRa_PreambleSize = 8;      // Same for Tx and Rx
uint8_t LoRa_PayloadMaxSize = 6;
bool LoRa_VariablePayload = true;
bool LoRa_PerformCRC = true;

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
	
	COMMAND_IS_PRESENT = 'z'
}Commands_t;

typedef enum
{
	RESPONSE_ACK = 1,
	RESPONSE_NACK = 0xff
}Responses_t;

extern UART_HandleTypeDef UartHandle;

uint16_t BufferSize = RADIO_BUFFER_SIZE;
uint8_t RadioBuffer[RADIO_BUFFER_SIZE];
uint8_t UartBuffer[UART_BUFFER_SIZE];
uint8_t RadioSetupRequired = 0;

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
                            
  Radio.Rx( LoRa_RxMsTimeout );
	HAL_UART_Receive_IT(&UartHandle, UartBuffer, UART_BUFFER_SIZE);
                                  
  while( 1 )
  {
		switch (UartState)
		{
			case UART_RX:
				switch (UartBuffer[0])
				{
					case COMMAND_BANDWIDTH:
						PRINTF("Bandwidth: %u\n\r", UartBuffer[1]);
						LoRa_Bandwidth = UartBuffer[1];
						RadioSetupRequired = 1;
						break;
					case COMMAND_CODING_RATE:
						PRINTF("Coding Rate: %u\n\r", UartBuffer[1]);
						LoRa_CodingRate = UartBuffer[1];
						RadioSetupRequired = 1;
						break;
					case COMMAND_OUTPUT_POWER:
						PRINTF("Output Power: %u\n\r", UartBuffer[1]);
						LoRa_OutputPower = UartBuffer[1];
						RadioSetupRequired = 1;
						break;
					case COMMAND_SPREAD_FACTOR:
						PRINTF("Spreading Factor: %u\n\r", UartBuffer[1]);
						LoRa_SpreadingFactor = UartBuffer[1];
						RadioSetupRequired = 1;
						break;
					case COMMAND_RX_SYM_TIMEOUT:
						PRINTF("Rx Timeout (sym): %u\n\r", UartBuffer[1]);
						LoRa_RxSymTimeout = UartBuffer[1];
						RadioSetupRequired = 1;
						break;
					case COMMAND_RX_MS_TIMEOUT:
						PRINTF("Rx Timeout (ms): %u\n\r", (uint16_t) UartBuffer[3] << 8 |
																												 UartBuffer[4]);
						LoRa_RxMsTimeout = (uint16_t) UartBuffer[3] << 8 |
																					UartBuffer[4];
						break;
					case COMMAND_TX_TIMEOUT:
						PRINTF("Tx Timeout (ms): %u\n\r", (uint32_t) UartBuffer[1] << 24 |
																							(uint32_t) UartBuffer[2] << 16 |
																							(uint32_t) UartBuffer[3] << 8 |
																												 UartBuffer[4]);
						LoRa_TxTimeout = (uint32_t) UartBuffer[1] << 24 |
														 (uint32_t) UartBuffer[2] << 16 |
														 (uint32_t) UartBuffer[3] << 8 |
																				UartBuffer[4];
						RadioSetupRequired = 1;
						break;
					case COMMAND_PREAMBLE_SIZE:
						PRINTF("Preamble Size: %u\n\r", UartBuffer[1]);
						LoRa_PreambleSize = UartBuffer[1];
						RadioSetupRequired = 1;
						break;
					case COMMAND_PAYLOAD_MAX_SIZE:
						PRINTF("Payload Max Size: %u\n\r", UartBuffer[1]);
						LoRa_PayloadMaxSize = UartBuffer[1];
						RadioSetupRequired = 1;
						break;
					case COMMAND_VARIABLE_PAYLOAD:
						PRINTF("Variable Payload: %u\n\r", UartBuffer[1]);
						LoRa_VariablePayload = UartBuffer[1];
						RadioSetupRequired = 1;
						break;
					case COMMAND_PERFORM_CRC:
						PRINTF("Perform CRC: %u\n\r", UartBuffer[1]);
						LoRa_PerformCRC = UartBuffer[1];
						RadioSetupRequired = 1;
						break;
					default:
						break;
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
				if (RadioBuffer[0] == DEVICE_ADDRESS)
				{
					switch (RadioBuffer[1])
					{
						case COMMAND_IS_PRESENT:
							PRINTF("Asked if present\n\r");
							RadioBuffer[0] = MASTER_ADDRESS;
							RadioBuffer[1] = RESPONSE_ACK;
							Radio.Send( RadioBuffer, LoRa_PayloadMaxSize );
							break;
						case COMMAND_BANDWIDTH:
							PRINTF("Bandwidth: %u\n\r", RadioBuffer[2]);
							LoRa_Bandwidth = RadioBuffer[2];
							RadioSetupRequired = 1;
							RadioBuffer[0] = MASTER_ADDRESS;
							RadioBuffer[1] = RESPONSE_ACK;
							Radio.Send( RadioBuffer, LoRa_PayloadMaxSize );
							break;
						default:
							break;
					}
				}
				else
					Radio.Rx( LoRa_RxMsTimeout );
				RadioState = RADIO_LOWPOWER;
				break;
			case RADIO_RX_TIMEOUT:
			case RADIO_RX_ERROR:
				Radio.Rx( LoRa_RxMsTimeout );
				RadioState = RADIO_LOWPOWER;
				break;
			case RADIO_TX:
				Radio.Rx( LoRa_RxMsTimeout );
				RadioState = RADIO_LOWPOWER;
				break;
			case RADIO_TX_TIMEOUT:
				Radio.Rx( LoRa_RxMsTimeout );
				RadioState = RADIO_LOWPOWER;
				break;
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
	HAL_UART_Receive_IT(&UartHandle, UartBuffer, UART_BUFFER_SIZE);
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
	memcpy( RadioBuffer, payload, BufferSize );
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

