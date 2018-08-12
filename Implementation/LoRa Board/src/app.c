/* Includes ------------------------------------------------------------------*/
#include "app.h"
#include "compensator.h"
#include "lora.h"
#include <string.h>

/* Private define ------------------------------------------------------------*/
#define NULL_PTR		((void*)0)

/* Private typedef -----------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
/* Private functions ---------------------------------------------------------*/
void App_Respond(Command_t command, bool ok, uint8_t *parameters, uint8_t paramLength)
{
	Message_t response;
	response.paramLength = paramLength;
	memcpy(response.params, parameters, paramLength);
	
	if (ok)
	{
		response.command = command;
		if (paramLength == 0)
		{
			response.paramLength = 1;
			response.params[0] = ACK;
		}
	}
	else
	{
		response.command = COMMAND_ERROR;
		if (paramLength == 0)
		{
			response.paramLength = 1;
			response.params[0] = NAK;
		}
	}
	
	LoRa_QueueMessage(response);
}

/* Public Functions ----------------------------------------------------------*/
void Comp_ProcessRequest(Message_t message)
{
	Comp_error_e returnValue;
	
	#ifdef DEBUG
	#endif
	
	switch (message.command)
	{
		case COMMAND_CHANGE_COMPENSATOR:
			Comp_Update((message.params[2] >> 4) & 0x0F,
				(message.params[0] << 8) | message.params[1],
				((message.params[2] >> 0) & 0x0F));
			App_Respond(message.command, true, NULL_PTR, 0);
			break;
		case COMMAND_SET_COMPENSATOR:
			returnValue = Comp_Set((message.params[0] >> 4) & 0x0F,
				(message.params[0] >> 0) & 0x0F);
			if (returnValue == COMP_OK)
				App_Respond(message.command, true, NULL_PTR, 0);
			else
				App_Respond(message.command, false, (uint8_t*) &returnValue, 1);
			break;
		default:
			break;
	}
}
