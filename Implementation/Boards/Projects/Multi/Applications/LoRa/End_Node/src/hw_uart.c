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

/* Receive buffer */
static uint8_t RxBuffer[BUFSIZE];
/* Receive buffer index */
static uint8_t RxLength;
/* UART Rx state */
static UartRxState_t RxState;

/* Private function prototypes -----------------------------------------------*/
void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart)
{
	if (RxLength > 2 &&
			((RxBuffer[RxLength - 1] == '\r' &&
			RxBuffer[RxLength] == '\n') ||
			(RxBuffer[RxLength - 1] == 0x03)))
	{
		RxState = UART_RX_AVAILABLE;
		HAL_UART_Receive_IT(&UartHandle, RxBuffer, 1);
	}
	else
	{
		HAL_UART_Receive_IT(&UartHandle, RxBuffer + RxLength + 1, 1);
	}
	
	RxLength++;
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
	
	RxState = UART_RX_PENDING;
	RxLength = 0;
}

UartRxState_t UART_Receive(uint8_t *buffer, uint8_t *length)
{
	UartRxState_t returnValue = RxState;
	
	HAL_UART_Receive(&UartHandle, buffer, 256, 4000);
	
	return returnValue;
}

void UART_Send(uint8_t *buffer, uint8_t length)
{
	HAL_UART_Transmit(&UartHandle, buffer, length, 4000);
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
