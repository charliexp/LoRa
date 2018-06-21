#include <string.h>
#include <stdlib.h>
#include "actuator.h"
#include "app_conf.h"
#include "daq.h"
#include "message.h"
#include "lora.h"
#include "pc.h"
#include "timeServer.h"

#define DEVICE_ADDRESS_LOCATION		0x08080000

uint8_t myAddress;
bool LoRa_setupPending;
TimerEvent_t appTimer;
uint8_t step = 0;

void setAddress(uint8_t newAddress)
{
	if (newAddress != myAddress)
	{
		HAL_FLASHEx_DATAEEPROM_Unlock();
		HAL_FLASHEx_DATAEEPROM_Erase(DEVICE_ADDRESS_LOCATION);
		HAL_FLASHEx_DATAEEPROM_Program(FLASH_TYPEPROGRAMDATA_BYTE, DEVICE_ADDRESS_LOCATION, newAddress);
		HAL_FLASHEx_DATAEEPROM_Lock();
		myAddress = newAddress;
	}
}

void processCommunicationCommand(uint8_t source, uint8_t command, uint8_t* parameters, uint8_t rssi, uint8_t snr)
{/*
	uint8_t i;
	uint8_t response[PARAMETERS_MAX_SIZE + 2];
	switch (command)
	{
		case COMMAND_IS_PRESENT:
			if (myAddress == ADDRESS_MASTER)
			{
				if (source == ADDRESS_PC)
					LoRa_send(ADDRESS_GENERAL, COMMAND_IS_PRESENT, 0, 0);
				else if (source >= ADDRESS_BEACON)
				{
					response[0] = parameters[3];
					response[4] = rssi;
					response[5] = snr;
					//PC_Send(source, myAddress, command, response, 6);
				}
			}
			else if (myAddress >= ADDRESS_BEACON)
			{
				if (source == ADDRESS_MASTER)
				{
						DelayMs((myAddress - 2) * 2000);
						parameters[3] = RESPONSE_ACK;
						response[0] = RESPONSE_ACK;
						response[4] = rssi;
						response[5] = snr;
						//PC_Send(source, myAddress, command, response, 6);
						LoRa_send(source, command, parameters, PARAMETERS_MAX_SIZE);
						break;
				}
			}
			break;
		default:
			response[0] = RESPONSE_NACK;
			if (source >= ADDRESS_BEACON)
			{
				for (i = 0; i < RADIO_MAX_NODES; i++)
					if (RadioNodes[i].address > source)
					{
						RadioNodes[i].address = source;
						break;
					}
				response[4] = rssi;
				response[5] = snr;
			}
			//PC_Send(source, myAddress, command, response, 6);
			break;
	}*/
}

void processPCCommand(uint8_t source, uint8_t command, uint8_t* parameters)
{/*
	uint8_t response[1];
	switch (command)
	{
		case COMMAND_GET_ADDRESS:
			response[0] = RESPONSE_ACK;
			//PC_Send(source, myAddress, command, response, 6);
			break;
		case COMMAND_SET_ADDRESS:
			setAddress(parameters[3]);
			response[0] = RESPONSE_ACK;
			//PC_Send(source, myAddress, command, response, 6);
			break;
		default:
			response[0] = RESPONSE_NACK;
			//PC_Send(source, myAddress, command, response, 6);
			break;
	}*/
}

