#ifndef PC_H
#define PC_H

#include "commands.h"
#include "hw.h"

void PC_Init(void);
void PC_MainLoop(void);
void PC_ProcessMessage(void);
void PC_Send(Message_t message);

#endif /* PC_H */
