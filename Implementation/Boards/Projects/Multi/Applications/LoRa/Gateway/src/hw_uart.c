/* Includes ------------------------------------------------------------------*/
#include "hw_uart.h"
#include <stdarg.h>

/* Private typedef -----------------------------------------------------------*/
/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
/* Uart Handle */
UART_HandleTypeDef UartHandle;
/* Receive buffer */
static uint8_t RxBuffer[UART_BUFFSIZE];
/* Index of first byte not forwarded to upper layer */
static uint16_t RxProcessedIndex;
/* Index of last byte checked for terminator character */
static uint16_t RxCheckedIndex;
/* Index of byte to be received */
static uint16_t RxReceiveIndex;


uint32_t lastSuccessfulRead;
uint32_t lastSend;
UartRxState_t UartRxState;

/* Private function prototypes -----------------------------------------------*/

void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart)
{
	if (RxReceiveIndex == UART_BUFFSIZE - 1)
		RxReceiveIndex = 0;
	else
		RxReceiveIndex++;
	HAL_UART_Receive_IT(&UartHandle, RxBuffer + RxReceiveIndex, 1);
}

void HAL_UART_ErrorCallback(UART_HandleTypeDef *huart)
{
	HAL_UART_AbortReceive(huart);
	HAL_UART_Receive_IT(&UartHandle, RxBuffer + RxReceiveIndex, 1);
	UartRxState = UART_RX_TIMEOUT;
}

/* Functions Definition ------------------------------------------------------*/
void UART_Init(void)
{
  UartHandle.Instance        = DAQ_USARTX;
  UartHandle.Init.BaudRate   = 115200;
  UartHandle.Init.WordLength = UART_WORDLENGTH_8B;
  UartHandle.Init.StopBits   = UART_STOPBITS_1;
  UartHandle.Init.Parity     = UART_PARITY_NONE;
  UartHandle.Init.HwFlowCtl  = UART_HWCONTROL_NONE;
  UartHandle.Init.Mode       = UART_MODE_TX_RX;
  
  if(HAL_UART_Init(&UartHandle) != HAL_OK)
  {
    /* Initialization Error */
    Error_Handler(); 
  }
	
  HAL_NVIC_SetPriority(DAQ_USARTX_IRQn, 0x1, 0);
  HAL_NVIC_EnableIRQ(DAQ_USARTX_IRQn);
	
	RxProcessedIndex = 0;
	RxCheckedIndex = 0;
	RxReceiveIndex = 0;
	lastSuccessfulRead = 0;
	lastSend = 0;
	
	HAL_UART_Receive_IT(&UartHandle, RxBuffer + RxReceiveIndex, 1);
}

UartRxState_t UART_Receive(uint8_t *buffer, uint16_t *length, uint8_t terminatorChar)
{
	*length = 0;
	UartRxState_t returnValue = UartRxState;
	
	while (RxCheckedIndex != RxReceiveIndex && UartRxState == UART_RX_PENDING)
	{
		if (RxBuffer[RxCheckedIndex - 1] == terminatorChar)
		{
			UartRxState = UART_RX_AVAILABLE;
		}
		if (RxCheckedIndex == UART_BUFFSIZE - 1)
			RxCheckedIndex = 0;
		else
			RxCheckedIndex++;
	}
	
	if (HAL_GetTick() - lastSend > 5 * 1000)
		HAL_UART_ErrorCallback(&UartHandle);
		
	if (UartRxState != UART_RX_PENDING)
	{
		while (RxProcessedIndex != RxCheckedIndex)
		{
			buffer[*length] = RxBuffer[RxProcessedIndex];
			(*length)++;
			if (RxProcessedIndex == UART_BUFFSIZE - 1)
				RxProcessedIndex = 0;
			else
				RxProcessedIndex++;
		}
		
		returnValue = UartRxState;
		UartRxState = UART_RX_PENDING;
		
		if (UartRxState == UART_RX_AVAILABLE)
			lastSuccessfulRead = HAL_GetTick();
	}
	
	return returnValue;
}

void UART_Send(uint8_t *buffer, uint8_t length)
{
	lastSend = HAL_GetTick();
	HAL_UART_Transmit(&UartHandle, buffer, length, 2000);
}

void UART_DeInit(void)
{
  HAL_UART_DeInit(&UartHandle);
}

/**
  * @brief UART MSP Initialization 
  *        This function configures the hardware resources used in this example: 
  *           - Peripheral's clock enable
  *           - Peripheral's GPIO Configuration  
  *           - NVIC configuration for UART interrupt request enable
  * @param huart: UART handle pointer
  * @retval None
  */
void HAL_UART_MspInit(UART_HandleTypeDef *huart)
{
  
  /*##-1- Enable peripherals and GPIO Clocks #################################*/

  /* Enable USART clock */
  USARTX_CLK_ENABLE(); 
  DAQ_USARTX_CLK_ENABLE(); 
  
  /*##-2- Configure peripheral GPIO ##########################################*/  
  vcom_IoInit( );
	
  GPIO_InitTypeDef  GPIO_InitStruct={0};
    /* Enable GPIO TX/RX clock */
  DAQ_USARTX_TX_GPIO_CLK_ENABLE();
  DAQ_USARTX_RX_GPIO_CLK_ENABLE();
    /* UART TX GPIO pin configuration  */
  GPIO_InitStruct.Pin       = DAQ_USARTX_TX_PIN;
  GPIO_InitStruct.Mode      = GPIO_MODE_AF_PP;
  GPIO_InitStruct.Pull      = GPIO_PULLUP;
  GPIO_InitStruct.Speed     = GPIO_SPEED_HIGH;
  GPIO_InitStruct.Alternate = DAQ_USARTX_TX_AF;

  HAL_GPIO_Init(DAQ_USARTX_TX_GPIO_PORT, &GPIO_InitStruct);

  /* UART RX GPIO pin configuration  */
  GPIO_InitStruct.Pin = DAQ_USARTX_RX_PIN;
  GPIO_InitStruct.Alternate = DAQ_USARTX_RX_AF;

  HAL_GPIO_Init(DAQ_USARTX_RX_GPIO_PORT, &GPIO_InitStruct);
}

/**
  * @brief UART MSP DeInit
  * @param huart: uart handle
  * @retval None
  */
void HAL_UART_MspDeInit(UART_HandleTypeDef *huart)
{
  vcom_IoDeInit( );
	
  GPIO_InitTypeDef GPIO_InitStructure={0};
  
  DAQ_USARTX_TX_GPIO_CLK_ENABLE();
  DAQ_USARTX_RX_GPIO_CLK_ENABLE();

  GPIO_InitStructure.Mode = GPIO_MODE_ANALOG;
  GPIO_InitStructure.Pull = GPIO_NOPULL;
  
  GPIO_InitStructure.Pin =  DAQ_USARTX_TX_PIN ;
  HAL_GPIO_Init(  DAQ_USARTX_TX_GPIO_PORT, &GPIO_InitStructure );
  
  GPIO_InitStructure.Pin =  DAQ_USARTX_RX_PIN ;
  HAL_GPIO_Init(  DAQ_USARTX_RX_GPIO_PORT, &GPIO_InitStructure ); 
}