void processRadioSetupCommand(uint8_t source, uint8_t command, uint8_t* parameters, uint8_t rssi, uint8_t snr)
{/*
	uint32_t longParameter;
	uint8_t response[PARAMETERS_MAX_SIZE + 2];
	uint8_t i;
	LoRa_setupPending = true;
	
	if (source == ADDRESS_MASTER || source == ADDRESS_PC)
	{
		switch (command)
		{
			case COMMAND_BANDWIDTH:
				LoRa_setBandwidth(parameters[3]);
				break;
			case COMMAND_OUTPUT_POWER:
				LoRa_setOutputPower(parameters[3]);
				break;
			case COMMAND_CODING_RATE:
				LoRa_setCodingRate(parameters[3]);
				break;
			case COMMAND_SPREAD_FACTOR:
				LoRa_setSpreadingFactor(parameters[3]);
				break;
			case COMMAND_RX_SYM_TIMEOUT:
				LoRa_setRxSymTimeout(parameters[3]);
				break;
			case COMMAND_RX_MS_TIMEOUT:
				longParameter = (uint32_t) parameters[0] << 24 |
												(uint32_t) parameters[1] << 16 |
												(uint32_t) parameters[2] << 8 |
																	 parameters[3];
				LoRa_setRxMsTimeout(longParameter);
				break;
			case COMMAND_TX_TIMEOUT:
				longParameter = (uint32_t) parameters[0] << 24 |
												(uint32_t) parameters[1] << 16 |
												(uint32_t) parameters[2] << 8 |
																	 parameters[3];
				LoRa_setTxTimeout(longParameter);
				break;
			case COMMAND_PREAMBLE_SIZE:
				LoRa_setPreambleSize(parameters[3]);
				break;
			case COMMAND_PAYLOAD_MAX_SIZE:
				LoRa_setPayloadMaxSize(parameters[3]);
				break;
			case COMMAND_VARIABLE_PAYLOAD:
				LoRa_setPayloadMaxSize((bool) parameters[3]);
				break;
			case COMMAND_PERFORM_CRC:
				LoRa_setPerformCRC((bool) parameters[3]);
				break;
			default:
				LoRa_setupPending = false;
				break;
		}
	
		for(i = 0; i < PARAMETERS_MAX_SIZE; i++)
			response[i] = parameters[i];
		
		if (LoRa_setupPending)
			parameters[0] = RESPONSE_ACK;
		else
			parameters[0] = RESPONSE_NACK;
		
		if (source == ADDRESS_MASTER)
		{
			response[4] = rssi;
			response[5] = snr;
			LoRa_send(source, command, parameters, PARAMETERS_MAX_SIZE);
		}
		else
		{
			LoRa_setupPending = false;
			LoRa_updateParameters();
		}
	}
	else if (source == ADDRESS_BEACON)
	{
		for(i = 0; i < PARAMETERS_MAX_SIZE; i++)
			response[i] = parameters[i];
		response[4] = rssi;
		response[5] = snr;
		LoRa_setupPending = false;
	}
	else
		LoRa_setupPending = false;
	
	//PC_Send(source, myAddress, command, response, 6);*/
}

void OnTimerEvent(void)
{
	Frame_t result;
	
	DAQ_ReadData();
	result.endDevice = myAddress;
	result.nrOfMessages = 1;
	result.messages[0].command = COMMAND_TIMESTAMP;
	result.messages[0].argLength = 3;
	result.messages[0].rawArgument[0] = DAQ_Data.time.hour;
	result.messages[0].rawArgument[1] = DAQ_Data.time.minute;
	result.messages[0].rawArgument[2] = DAQ_Data.time.second;
	
	if (PC_Connected())
	{
		PC_Send(result);
	}
	/*PRINTF("\r\nTura %d\r\n", step++);
	PRINTF("Ultima citire contor\t%02d:%02d:%02d\r\n", DAQ_Data.time.hour, DAQ_Data.time.minute, DAQ_Data.time.second);
	PRINTF("Energie activa\t\t%03d.%03d\tkWh\r\n", DAQ_Data.activeEnergy / 1000, DAQ_Data.activeEnergy % 1000);
	PRINTF("Energie inductiva\t\t%03d.%03d\tkVARh\r\n", DAQ_Data.inductiveEnergy / 1000, DAQ_Data.inductiveEnergy % 1000);
	PRINTF("Energie capacitiva\t\t%03d.%03d\tkVARh\r\n", DAQ_Data.capacitiveEnergy / 1000, DAQ_Data.capacitiveEnergy % 1000);
	PRINTF("Energie reactiva\t%c%03d.%03d\tkVARh\r\n", DAQ_Data.inductive? '+': '-',  DAQ_Data.reactiveEnergy / 1000,  DAQ_Data.reactiveEnergy % 1000);
	PRINTF("Putere activa\t\t%03d.%03d\tkW\r\n", DAQ_Data.activePower / 1000, DAQ_Data.activePower % 1000);
	PRINTF("Putere reactiva\t%c%03d.%03d\tkVAR\r\n", DAQ_Data.inductive? '+': '-',  DAQ_Data.reactivePower / 1000,  DAQ_Data.reactivePower % 1000);
	PRINTF("Putere aparenta\t\t%03d.%03d\tkVA\r\n", DAQ_Data.apparentPower / 1000, DAQ_Data.apparentPower % 1000);
	PRINTF("Factor putere\t\t%c%d.%02d\r\n", DAQ_Data.inductive? '+': '-', abs(DAQ_Data.powerFactor) / 100, abs(DAQ_Data.powerFactor) % 100);
	*/DAQ_Start();
	
  TimerStart(&appTimer);
}

