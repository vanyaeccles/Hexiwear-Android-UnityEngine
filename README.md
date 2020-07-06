# Welcome!

This is a project I was working with Patrick King on back in 2018. I won't be working on or updating it much - but if you are using any part of it for a project and would like help getting it to work please get in touch!


Code mostly by me but I did use work from the [UnityDesktopToAndroidBluetooth](https://github.com/Freefly18/UnityDesktopToAndroidBluetooth) by Alexandre Millette. The Hexiwear code is from the [Hexiwear](https://github.com/MikroElektronika/HEXIWEAR) repository. Please get in touch if I have neglected to acknowledge work by anyone or if I am breaking any license.

_Vanya Eccles, 5/7/2020_


___
## What was the purpose of the project?

The core idea was to allow play with the Unity game engine through the movement of one's body. The player would wear multiple sensors, connect everything up via Bluetooth and play a Unity game through movement.

We used Hexiwears as the wearables, which are a cool hackable IoT/Wearable device with multiple sensors. It runs on a version of FreeRTOS and is a great way to learn about real-time operating systems and hardware-software interactions.


___
## Where was the project left off?

We left it in a state where it was a little bit fiddly setting up the connections and we weren't able to get the latency between the wearable and the game engine down to a suitable level \*. Still I believe it could be used as a jumping off point for projects using Wearables/Bluetooth/Android/Windows/Unity.


\* *Probably due to BLE not being ideal for high frequency communication. I thought about adapting the firmware for the KW40Z Bluetooth MCU so it would allow Bluetooth Classic but I never got the required software/JTAG debugger. I'm unclear whether the KW40Z could support this.*

___
## Details:

This is a system to create a kind of Bluetooth network between multiple [Hexiwear](https://www.hexiwear.com/) devices, an Android phone and a Windows PC.

- Uses an Android phone to read sensor data from one or more [Hexiwear](https://www.hexiwear.com/) devices via bluetooth low energy (BLE).

- The data can then be streamed (via a seperate Bluetooth Classic connection) from an Android phone to a PC running the Unity game engine.
___
## Major components:

**Android/android-BluetoothChat-master/**

 Android app built in Android Studio, used here primarily as a node that connects the wearables to a Windows PC.

- Connects to Hexiwears via BLE connection.
- Allows reading of sensor data (via a GATT service that presents Hexiwear IMU data such as from the accelerometer or gyroscope).
- Connects to a Bluetooth Classic (BC) connection
- Streams hexiwear data to the BC connection.


**Unity/HexiwearBTGame/**

Unity project that opens the BluetoothToTCP program and receives bluetooth data from Windows. Has a simple game that uses IMU data from the Hexiwears as player input.

- Opens the TCP server program (UnityDesktopToAndroidBluetooth - see directly below)
- Opens a UDP socket in Unity that reads incoming data from the TCP server (TODO - need to confirm these details, its been a while..)
- Data read from the UDP socket is used by the simple Unity game.


**Unity/UnityDesktopToAndroidBluetooth-master/PC/BluetoothToTCP/**

Software from the [UnityDesktopToAndroidBluetooth](https://github.com/Freefly18/UnityDesktopToAndroidBluetooth) repository. At the time Unity couldn't read Bluetooth data from Windows so this was a kind of workaround. Recieves Bluetooth data from the windows OS and sends it to Unity via a TCP server.

- Reads Bluetooth from the Windows OS
- Opens a TCP server on the host that can be connected to by Unity.



**hexiwear/**

This contains the Kinetis Design Studio project and the open-source Hexiwear firmware.

- Code pertaining to the hexiwear MCU was adapted to allow for higher frequency transmission of IMU sensor data along with other small changes. My changes to the code are commented with '//vanya'
- Has some other hexiwear stuff like datasheets.
