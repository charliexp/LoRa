#include "hw.h"
#include "pc.h"
#include "vcom.h"

#define UART_BUFFER_SIZE			PARAMETERS_MAX_SIZE + 3

extern UART_HandleTypeDef UartHandle;

volatile uint8_t UartRxBuffer[UART_BUFFER_SIZE];
volatile uint8_t UartTxBuffer[UART_BUFFER_SIZE + 2];

UartStates_t UartState = UART_IDLE;

void PC_init(void)
{
	HAL_UART_Receive_IT(&UartHandle, (uint8_t *) UartRxBuffer, UART_BUFFER_SIZE);
}

void PC_receive(uint8_t* target, uint8_t* command, uint8_t* parameters)
{
	uint8_t i;
	*target = UartRxBuffer[IDX_TARGET_ADDRESS];
	*command = UartRxBuffer[IDX_COMMAND];
	for (i = 0; i < PARAMETERS_MAX_SIZE; i++)
		parameters[i] = UartRxBuffer[IDX_COMMAND_PARAMETER + i];
	UartState = UART_IDLE;
}

void PC_send(uint8_t source, uint8_t target, uint8_t command, uint8_t* data, uint8_t length)
{
	uint8_t i;
	UartTxBuffer[IDX_SOURCE_ADDRESS] = source;
	UartTxBuffer[IDX_TARGET_ADDRESS] = target;
	UartTxBuffer[IDX_COMMAND] = command;
	for (i = 0; i < length; i++)
		UartTxBuffer[IDX_COMMAND_PARAMETER + i] = data[i];
	for (i = 0; i < length + 3; i++)
		PRINTF("%c", UartTxBuffer[i]);
}

void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart)
{
	UartState = UART_RX;
	HAL_UART_Receive_IT(&UartHandle, (uint8_t *) UartRxBuffer, UART_BUFFER_SIZE);
}
