/*
 * Copyright (C) 2014 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

package com.example.android.bluetoothchat;

/**
 * Defines several constants used between {@link BluetoothChatService} and the UI.
 */
public interface Constants {

    // Message types sent from the BluetoothChatService Handler
    public static final int MESSAGE_STATE_CHANGE = 1;
    public static final int MESSAGE_READ = 2;
    public static final int MESSAGE_WRITE = 3;
    public static final int MESSAGE_DEVICE_NAME = 4;
    public static final int MESSAGE_TOAST = 5;

    // Key names received from the BluetoothChatService Handler
    public static final String DEVICE_NAME = "device_name";
    public static final String TOAST = "toast";


    // Hardcoded hexiwear MAC address (find from the hexiwear)
    public static String HEXIWEAR_ADDRESS_0 = "00:39:40:0C:00:4D";
    public static String HEXIWEAR_ADDRESS_1 = "00:22:40:08:00:25";
    public static String HEXIWEAR_ADDRESS_2 = "00:26:40:08:00:4E";

    //Hardcoded ids for hexiwears
    public static int HEXIWEAR_ID_0 = 0;
    public static int HEXIWEAR_ID_1 = 1;
    public static int HEXIWEAR_ID_2 = 2;

    public static int MOTION_SERVICE_UUID_INT = 0x2000;
    public static int ACC_CHAR_UUID_INT = 0x2001;
    public static int GYRO_CHAR_UUID_INT = 0x2002;

    public static byte[] streamStartArray = new byte[]{0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, (byte)0xFE};
    public static byte[] streamEndArray = new byte[]{0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, (byte)0xFF};
}
