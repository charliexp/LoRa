/* Includes ------------------------------------------------------------------*/
#include "hw.h"

/* Private typedef -----------------------------------------------------------*/
/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
/* I2C Handle */
/* Private function prototypes -----------------------------------------------*/
/* Functions Definition ------------------------------------------------------*/
/**
  * @brief I2C MSP Initialization 
  *        This function configures the hardware resources used in this example: 
  *           - Peripheral's clock enable
  *           - Peripheral's GPIO Configuration  
  *           - NVIC configuration for I2C interrupt request enable
  * @param hi2c: I2C handle
  * @retval None
  */
void HAL_I2C_MspInit(I2C_HandleTypeDef *hi2c)
{
  
  /*##-1- Enable peripherals and GPIO Clocks #################################*/

  /* Enable I2C clock */
  COMP_I2C_CLK_ENABLE(); 
	
  /*##-2- Configure peripheral GPIO ##########################################*/  
  GPIO_InitTypeDef  GPIO_InitStruct={0};
    /* Enable GPIO SCL/SDA clock */
  COMP_I2C_SCL_GPIO_CLK_ENABLE();
  COMP_I2C_SDA_GPIO_CLK_ENABLE();
    /* GPIO SCL pin configuration  */
  GPIO_InitStruct.Pin       = COMP_I2C_SCL_PIN;
  GPIO_InitStruct.Mode      = GPIO_MODE_AF_OD;
  GPIO_InitStruct.Pull      = GPIO_NOPULL;
  GPIO_InitStruct.Speed     = GPIO_SPEED_HIGH;
  GPIO_InitStruct.Alternate = COMP_I2C_SCL_AF;

  HAL_GPIO_Init(COMP_I2C_SCL_GPIO_PORT, &GPIO_InitStruct);

		/* GPIO SDA pin configuration  */
  GPIO_InitStruct.Pin = COMP_I2C_SDA_PIN;
  GPIO_InitStruct.Alternate = COMP_I2C_SDA_AF;

  HAL_GPIO_Init(COMP_I2C_SDA_GPIO_PORT, &GPIO_InitStruct);
}

/**
  * @brief I2C MSP DeInit
  * @param hi2c: I2C handle
  * @retval None
  */
void HAL_I2C_MspDeInit(I2C_HandleTypeDef *hi2c)
{
  GPIO_InitTypeDef GPIO_InitStructure={0};
  
  COMP_I2C_SCL_GPIO_CLK_ENABLE();
  COMP_I2C_SDA_GPIO_CLK_ENABLE();

  GPIO_InitStructure.Mode = GPIO_MODE_ANALOG;
  GPIO_InitStructure.Pull = GPIO_NOPULL;
  
  GPIO_InitStructure.Pin =  COMP_I2C_SCL_PIN ;
  HAL_GPIO_Init(COMP_I2C_SCL_GPIO_PORT, &GPIO_InitStructure);
  
  GPIO_InitStructure.Pin =  COMP_I2C_SDA_PIN ;
  HAL_GPIO_Init(COMP_I2C_SDA_GPIO_PORT, &GPIO_InitStructure); 
}
