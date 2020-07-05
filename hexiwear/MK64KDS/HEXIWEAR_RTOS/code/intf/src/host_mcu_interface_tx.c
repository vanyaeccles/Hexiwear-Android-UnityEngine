/**
 * \file host_mcu_interface_tx.c
 * \version 1.00
 * \brief this file contains MCU-to-MCU TX interface functionality
 *
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this list
 *   of conditions and the following disclaimer.
 *
 * Redistributions in binary form must reproduce the above copyright notice, this
 *   list of conditions and the following disclaimer in the documentation and/or
 *   other materials provided with the distribution.
 *
 * Neither the name of NXP, nor the names of its
 *   contributors may be used to endorse or promote products derived from this
 *   software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * visit: http://www.mikroe.com and http://www.nxp.com
 *
 * get support at: http://www.mikroe.com/forum and https://community.nxp.com
 *
 * Project HEXIWEAR, 2015
 */

/** includes */
#include <stdint.h>
#include <stddef.h>
#include "board.h"
#include "fsl_uart_driver.h"
#include "fsl_interrupt_manager.h"

#include "HEXIWEAR_info.h"

#include "host_mcu_interface.h"
#include "host_mcu_interface_defs.h"

#if defined( HEXIWEAR_DEBUG )
#include "DEBUG_UART.h"
#include "flashlight.h"
#endif

#include "error.h"
#include "error_types.h"

/** private memory declarations */
static hostInterface_packet_t
  hostInterface_okPacket =  {
                              .start1 = 0x55,
                              .start2 = 0xAA,
                              .type   = packetType_OK,
                              .length = 0,
                              .data[0] = gHostInterface_trailerByte
                            };

/** private prototypes */
static void         HostInterface_FlushPacket(hostInterface_packet_t * pHostInterface_packet);
static void         HostInterface_OkTask(task_param_t param);

/** functions definitions */

/**
 * create RTOS structures for sending data via communication interface
 * @return status flag
 */
osa_status_t HostInterface_TxInit()
{
  osa_status_t
    status = OSA_TaskCreate (
                            HostInterface_OkTask,
                            (uint8_t*)"HostInterface_OkTask",
                            gHostInterfaceOkTaskStackSize_c,
                            NULL,
                            gHostInterfaceOkPriority_c,
                            (task_param_t)NULL,
                            false,
                            &hexiwear_intf_OK_handler
                          );

  if ( kStatus_OSA_Success != status )
  {
    catch( CATCH_TASK ) ;
  }

  return (osa_status_t)status;
}

/**
 * send OK packet
 * @param param optional parameter
 */
static void HostInterface_OkTask(task_param_t param)
{
  while (1)
  {
    osa_status_t
      status = HostInterface_EventSendOkPacketWait();

    if ( kStatus_OSA_Success == status )
    {
      HostInterface_FlushPacket(&hostInterface_okPacket );
    }
  }
}

int test = 0;

/**
 * send data packet
 * @param pHostInterface_packet packet to be sent
 */
static void HostInterface_FlushPacket(hostInterface_packet_t * pHostInterface_packet)
{
	test = !test; //toggle test

#if defined( SEND_PACKETS_VIA_UART_INT )
    UART_DRV_SendDataBlocking( gHostInterface_instance, (uint8_t*)pHostInterface_packet, pHostInterface_packet->length + gHostInterface_headerSize + 1, UART_TIMEOUT );
#elif defined( SEND_PACKETS_VIA_UART_DMA )
    UART_DRV_EdmaSendDataBlocking( gHostInterface_instance, (uint8_t*)pHostInterface_packet, pHostInterface_packet->length + gHostInterface_headerSize + 1, UART_TIMEOUT );
#endif

#if defined( HEXIWEAR_DEBUG )
//    UART_DRV_SendDataBlocking( HEXIWEAR_DEBUG_UART_INSTANCE, (uint8_t*)pHostInterface_packet, pHostInterface_packet->length + gHostInterface_headerSize + 1, UART_TIMEOUT );
#endif

}
