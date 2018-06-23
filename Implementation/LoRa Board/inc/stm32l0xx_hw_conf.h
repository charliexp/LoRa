/*
 / _____)             _              | |
( (____  _____ ____ _| |_ _____  ____| |__
 \____ \| ___ |    (_   _) ___ |/ ___)  _ \
 _____) ) ____| | | || |_| ____( (___| | | |
(______/|_____)_|_|_| \__)_____)\____)_| |_|
    (C)2013 Semtech

Description: contains hardaware configuration Macros and Constants

License: Revised BSD License, see LICENSE.TXT file include in the project

Maintainer: Miguel Luis and Gregory Cristian
*/
 /******************************************************************************
  * @file    stm32l0xx_hw_conf.h
  * @author  MCD Application Team
  * @version V1.1.2
  * @date    08-September-2017
  * @brief   contains hardaware configuration Macros and Constants
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; Copyright (c) 2017 STMicroelectronics International N.V. 
  * All rights reserved.</center></h2>
  *
  * Redistribution and use in source and binary forms, with or without 
  * modification, are permitted, provided that the following conditions are met:
  *
  * 1. Redistribution of source code must retain the above copyright notice, 
  *    this list of conditions and the following disclaimer.
  * 2. Redistributions in binary form must reproduce the above copyright notice,
  *    this list of conditions and the following disclaimer in the documentation
  *    and/or other materials provided with the distribution.
  * 3. Neither the name of STMicroelectronics nor the names of other 
  *    contributors to this software may be used to endorse or promote products 
  *    derived from this software without specific written permission.
  * 4. This software, including modifications and/or derivative works of this 
  *    software, must execute solely and exclusively on microcontroller or
  *    microprocessor devices manufactured by or for STMicroelectronics.
  * 5. Redistribution and use of this software other than as permitted under 
  *    this license is void and will automatically terminate your rights under 
  *    this license. 
  *
  * THIS SOFTWARE IS PROVIDED BY STMICROELECTRONICS AND CONTRIBUTORS "AS IS" 
  * AND ANY EXPRESS, IMPLIED OR STATUTORY WARRANTIES, INCLUDING, BUT NOT 
  * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
  * PARTICULAR PURPOSE AND NON-INFRINGEMENT OF THIRD PARTY INTELLECTUAL PROPERTY
  * RIGHTS ARE DISCLAIMED TO THE FULLEST EXTENT PERMITTED BY LAW. IN NO EVENT 
  * SHALL STMICROELECTRONICS OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
  * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
  * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
  * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
  * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
  * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
  * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  *
  ******************************************************************************
  */

/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __HW_CONF_L0_H__
#define __HW_CONF_L0_H__

#ifdef __cplusplus
 extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
/* Exported types ------------------------------------------------------------*/
/* Exported defines ----------------------------------------------------------*/
/* LORA definition */

//#define RADIO_DIO_4
//#define RADIO_DIO_5

#define RADIO_RESET_PORT                        GPIOA
#define RADIO_RESET_PIN                         GPIO_PIN_0

#define RADIO_MOSI_PORT                         GPIOA
#define RADIO_MOSI_PIN                          GPIO_PIN_7

#define RADIO_MISO_PORT                         GPIOA
#define RADIO_MISO_PIN                          GPIO_PIN_6

#define RADIO_SCLK_PORT                         GPIOA
#define RADIO_SCLK_PIN                          GPIO_PIN_5

#define RADIO_NSS_PORT                          GPIOB
#define RADIO_NSS_PIN                           GPIO_PIN_6

#define RADIO_DIO_0_PORT                        GPIOA
#define RADIO_DIO_0_PIN                         GPIO_PIN_10

#define RADIO_DIO_1_PORT                        GPIOB
#define RADIO_DIO_1_PIN                         GPIO_PIN_3

#define RADIO_DIO_2_PORT                        GPIOB
#define RADIO_DIO_2_PIN                         GPIO_PIN_5

#define RADIO_DIO_3_PORT                        GPIOB
#define RADIO_DIO_3_PIN                         GPIO_PIN_4

#ifdef RADIO_DIO_4 
#define RADIO_DIO_4_PORT                        GPIOA
#define RADIO_DIO_4_PIN                         GPIO_PIN_9
#endif

#ifdef RADIO_DIO_5 
#define RADIO_DIO_5_PORT                        GPIOC
#define RADIO_DIO_5_PIN                         GPIO_PIN_7
#endif

