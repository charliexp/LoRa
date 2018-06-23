/* Includes ------------------------------------------------------------------*/
#include "hw.h"

/* Private typedef -----------------------------------------------------------*/
/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
/* Private function prototypes -----------------------------------------------*/
/* Functions Definition ------------------------------------------------------*/
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
  PC_USARTX_CLK_ENABLE(); 
  METER_USARTX_CLK_ENABLE(); 
  
  GPIO_InitTypeDef  GPIO_InitStruct={0};
	
  METER_USARTX_TX_GPIO_CLK_ENABLE();
  METER_USARTX_RX_GPIO_CLK_ENABLE();
  PC_USARTX_TX_GPIO_CLK_ENABLE();
  PC_USARTX_RX_GPIO_CLK_ENABLE();
	
  GPIO_InitStruct.Mode = GPIO_MODE_AF_PP;
  GPIO_InitStruct.Pull = GPIO_PULLUP;
  GPIO_InitStruct.Speed = GPIO_SPEED_HIGH;
  GPIO_InitStruct.Alternate = METER_USARTX_TX_AF;
  GPIO_InitStruct.Pin = METER_USARTX_TX_PIN;
  HAL_GPIO_Init(METER_USARTX_TX_GPIO_PORT, &GPIO_InitStruct);
  GPIO_InitStruct.Alternate = PC_USARTX_TX_AF;
  GPIO_InitStruct.Pin = PC_USARTX_TX_PIN;
  HAL_GPIO_Init(PC_USARTX_TX_GPIO_PORT, &GPIO_InitStruct);

  GPIO_InitStruct.Alternate = METER_USARTX_RX_AF;
  GPIO_InitStruct.Pin = METER_USARTX_RX_PIN;
  HAL_GPIO_Init(METER_USARTX_RX_GPIO_PORT, &GPIO_InitStruct);
  GPIO_InitStruct.Alternate = PC_USARTX_RX_AF;
  GPIO_InitStruct.Pin = PC_USARTX_RX_PIN;
  HAL_GPIO_Init(PC_USARTX_RX_GPIO_PORT, &GPIO_InitStruct);
}

/**
  * @brief UART MSP DeInit
  * @param huart: uart handle
  * @retval None
  */
void HAL_UART_MspDeInit(UART_HandleTypeDef *huart)
{ 
  GPIO_InitTypeDef GPIO_InitStructure={0};
  
  METER_USARTX_RX_GPIO_CLK_ENABLE();
  METER_USARTX_TX_GPIO_CLK_ENABLE();
  PC_USARTX_RX_GPIO_CLK_ENABLE();
  PC_USARTX_TX_GPIO_CLK_ENABLE();

  GPIO_InitStructure.Mode = GPIO_MODE_ANALOG;
  GPIO_InitStructure.Pull = GPIO_NOPULL;
  
  GPIO_InitStructure.Pin = METER_USARTX_RX_PIN;
  HAL_GPIO_Init(METER_USARTX_RX_GPIO_PORT, &GPIO_InitStructure);
  GPIO_InitStructure.Pin = METER_USARTX_TX_PIN;
  HAL_GPIO_Init(METER_USARTX_TX_GPIO_PORT, &GPIO_InitStructure);
  GPIO_InitStructure.Pin = PC_USARTX_RX_PIN;
  HAL_GPIO_Init(PC_USARTX_RX_GPIO_PORT, &GPIO_InitStructure);
  GPIO_InitStructure.Pin = PC_USARTX_TX_PIN;
  HAL_GPIO_Init(PC_USARTX_TX_GPIO_PORT, &GPIO_InitStructure);
}