int main(void)
{
	HAL_Init();
	SystemClock_Config();
	DBG_Init();
	HW_Init();
	LoRa_init();
	PC_Init();
	DAQ_Init();
	ACT_Init();
	
	LoRa_setupPending = false;
	myAddress = *((uint8_t *) DEVICE_ADDRESS_LOCATION);
	LoRa_startReceiving();
	
	TimerInit(&appTimer, OnTimerEvent);
	TimerSetValue(&appTimer, APP_DUTYCYCLE * 1000); 
  OnTimerEvent();
	
  while(1)
  {
		/*ACT_SetContact(0, true);
		ACT_SetContact(1, false);
	  HAL_Delay(10000);
		ACT_SetContact(0, false);
		ACT_SetContact(1, true);
	  HAL_Delay(10000);*/
		PC_MainLoop();
		DAQ_MainLoop();
		/*if(UartState == UART_RX)
		{
			PC_Receive(&target, &command, parameters, &length);
		
			if (target == myAddress)
			{
				if ((command & COMMANDS_COMMUNICATION_MASK) != 0)
				{
					processCommunicationCommand(ADDRESS_PC, command, parameters, 0, 0);
				}
				else if ((command & COMMANDS_PC_MASK) != 0)
				{
					processPCCommand(ADDRESS_PC, command, parameters);
				}
				else if ((command & COMMANDS_RADIO_SETUP_MASK) != 0)
				{
					processRadioSetupCommand(ADDRESS_PC, command, parameters, 0 , 0);
				}
			}
			else if (target == ADDRESS_GENERAL)
			{
				if ((command & COMMANDS_PC_MASK) != 0)
				{
					processPCCommand(ADDRESS_PC, command, parameters);
				}
				else if (myAddress == ADDRESS_MASTER)
				{
					PC_Send(ADDRESS_PC, target, command, parameters, 6);
					LoRa_send(target, command, parameters, PARAMETERS_MAX_SIZE);
				}
			}
			else if (myAddress == ADDRESS_MASTER)
			{
				PC_Send(myAddress, target, command, parameters, 6);
				LoRa_send(target, command, parameters, PARAMETERS_MAX_SIZE);
			}
			UartState = UART_IDLE;
		}
		*//*
    switch(RadioState)
    {
			case RADIO_RX:
				LoRa_receive(&source, &command, parameters, &rssi, &snr);
				
				if (source == ADDRESS_MASTER || source == ADDRESS_GENERAL || myAddress == ADDRESS_MASTER)
				{
					if ((command & COMMANDS_COMMUNICATION_MASK) != 0)
					{
						processCommunicationCommand(source, command, parameters, rssi, snr);
					}
					else if ((command & COMMANDS_RADIO_SETUP_MASK) != 0)
					{
						processRadioSetupCommand(source, command, parameters, rssi, snr);
					}
				}
				else
					LoRa_startReceiving();
				break;
			case RADIO_RX_TIMEOUT:
				if (LoRa_setupPending)
				{
					LoRa_setupPending = false;
					LoRa_updateParameters();
				}
				if (myAddress == ADDRESS_MASTER)
				{
					response[0] = RESPONSE_NACK;
					response[1] = RADIO_RX_TIMEOUT;
					//PC_Send(LoRa_whoTimedOut(), myAddress, command, response, 6);
				}
				else if (myAddress >= ADDRESS_BEACON)
					LoRa_startReceiving();
				break;
			case RADIO_RX_ERROR:
				//LoRa_send(ADDRESS_BEACON, COMMAND_RESEND, 0, 0);
				if (LoRa_setupPending)
				{
					LoRa_setupPending = false;
					LoRa_updateParameters();
				}
				if (myAddress >= ADDRESS_BEACON)
					LoRa_startReceiving();
				break;
			case RADIO_TX:
				if (LoRa_setupPending)
				{
					LoRa_setupPending = false;
					LoRa_updateParameters();
				}
				if (myAddress == ADDRESS_MASTER)
				{
					LoRa_startReceiving();
				}
				if (myAddress >= ADDRESS_BEACON)
				{
					LoRa_startReceiving();
				}
				break;
			case RADIO_TX_TIMEOUT:
				response[0] = RESPONSE_NACK;
				response[1] = RADIO_TX_TIMEOUT;
				//PC_Send(LoRa_whoTimedOut(), myAddress, command, response, 6);
				if (myAddress >= ADDRESS_BEACON)
					LoRa_startReceiving();
					if (LoRa_setupPending)
					{
						LoRa_setupPending = false;
						LoRa_updateParameters();
					}
				break;
			case RADIO_LOWPOWER:
			default:
				break;
    }*/
  }
}
