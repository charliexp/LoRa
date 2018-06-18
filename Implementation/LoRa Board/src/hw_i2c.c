/* Includes ------------------------------------------------------------------*/
#include "hw_i2c.h"

/* Private typedef -----------------------------------------------------------*/
/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
/* I2C Handle */
I2C_HandleTypeDef  hi2c;
/* Private function prototypes -----------------------------------------------*/
/* Functions Definition ------------------------------------------------------*/
void I2C_Init(void)
{
	hi2c.Instance		= ACT_I2C;
	hi2c.Init.AddressingMode = I2C_ADDRESSINGMODE_7BIT;
	hi2c.Init.DualAddressMode = I2C_DUALADDRESS_DISABLE;
	hi2c.Init.GeneralCallMode = I2C_GENERALCALL_DISABLE;
	hi2c.Init.NoStretchMode = I2C_NOSTRETCH_DISABLE;
	hi2c.Init.Timing = 0x10420F13;
	HAL_I2C_Init(&hi2c);
}

void I2C_DeInit(void)
{
  HAL_I2C_DeInit(&hi2c);
}

uint8_t I2C_Read(uint8_t deviceAddress, uint8_t registerAddress)
{
	uint8_t data;
	
	HAL_I2C_Mem_Read(&hi2c, deviceAddress, registerAddress, I2C_MEMADD_SIZE_8BIT, &data, 1, 1000);
	
	return data;
}	

void I2C_Write(uint8_t deviceAddress, uint8_t registerAddress, uint8_t value)
{
	HAL_I2C_Mem_Write(&hi2c, deviceAddress, registerAddress, I2C_MEMADD_SIZE_8BIT, &value, 1, 1000);
}

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
  ACT_I2C_CLK_ENABLE(); 
	
  /*##-2- Configure peripheral GPIO ##########################################*/  
  GPIO_InitTypeDef  GPIO_InitStruct={0};
    /* Enable GPIO SCL/SDA clock */
  ACT_I2C_SCL_GPIO_CLK_ENABLE();
  ACT_I2C_SDA_GPIO_CLK_ENABLE();
    /* GPIO SCL pin configuration  */
  GPIO_InitStruct.Pin       = ACT_I2C_SCL_PIN;
  GPIO_InitStruct.Mode      = GPIO_MODE_AF_OD;
  GPIO_InitStruct.Pull      = GPIO_NOPULL;
  GPIO_InitStruct.Speed     = GPIO_SPEED_HIGH;
  GPIO_InitStruct.Alternate = ACT_I2C_SCL_AF;

  HAL_GPIO_Init(ACT_I2C_SCL_GPIO_PORT, &GPIO_InitStruct);

		/* GPIO SDA pin configuration  */
  GPIO_InitStruct.Pin = ACT_I2C_SDA_PIN;
  GPIO_InitStruct.Alternate = ACT_I2C_SDA_AF;

  HAL_GPIO_Init(ACT_I2C_SDA_GPIO_PORT, &GPIO_InitStruct);
}

/**
  * @brief I2C MSP DeInit
  * @param hi2c: I2C handle
  * @retval None
  */
void HAL_I2C_MspDeInit(I2C_HandleTypeDef *hi2c)
{
  GPIO_InitTypeDef GPIO_InitStructure={0};
  
  ACT_I2C_SCL_GPIO_CLK_ENABLE();
  ACT_I2C_SDA_GPIO_CLK_ENABLE();

  GPIO_InitStructure.Mode = GPIO_MODE_ANALOG;
  GPIO_InitStructure.Pull = GPIO_NOPULL;
  
  GPIO_InitStructure.Pin =  ACT_I2C_SCL_PIN ;
  HAL_GPIO_Init(  ACT_I2C_SCL_GPIO_PORT, &GPIO_InitStructure );
  
  GPIO_InitStructure.Pin =  ACT_I2C_SDA_PIN ;
  HAL_GPIO_Init(  ACT_I2C_SDA_GPIO_PORT, &GPIO_InitStructure ); 
}
