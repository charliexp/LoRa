/* Includes ------------------------------------------------------------------*/
#include "eeprom.h"
#include "hw.h"

/* Private typedef -----------------------------------------------------------*/
/* Private define ------------------------------------------------------------*/
/* Private macro -------------------------------------------------------------*/
/* Private constants ---------------------------------------------------------*/
/* Private variables ---------------------------------------------------------*/
/* Private function prototypes -----------------------------------------------*/
/* Public variables ----------------------------------------------------------*/
/* Functions Definition ------------------------------------------------------*/
void EEPROM_EraseWord(uint32_t address)
{
		HAL_FLASHEx_DATAEEPROM_Unlock();
		HAL_FLASHEx_DATAEEPROM_Erase(address);
		HAL_FLASHEx_DATAEEPROM_Lock();
}

uint8_t EEPROM_ReadByte(uint32_t address)
{
	return *((uint8_t *) address);
}

uint16_t EEPROM_ReadHalfWord(uint32_t address)
{
	return *((uint16_t *) address);
}

uint32_t EEPROM_ReadWord(uint32_t address)
{
	return *((uint32_t *) address);
}

void EEPROM_WriteByte(uint32_t address, uint8_t value)
{
	if (*((uint8_t *) address) != value)
	{
		HAL_FLASHEx_DATAEEPROM_Unlock();
		HAL_FLASHEx_DATAEEPROM_Program(FLASH_TYPEPROGRAMDATA_BYTE, address, value);
		HAL_FLASHEx_DATAEEPROM_Lock();
	}
}

void EEPROM_WriteHalfWord(uint32_t address, uint16_t value)
{
	if (*((uint16_t *) address) != value)
	{
		HAL_FLASHEx_DATAEEPROM_Unlock();
		HAL_FLASHEx_DATAEEPROM_Program(FLASH_TYPEPROGRAMDATA_HALFWORD, address, value);
		HAL_FLASHEx_DATAEEPROM_Lock();
	}
}

void EEPROM_WriteWord(uint32_t address, uint32_t value)
{
	if (*((uint32_t *) address) != value)
	{
		HAL_FLASHEx_DATAEEPROM_Unlock();
		HAL_FLASHEx_DATAEEPROM_Program(FLASH_TYPEPROGRAMDATA_WORD, address, value);
		HAL_FLASHEx_DATAEEPROM_Lock();
	}
}
