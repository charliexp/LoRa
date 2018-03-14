#include <string.h>
#include "commands.h"
#include "lora.h"
#include "pc.h"
#include "delay.h"

#define DEVICE_ADDRESS_LOCATION		0x08080000

uint8_t myAddress;

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

void processCommunicationCommand(uint8_t source, uint8_t command, uint8_t* parameters)
{
	uint8_t response[1];
	if (myAddress == ADDRESS_MASTER)
	{
		if (source == ADDRESS_PC)
		{
			switch (command)
			{
				case COMMAND_IS_PRESENT:
					LoRa_send(ADDRESS_GENERAL, COMMAND_IS_PRESENT, 0, 0);
					break;
				default:
					response[0] = RESPONSE_NACK;
					PC_send(myAddress, ADDRESS_PC, command, response, 4);
					break;
			}
		}
		else if (source >= ADDRESS_BEACON)
		{
			switch (command)
			{
				case COMMAND_IS_PRESENT:
					response[0] = parameters[3];
					PC_send(source, myAddress, command, response, 4);
					break;
				default:
					response[0] = RESPONSE_NACK;
					PC_send(source, myAddress, command, response, 4);
					break;
			}
		}
	}
	else if (myAddress >= ADDRESS_BEACON)
	{
		if (source == ADDRESS_MASTER)
		{
			switch (command)
			{
				case COMMAND_IS_PRESENT:
					DelayMs((myAddress - 2) * 1000);
					parameters[3] = RESPONSE_ACK;
					response[0] = RESPONSE_ACK;
					PC_send(myAddress, ADDRESS_MASTER, command, response, 4);
					LoRa_send(ADDRESS_MASTER, COMMAND_IS_PRESENT, parameters, PARAMETERS_MAX_SIZE);
					break;
				default:
					response[0] = RESPONSE_NACK;
					PC_send(myAddress, ADDRESS_MASTER, command, response, 4);
					break;
			}
		}
	}
}

void processPCCommand(uint8_t source, uint8_t command, uint8_t* parameters)
{
	uint8_t response[1];
	switch (command)
	{
		case COMMAND_GET_ADDRESS:
			response[0] = RESPONSE_ACK;
			PC_send(myAddress, ADDRESS_PC, command, response, 4);
			break;
		case COMMAND_SET_ADDRESS:
			setAddress(parameters[3]);
			response[0] = RESPONSE_ACK;
			PC_send(myAddress, ADDRESS_PC, command, response, 4);
			break;
		default:
			response[0] = RESPONSE_NACK;
			PC_send(myAddress, ADDRESS_PC, command, response, 4);
			break;
	}
}

void processRadioSetupCommand(uint8_t source, uint8_t command, uint8_t* parameters)
{
	uint32_t longParameter;
	switch (command)
	{
		case COMMAND_BANDWIDTH:
			PC_send(source, myAddress, command, parameters, 4);
			LoRa_setBandwidth(parameters[3]);
			break;
		case COMMAND_OUTPUT_POWER:
			PC_send(source, myAddress, command, parameters, 4);
			LoRa_setOutputPower(parameters[3]);
			break;
		case COMMAND_CODING_RATE:
			PC_send(source, myAddress, command, parameters, 4);
			LoRa_setCodingRate(parameters[3]);
			break;
		case COMMAND_SPREAD_FACTOR:
			PC_send(source, myAddress, command, parameters, 4);
			LoRa_setSpreadingFactor(parameters[3]);
			break;
		case COMMAND_RX_SYM_TIMEOUT:
			PC_send(source, myAddress, command, parameters, 4);
			LoRa_setRxSymTimeout(parameters[3]);
			break;
		case COMMAND_RX_MS_TIMEOUT:
			longParameter = (uint32_t) parameters[0] << 24 |
											(uint32_t) parameters[1] << 16 |
											(uint32_t) parameters[2] << 8 |
																 parameters[3];
			PC_send(source, myAddress, command, parameters, 4);
			LoRa_setRxMsTimeout(longParameter);
			break;
		case COMMAND_TX_TIMEOUT:
			longParameter = (uint32_t) parameters[0] << 24 |
											(uint32_t) parameters[1] << 16 |
											(uint32_t) parameters[2] << 8 |
																 parameters[3];
			PC_send(source, myAddress, command, parameters, 4);
			LoRa_setTxTimeout(longParameter);
			break;
		case COMMAND_PREAMBLE_SIZE:
			PC_send(source, myAddress, command, parameters, 4);
			LoRa_setPreambleSize(parameters[3]);
			break;
		case COMMAND_PAYLOAD_MAX_SIZE:
			PC_send(source, myAddress, command, parameters, 4);
			LoRa_setPayloadMaxSize(parameters[3]);
			break;
		case COMMAND_VARIABLE_PAYLOAD:
			PC_send(source, myAddress, command, parameters, 4);
			LoRa_setPayloadMaxSize((bool) parameters[3]);
			break;
		case COMMAND_PERFORM_CRC:
			PC_send(source, myAddress, command, parameters, 4);
			LoRa_setPerformCRC((bool) parameters[3]);
			break;
		default:
			PC_send(source, myAddress, command, parameters, 4);
			break;
	}
}

