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
	if (myAddress == ADDRESS_MASTER)
	{
		if (source == ADDRESS_PC)
		{
			switch (command)
			{
				case COMMAND_IS_PRESENT:
					PRINTF("Checking for present beacons\n\r");
					LoRa_send(ADDRESS_GENERAL, COMMAND_IS_PRESENT, 0, 0);
					break;
				default:
					PRINTF("Received unknown command %x from %d\n\r", command, source);
					break;
			}
		}
		else if (source >= ADDRESS_BEACON)
		{
			switch (command)
			{
				case COMMAND_IS_PRESENT:
					if (parameters[3] == RESPONSE_ACK)
						PRINTF("Beacon %u ACK\n\r", source);
					else if (parameters[3] == RESPONSE_NACK)
						PRINTF("Beacon %u NACK\n\r", source);
					break;
				default:
					PRINTF("Received unknown command %x from %d\n\r", command, source);
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
					PRINTF("Asked if present\n\r");
					DelayMs((myAddress - 2) * 1000);
					parameters[3] = RESPONSE_ACK;
					LoRa_send(ADDRESS_MASTER, COMMAND_IS_PRESENT, parameters, PARAMETERS_MAX_SIZE);
					break;
				default:
					PRINTF("Received unknown command %x from %d\n\r", command, source);
					break;
			}
		}
	}
}

void processPCCommand(uint8_t source, uint8_t command, uint8_t* parameters)
{
	switch (command)
	{
		case COMMAND_GET_ADDRESS:
			PRINTF("My address is %d\n\r", myAddress);
			break;
		case COMMAND_SET_ADDRESS:
			setAddress(parameters[3]);
			PRINTF("Set my address to %d\n\r", myAddress);
			break;
		default:
			PRINTF("Received unknown command %x from %d\n\r", command, source);
			break;
	}
}

void processRadioSetupCommand(uint8_t source, uint8_t command, uint8_t* parameters)
{
	uint32_t longParameter;
	switch (command)
	{
		case COMMAND_BANDWIDTH:
			PRINTF("Bandwidth: %u\n\r", parameters[3]);
			//fw command
			//RadioTxBuffer[IDX_TARGET_ADDRESS] = ADDRESS_BEACON;
			LoRa_setBandwidth(parameters[3]);
			break;
		case COMMAND_OUTPUT_POWER:
			PRINTF("Output Power: %u\n\r", parameters[3]);
			LoRa_setOutputPower(parameters[3]);
			break;
		case COMMAND_CODING_RATE:
			PRINTF("Coding Rate: %u\n\r", parameters[3]);
			LoRa_setCodingRate(parameters[3]);
			break;
		case COMMAND_SPREAD_FACTOR:
			PRINTF("Spreading Factor: %u\n\r", parameters[3]);
			LoRa_setSpreadingFactor(parameters[3]);
			break;
		case COMMAND_RX_SYM_TIMEOUT:
			PRINTF("Rx Timeout (sym): %u\n\r", parameters[3]);
			LoRa_setRxSymTimeout(parameters[3]);
			break;
		case COMMAND_RX_MS_TIMEOUT:
			longParameter = (uint32_t) parameters[0] << 24 |
											(uint32_t) parameters[1] << 16 |
											(uint32_t) parameters[2] << 8 |
																 parameters[3];
			PRINTF("Rx Timeout (ms): %u\n\r", longParameter);
			LoRa_setRxMsTimeout(longParameter);
			break;
		case COMMAND_TX_TIMEOUT:
			longParameter = (uint32_t) parameters[0] << 24 |
											(uint32_t) parameters[1] << 16 |
											(uint32_t) parameters[2] << 8 |
																 parameters[3];
			PRINTF("Tx Timeout (ms): %u\n\r", longParameter);
			LoRa_setTxTimeout(longParameter);
			break;
		case COMMAND_PREAMBLE_SIZE:
			PRINTF("Preamble Size: %u\n\r", parameters[3]);
			LoRa_setPreambleSize(parameters[3]);
			break;
		case COMMAND_PAYLOAD_MAX_SIZE:
			PRINTF("Payload Max Size: %u\n\r", parameters[3]);
			LoRa_setPayloadMaxSize(parameters[3]);
			break;
		case COMMAND_VARIABLE_PAYLOAD:
			PRINTF("Variable Payload: %u\n\r", parameters[3]);
			LoRa_setPayloadMaxSize((bool) parameters[3]);
			break;
		case COMMAND_PERFORM_CRC:
			PRINTF("Perform CRC: %u\n\r", parameters[3]);
			LoRa_setPerformCRC((bool) parameters[3]);
			break;
		default:
			PRINTF("Received unknown command %x from %d\n\r", command, source);
			break;
	}
}

int main(void)
{
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
					processCommunicationCommand(source, command, parameters);
				}
				else if ((command & COMMANDS_PC_MASK) != 0)
				{
					processPCCommand(source, command, parameters);
				}
				else if ((command & COMMANDS_RADIO_SETUP_MASK) != 0)
				{
					processRadioSetupCommand(source, command, parameters);
				}
			}
			else if (target == ADDRESS_GENERAL)
			{
				if ((command & COMMANDS_PC_MASK) != 0)
				{
					processPCCommand(source, command, parameters);
				}
				else if (myAddress == ADDRESS_MASTER)
				{
					PRINTF("Forwarding command %x to all devices\n\r", command);
					LoRa_send(ADDRESS_GENERAL, command, parameters, PARAMETERS_MAX_SIZE);
				}
			}
			else if (myAddress == ADDRESS_MASTER)
			{
				PRINTF("Forwarding command %x to: %d\n\r", command, target);
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
					if (LoRa_whoTimedOut() == ADDRESS_GENERAL)
						PRINTF("No beacon responding\n\r");
					else
						PRINTF("Beacon %u not responding\n\r", LoRa_whoTimedOut());
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