#define RADIO_ANT_SWITCH_PORT                   GPIOC
#define RADIO_ANT_SWITCH_PIN                    GPIO_PIN_1

#define SPI_CLK_ENABLE()												__HAL_RCC_SPI1_CLK_ENABLE()
#define SPI1_AF																	GPIO_AF0_SPI1  

/* ADC definition */
#define BAT_LEVEL_PORT                          GPIOA
#define BAT_LEVEL_PIN                           GPIO_PIN_4

#ifdef USE_STM32L0XX_NUCLEO
#define ADC_READ_CHANNEL												ADC_CHANNEL_4
#define ADCCLK_ENABLE()													__HAL_RCC_ADC1_CLK_ENABLE() ;
#define ADCCLK_DISABLE()												__HAL_RCC_ADC1_CLK_DISABLE() ;
#endif

/* RTC definition */
#define RTC_OUTPUT															DBG_RTC_OUTPUT
#define RTC_Alarm_IRQn													RTC_IRQn

/* Debug USART definition */
#define PC_USARTX															USART2
#define PC_USARTX_CLK_ENABLE()									__USART2_CLK_ENABLE()
#define PC_USARTX_RX_GPIO_CLK_ENABLE()					__GPIOA_CLK_ENABLE()
#define PC_USARTX_TX_GPIO_CLK_ENABLE()					__GPIOA_CLK_ENABLE() 

#define PC_USARTX_FORCE_RESET()								__USART2_FORCE_RESET()
#define PC_USARTX_RELEASE_RESET()							__USART2_RELEASE_RESET()

#define PC_USARTX_TX_PIN												GPIO_PIN_2
#define PC_USARTX_TX_GPIO_PORT									GPIOA  
#define PC_USARTX_TX_AF												GPIO_AF4_USART2
#define PC_USARTX_RX_PIN												GPIO_PIN_3
#define PC_USARTX_RX_GPIO_PORT									GPIOA
#define PC_USARTX_RX_AF												GPIO_AF4_USART2

#define PC_USARTX_IRQn													USART2_IRQn
#define PC_USARTX_IRQHandler										USART2_IRQHandler

/* Data acquisition USART definition */
#define METER_USARTX															USART4
#define METER_USARTX_CLK_ENABLE()									__HAL_RCC_USART4_CLK_ENABLE()
#define METER_USARTX_RX_GPIO_CLK_ENABLE()					__GPIOC_CLK_ENABLE()
#define METER_USARTX_TX_GPIO_CLK_ENABLE()					__GPIOC_CLK_ENABLE() 

#define METER_USARTX_FORCE_RESET()								__USART4_FORCE_RESET()
#define METER_USARTX_RELEASE_RESET()							__USART4_RELEASE_RESET()

#define METER_USARTX_TX_PIN												GPIO_PIN_10
#define METER_USARTX_TX_GPIO_PORT									GPIOC  
#define METER_USARTX_TX_AF												GPIO_AF6_USART4
#define METER_USARTX_RX_PIN												GPIO_PIN_11
#define METER_USARTX_RX_GPIO_PORT									GPIOC
#define METER_USARTX_RX_AF												GPIO_AF6_USART4

#define METER_USARTX_IRQn                      		USART4_5_IRQn
#define METER_USARTX_IRQHandler                		USART4_5_IRQHandler

/* LED definition */
#define LED_Toggle( x )
#define LED_On( x )
#define LED_Off( x )

/* Compensator I2C definition */
#define COMP_I2C																I2C1
#define COMP_I2C_CLK_ENABLE()										__HAL_RCC_I2C1_CLK_ENABLE()
#define COMP_I2C_SDA_GPIO_CLK_ENABLE()					__GPIOB_CLK_ENABLE()
#define COMP_I2C_SCL_GPIO_CLK_ENABLE()					__GPIOB_CLK_ENABLE()

#define COMP_I2C_SCL_PIN												GPIO_PIN_8
#define COMP_I2C_SCL_GPIO_PORT									GPIOB 
#define COMP_I2C_SCL_AF													GPIO_AF4_I2C1
#define COMP_I2C_SDA_PIN												GPIO_PIN_9
#define COMP_I2C_SDA_GPIO_PORT									GPIOB
#define COMP_I2C_SDA_AF													GPIO_AF4_I2C1


#ifdef __cplusplus
}
#endif

#endif /* __HW_CONF_L0_H__ */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