int main(void)
{
	uint8_t response[4];
	uint8_t source;
	uint8_t target;
	uint8_t command;
	uint8_t parameters[PARAMETERS_MAX_SIZE];
				
	HAL_Init();
	SystemClock_Config();
	DBG_Init();
	HW_Init();
	LoRa_init();
	PC_init();
	
	myAddress = *((uint8_t *) DEVICE_ADDRESS_LOCATION);
	if (myAddress >= ADDRESS_BEACON)
		LoRa_startReceiving();
	       
  while(1)
  {
		if(UartState == UART_RX)
		{
			PC_receive(&target, &command, parameters);
		
			if (target == myAddress)
			{
				if ((command & COMMANDS_COMMUNICATION_MASK) != 0)
				{
					processCommunicationCommand(ADDRESS_PC, command, parameters);
				}
				else if ((command & COMMANDS_PC_MASK) != 0)
				{
					processPCCommand(ADDRESS_PC, command, parameters);
				}
				else if ((command & COMMANDS_RADIO_SETUP_MASK) != 0)
				{
					processRadioSetupCommand(ADDRESS_PC, command, parameters);
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
					response[0] = RESPONSE_ACK;
					PC_send(myAddress, ADDRESS_GENERAL, command, response, 4);
					LoRa_send(ADDRESS_GENERAL, command, parameters, PARAMETERS_MAX_SIZE);
				}
			}
			else if (myAddress == ADDRESS_MASTER)
			{
				response[0] = RESPONSE_ACK;
				PC_send(myAddress, target, command, response, 4);
				LoRa_send(ADDRESS_GENERAL, command, parameters, PARAMETERS_MAX_SIZE);
			}
			UartState = UART_IDLE;
		}
		
    switch(RadioState)
    {
			case RADIO_RX:
				LoRa_receive(&source, &command, parameters);
				
				if ((command & COMMANDS_COMMUNICATION_MASK) != 0)
				{
					processCommunicationCommand(source, command, parameters);
				}
				else if ((command & COMMANDS_RADIO_SETUP_MASK) != 0)
				{
					processRadioSetupCommand(source, command, parameters);
				}
				if (myAddress >= ADDRESS_BEACON)
					LoRa_startReceiving();
		
				break;
			case RADIO_RX_TIMEOUT:
				if (myAddress == ADDRESS_MASTER)
				{
					response[0] = RESPONSE_NACK;
					response[1] = RADIO_RX_TIMEOUT;
					PC_send(myAddress, LoRa_whoTimedOut(), command, response, 4);
				}
				else if (myAddress >= ADDRESS_BEACON)
					LoRa_startReceiving();
				break;
			case RADIO_RX_ERROR:
				//LoRa_send(ADDRESS_BEACON, COMMAND_RESEND, 0, 0);
				if (myAddress >= ADDRESS_BEACON)
					LoRa_startReceiving();
				break;
			case RADIO_TX:
				if (myAddress == ADDRESS_MASTER)
				{
					LoRa_startReceiving();
				}
				if (myAddress >= ADDRESS_BEACON)
					LoRa_startReceiving();
				break;
			case RADIO_TX_TIMEOUT:
				response[0] = RESPONSE_NACK;
				response[1] = RADIO_TX_TIMEOUT;
				PC_send(LoRa_whoTimedOut(), myAddress, command, parameters, 4);
				PRINTF("TX timeout. Increase Tx Timeout parameter\n\r");
				if (myAddress >= ADDRESS_BEACON)
					LoRa_startReceiving();
				break;
			case RADIO_LOWPOWER:
			default:
				break;
    }
  }
}
