################################################################################
# Automatically-generated file. Do not edit!
################################################################################

-include ../makefile.init

RM := rm -rf

# All of the sources participating in the build are defined here
-include sources.mk
-include utilities/subdir.mk
-include startup/subdir.mk
-include sources/subdir.mk
-include error/src/subdir.mk
-include drivers/intern_flash/src/subdir.mk
-include drivers/SPI/src/subdir.mk
-include boot/src/subdir.mk
-include board/subdir.mk
-include subdir.mk
-include objects.mk

ifneq ($(MAKECMDGOALS),clean)
ifneq ($(strip $(C_DEPS)),)
-include $(C_DEPS)
endif
ifneq ($(strip $(ASM_DEPS)),)
-include $(ASM_DEPS)
endif
ifneq ($(strip $(S_UPPER_DEPS)),)
-include $(S_UPPER_DEPS)
endif
endif

-include ../makefile.defs

# Add inputs and outputs from these tool invocations to the build variables 
SECONDARY_FLASH += \
HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.hex \

SECONDARY_SIZE += \
HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.siz \


# All Target
all: HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.elf secondary-outputs

# Tool invocations
HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.elf: $(OBJS) $(USER_OBJS)
	@echo 'Building target: $@'
	@echo 'Invoking: Cross ARM C Linker'
	arm-none-eabi-gcc -mcpu=cortex-m4 -mthumb -mfloat-abi=hard -mfpu=fpv4-sp-d16 -Os -fmessage-length=0 -fsigned-char -ffunction-sections -fdata-sections -Wall  -g -T "C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/Linker_Files/MK64FN1M0xxx12_flash.ld" -Xlinker --gc-sections -Wl,-Map,"HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.map" -Xlinker -z  -Xlinker muldefs   --specs=nano.specs          -Wall  -fno-common  -ffunction-sections  -fdata-sections  -ffreestanding  -fno-builtin  -Os  -mapcs  -Xlinker -static -o "HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.elf" -Xlinker --start-group $(OBJS) $(USER_OBJS) $(LIBS) -Xlinker --end-group
	@echo 'Finished building target: $@'
	@echo ' '

HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.hex: HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.elf
	@echo 'Invoking: Cross ARM GNU Create Flash Image'
	arm-none-eabi-objcopy -O ihex "HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.elf"  "HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.hex"
	@echo 'Finished building: $@'
	@echo ' '

HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.siz: HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.elf
	@echo 'Invoking: Cross ARM GNU Print Size'
	arm-none-eabi-size --format=berkeley "HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.elf"
	@echo 'Finished building: $@'
	@echo ' '

# Other Targets
clean:
	-$(RM) $(SECONDARY_SIZE)$(OBJS)$(C_DEPS)$(ASM_DEPS)$(SECONDARY_FLASH)$(S_UPPER_DEPS) HEXIWEAR_Bootloader_0_MK64FN1M0VLL12.elf
	-@echo ' '

secondary-outputs: $(SECONDARY_FLASH) $(SECONDARY_SIZE)

.PHONY: all clean dependents
.SECONDARY:

-include ../makefile.targets
