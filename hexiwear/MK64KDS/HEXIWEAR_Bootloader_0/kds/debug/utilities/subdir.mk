################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
C_SRCS += \
c:/Freescale/KSDK_1.2.0/platform/utilities/src/fsl_debug_console.c \
c:/Freescale/KSDK_1.2.0/platform/utilities/src/print_scan.c 

OBJS += \
./utilities/fsl_debug_console.o \
./utilities/print_scan.o 

C_DEPS += \
./utilities/fsl_debug_console.d \
./utilities/print_scan.d 


# Each subdirectory must supply rules for building sources it contributes
utilities/fsl_debug_console.o: c:/Freescale/KSDK_1.2.0/platform/utilities/src/fsl_debug_console.c
	@echo 'Building file: $<'
	@echo 'Invoking: Cross ARM C Compiler'
	arm-none-eabi-gcc -mcpu=cortex-m4 -mthumb -mfloat-abi=hard -mfpu=fpv4-sp-d16 -Os -fmessage-length=0 -fsigned-char -ffunction-sections -fdata-sections -Wall  -g -DDEBUG -DCPU_MK64FN1M0VLL12 -DFRDM_K64F -DFREEDOM -DMCU_MK64FN1M -I../../ -I../../board -I"c:\Freescale\KSDK_1.2.0/platform/osa/inc" -I"c:\Freescale\KSDK_1.2.0/platform/utilities/inc" -I"c:\Freescale\KSDK_1.2.0/platform/CMSIS/Include" -I"c:\Freescale\KSDK_1.2.0/platform/devices" -I"c:\Freescale\KSDK_1.2.0/platform/devices/MK64F12/include" -I"c:\Freescale\KSDK_1.2.0/platform/devices/MK64F12/startup" -I"c:\Freescale\KSDK_1.2.0/platform/hal/inc" -I"c:\Freescale\KSDK_1.2.0/platform/drivers/inc" -I"c:\Freescale\KSDK_1.2.0/platform/system/inc" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/cfg" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/include" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/error/inc" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/drivers/intern_flash/inc" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/drivers/SPI/inc" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/boot/inc" -std=gnu99 -fno-common  -ffreestanding  -fno-builtin  -mapcs -MMD -MP -MF"$(@:%.o=%.d)" -MT"$@" -c -o "$@" "$<"
	@echo 'Finished building: $<'
	@echo ' '

utilities/print_scan.o: c:/Freescale/KSDK_1.2.0/platform/utilities/src/print_scan.c
	@echo 'Building file: $<'
	@echo 'Invoking: Cross ARM C Compiler'
	arm-none-eabi-gcc -mcpu=cortex-m4 -mthumb -mfloat-abi=hard -mfpu=fpv4-sp-d16 -Os -fmessage-length=0 -fsigned-char -ffunction-sections -fdata-sections -Wall  -g -DDEBUG -DCPU_MK64FN1M0VLL12 -DFRDM_K64F -DFREEDOM -DMCU_MK64FN1M -I../../ -I../../board -I"c:\Freescale\KSDK_1.2.0/platform/osa/inc" -I"c:\Freescale\KSDK_1.2.0/platform/utilities/inc" -I"c:\Freescale\KSDK_1.2.0/platform/CMSIS/Include" -I"c:\Freescale\KSDK_1.2.0/platform/devices" -I"c:\Freescale\KSDK_1.2.0/platform/devices/MK64F12/include" -I"c:\Freescale\KSDK_1.2.0/platform/devices/MK64F12/startup" -I"c:\Freescale\KSDK_1.2.0/platform/hal/inc" -I"c:\Freescale\KSDK_1.2.0/platform/drivers/inc" -I"c:\Freescale\KSDK_1.2.0/platform/system/inc" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/cfg" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/include" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/error/inc" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/drivers/intern_flash/inc" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/drivers/SPI/inc" -I"C:/Fitreo/hexiwear/HEXIWEAR-master/SW/MK64KDS/HEXIWEAR_Bootloader_0/kds/boot/inc" -std=gnu99 -fno-common  -ffreestanding  -fno-builtin  -mapcs -MMD -MP -MF"$(@:%.o=%.d)" -MT"$@" -c -o "$@" "$<"
	@echo 'Finished building: $<'
	@echo ' '


