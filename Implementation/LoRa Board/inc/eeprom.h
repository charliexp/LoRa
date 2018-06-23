/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __EEPROM_H__
#define __EEPROM_H__

#ifdef __cplusplus
 extern "C" {
#endif
   
/* Includes ------------------------------------------------------------------*/
#include <stdint.h>

/* Exported defines ----------------------------------------------------------*/
/* Exported types ------------------------------------------------------------*/
/* Exported constants --------------------------------------------------------*/
/* External variables --------------------------------------------------------*/
/* Exported functions --------------------------------------------------------*/
void EEPROM_EraseWord(uint32_t address);
uint8_t EEPROM_ReadByte(uint32_t address);
uint16_t EEPROM_ReadHalfWord(uint32_t address);
uint32_t EEPROM_ReadWord(uint32_t address);
void EEPROM_WriteByte(uint32_t address, uint8_t value);
void EEPROM_WriteHalfWord(uint32_t address, uint16_t value);
void EEPROM_WriteWord(uint32_t address, uint32_t value);

#ifdef __cplusplus
}
#endif

#endif /* __EEPROM_H__*/
