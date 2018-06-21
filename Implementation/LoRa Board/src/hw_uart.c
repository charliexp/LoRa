/* Includes ------------------------------------------------------------------*/
#include "hw_uart.h"

/* Private typedef -----------------------------------------------------------*/
/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
/* Uart Handles */
UartHandle_t DAQ_UartHandle;
UartHandle_t DBG_UartHandle;

/* Private function prototypes -----------------------------------------------*/
void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart)
{
	if (huart->Instance == DAQ_USARTX)
	{
		if (DAQ_UartHandle.RxReceiveIndex == UART_BUFFSIZE - 1)
			DAQ_UartHandle.RxReceiveIndex = 0;
		else
			DAQ_UartHandle.RxReceiveIndex++;
		HAL_UART_Receive_IT(huart, DAQ_UartHandle.RxBuffer + DAQ_UartHandle.RxReceiveIndex, 1);
	}
	else if (huart->Instance == DBG_USARTX)
	{
		if (DBG_UartHandle.RxReceiveIndex == UART_BUFFSIZE - 1)
			DBG_UartHandle.RxReceiveIndex = 0;
		else
			DBG_UartHandle.RxReceiveIndex++;
		HAL_UART_Receive_IT(huart, DBG_UartHandle.RxBuffer + DBG_UartHandle.RxReceiveIndex, 1);
	}
}

void HAL_UART_ErrorCallback(UART_HandleTypeDef *huart)
{
	if (huart->Instance == DAQ_USARTX)
	{
		HAL_UART_AbortReceive(huart);
		HAL_UART_Receive_IT(huart, DAQ_UartHandle.RxBuffer + DAQ_UartHandle.RxReceiveIndex, 1);
		DAQ_UartHandle.RxState = UART_RX_TIMEOUT;
	}
	else if (huart->Instance == DBG_USARTX)
	{
		HAL_UART_AbortReceive(huart);
		HAL_UART_Receive_IT(huart, DBG_UartHandle.RxBuffer + DBG_UartHandle.RxReceiveIndex, 1);
		DBG_UartHandle.RxState = UART_RX_TIMEOUT;
	}
}

/* Functions Definition ------------------------------------------------------*/
void UART_Init(void)
{
  DAQ_UartHandle.lowLevelHandle.Instance        = DAQ_USARTX;
  DAQ_UartHandle.lowLevelHandle.Init.BaudRate   = 9600;
  DAQ_UartHandle.lowLevelHandle.Init.WordLength = UART_WORDLENGTH_8B;
  DAQ_UartHandle.lowLevelHandle.Init.StopBits   = UART_STOPBITS_1;
  DAQ_UartHandle.lowLevelHandle.Init.Parity     = UART_PARITY_EVEN;
  DAQ_UartHandle.lowLevelHandle.Init.HwFlowCtl  = UART_HWCONTROL_NONE;
  DAQ_UartHandle.lowLevelHandle.Init.Mode       = UART_MODE_TX_RX;
  
  if(HAL_UART_Init(&DAQ_UartHandle.lowLevelHandle) != HAL_OK)
  {
    /* Initialization Error */
    Error_Handler(); 
  }
	
  HAL_NVIC_SetPriority(DAQ_USARTX_IRQn, 0x2, 0);
  HAL_NVIC_EnableIRQ(DAQ_USARTX_IRQn);
	
	DAQ_UartHandle.RxProcessedIndex = 0;
	DAQ_UartHandle.RxCheckedIndex = 0;
	DAQ_UartHandle.RxReceiveIndex = 0;
	DAQ_UartHandle.lastSendTime = 0;
	DAQ_UartHandle.lastReceiveTime = 0;
	
	HAL_UART_Receive_IT(&DAQ_UartHandle.lowLevelHandle, DAQ_UartHandle.RxBuffer + DAQ_UartHandle.RxReceiveIndex, 1);
}

UartRxState_t UART_ReceiveUntilChar(UartHandle_t *uart, uint8_t *buffer, uint16_t *length, uint8_t terminatorChar)
{
	*length = 0;
	UartRxState_t returnValue = uart->RxState;
	
	while (uart->RxCheckedIndex != uart->RxReceiveIndex && uart->RxState == UART_RX_PENDING)
	{
		if (uart->RxBuffer[uart->RxCheckedIndex - 1] == terminatorChar)
		{
			uart->RxState = UART_RX_AVAILABLE;
		}
		if (uart->RxCheckedIndex == UART_BUFFSIZE - 1)
			uart->RxCheckedIndex = 0;
		else
			uart->RxCheckedIndex++;
	}
	//timeout as parameter
	if (HAL_GetTick() - uart->lastSendTime > 5 * 1000)
		HAL_UART_ErrorCallback(&uart->lowLevelHandle);
		
	if (uart->RxState != UART_RX_PENDING)
	{
		while (uart->RxProcessedIndex != uart->RxCheckedIndex)
		{
			buffer[*length] = uart->RxBuffer[uart->RxProcessedIndex];
			(*length)++;
			if (uart->RxProcessedIndex == UART_BUFFSIZE - 1)
				uart->RxProcessedIndex = 0;
			else
				uart->RxProcessedIndex++;
		}
		
		if (returnValue == UART_RX_AVAILABLE)
			uart->lastReceiveTime = HAL_GetTick();
		
		returnValue = uart->RxState;
		uart->RxState = UART_RX_PENDING;
	}
	
	return returnValue;
}

UartRxState_t UART_ReceiveFixedLength(UartHandle_t *uart, uint8_t *buffer, uint16_t length)
{
	uint8_t i = 0;
	UartRxState_t returnValue = DAQ_UartHandle.RxState;
	
	if ((uart->RxCheckedIndex < uart->RxReceiveIndex) &&
		(uart->RxReceiveIndex - uart->RxCheckedIndex >= length))
	{
		uart->RxState = UART_RX_AVAILABLE;
		uart->RxCheckedIndex += length;
	}
	else if ((uart->RxCheckedIndex > uart->RxReceiveIndex) &&
		((uart->RxReceiveIndex + UART_BUFFSIZE) - uart->RxCheckedIndex > length))
	{
		uart->RxState = UART_RX_AVAILABLE;
		uart->RxCheckedIndex += length;
		if (uart->RxCheckedIndex >= UART_BUFFSIZE)
			uart->RxCheckedIndex -= UART_BUFFSIZE;
	}
	//timeout as parameter
	/*
	if (HAL_GetTick() - uart->lastReceiveTime > 5 * 1000)
		HAL_UART_ErrorCallback(&uart->lowLevelHandle);
		*/
	if (uart->RxState != UART_RX_PENDING)
	{
		while (i < length)
		{
			buffer[i] = uart->RxBuffer[uart->RxProcessedIndex];
			i++;
			if (uart->RxProcessedIndex == UART_BUFFSIZE - 1)
				uart->RxProcessedIndex = 0;
			else
				uart->RxProcessedIndex++;
		}
		
		if (returnValue == UART_RX_AVAILABLE)
			uart->lastReceiveTime = HAL_GetTick();
		
		returnValue = uart->RxState;
		uart->RxState = UART_RX_PENDING;
	}
	
	return returnValue;
}

void UART_Send(UartHandle_t *uart, uint8_t *buffer, uint8_t length)
{
	uart->lastSendTime = HAL_GetTick();
	HAL_UART_Transmit(&uart->lowLevelHandle, buffer, length, 2000);
}

void UART_DeInit(void)
{
  HAL_UART_DeInit(&DAQ_UartHandle.lowLevelHandle);
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
  DBG_USARTX_CLK_ENABLE(); 
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
