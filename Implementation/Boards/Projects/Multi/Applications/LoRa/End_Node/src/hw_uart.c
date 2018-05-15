/* Includes ------------------------------------------------------------------*/
#include "hw_uart.h"
#include <stdarg.h>

/* Private typedef -----------------------------------------------------------*/
/* Private define ------------------------------------------------------------*/
#define BUFSIZE 256
/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
/* Uart Handle */
UART_HandleTypeDef UartHandle;
/* Send buffer */
static uint8_t TxBuffer[BUFSIZE];
/* Send buffer max index */
static uint8_t TxLength;
/* UART Tx state */
static UartTxState_t TxState;

/* Receive buffer */
static volatile uint8_t RxBuffer[BUFSIZE];
/* Receive buffer index */
static volatile uint8_t RxLength;
/* UART Rx state */
static UartRxState_t RxState;

/* Private function prototypes -----------------------------------------------*/
void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart)
{
	if (RxLength > 2 &&
			RxBuffer[RxLength - 1] == '\r' &&
			RxBuffer[RxLength] == '\n')
		RxState = UART_RX_AVAILABLE;
	
	RxLength++;
	HAL_UART_Receive_IT(&UartHandle, (uint8_t *) RxBuffer + RxLength, 1);
}

void HAL_UART_TxCpltCallback(UART_HandleTypeDef *huart)
{
	TxState = UART_TX_AVAILABLE;
}

/* Functions Definition ------------------------------------------------------*/
void UART_Init(void)
{
  UartHandle.Instance        = UART_PERIPHERAL;
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
  
  HAL_NVIC_SetPriority(UART_IRQn, 0x1, 0);
  HAL_NVIC_EnableIRQ(UART_IRQn);
	
	TxState = UART_TX_AVAILABLE;
	TxLength = 0;
	
	RxState = UART_RX_PENDING;
	RxLength = 0;
	HAL_UART_Receive_IT(&UartHandle, (uint8_t *) RxBuffer, 1);
}

UartRxState_t UART_Receive(uint8_t *buffer, uint8_t *length)
{
	int i;
	UartRxState_t returnValue = RxState;
	
	if (RxState == UART_RX_AVAILABLE)
	{
		for (i = 0; i < RxLength; i++)
			buffer[i] = RxBuffer[i];
		*length = RxLength;
		RxLength = 0;
		RxState = UART_RX_PENDING;
	}
	
	return returnValue;
}

UartTxState_t UART_Send(uint8_t *buffer, uint8_t length)
{
	int i;
	UartTxState_t returnValue = TxState;
	
	if (TxState == UART_TX_AVAILABLE)
	{
		for (i = 0; i < length; i++)
			TxBuffer[i] = buffer[i];
		TxLength = length;
		TxState = UART_TX_BUSY;
		
		HAL_UART_Transmit_IT(&UartHandle, TxBuffer, TxLength);
	}
	
	return returnValue;
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

  /* Enable USART1 clock */
  UART_CLK_ENABLE(); 
  
  /*##-2- Configure peripheral GPIO ##########################################*/  
  GPIO_InitTypeDef  GPIO_InitStruct={0};
    /* Enable GPIO TX/RX clock */
  UART_TX_GPIO_CLK_ENABLE();
  UART_RX_GPIO_CLK_ENABLE();
    /* UART TX GPIO pin configuration  */
  GPIO_InitStruct.Pin       = UART_TX_PIN;
  GPIO_InitStruct.Mode      = GPIO_MODE_AF_PP;
  GPIO_InitStruct.Pull      = GPIO_PULLUP;
  GPIO_InitStruct.Speed     = GPIO_SPEED_HIGH;
  GPIO_InitStruct.Alternate = UART_TX_AF;

  HAL_GPIO_Init(UART_TX_GPIO_PORT, &GPIO_InitStruct);

  /* UART RX GPIO pin configuration  */
  GPIO_InitStruct.Pin = UART_RX_PIN;
  GPIO_InitStruct.Alternate = UART_RX_AF;

  HAL_GPIO_Init(UART_RX_GPIO_PORT, &GPIO_InitStruct);
}

/**
  * @brief UART MSP DeInit
  * @param huart: uart handle
  * @retval None
  */
void HAL_UART_MspDeInit(UART_HandleTypeDef *huart)
{
  GPIO_InitTypeDef GPIO_InitStructure={0};
  
  UART_TX_GPIO_CLK_ENABLE();
  UART_RX_GPIO_CLK_ENABLE();

  GPIO_InitStructure.Mode = GPIO_MODE_ANALOG;
  GPIO_InitStructure.Pull = GPIO_NOPULL;
  
  GPIO_InitStructure.Pin =  UART_TX_PIN ;
  HAL_GPIO_Init(  UART_TX_GPIO_PORT, &GPIO_InitStructure );
  
  GPIO_InitStructure.Pin =  UART_RX_PIN ;
  HAL_GPIO_Init(  UART_RX_GPIO_PORT, &GPIO_InitStructure ); 
}
