#ifndef PC_H
#define PC_H

#include "commands.h"

typedef enum UartStates_t
{
    UART_IDLE,
		UART_RX
}UartStates_t;

extern UartStates_t UartState;

void PC_init(void);
void PC_receive(uint8_t* target, uint8_t* command, uint8_t* parameters);
void PC_send(uint8_t source, uint8_t target, uint8_t command, uint8_t* data, uint8_t length);

#endif /* PC_H */
