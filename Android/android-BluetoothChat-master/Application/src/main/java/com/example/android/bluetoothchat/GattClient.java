package com.example.android.bluetoothchat;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.BluetoothGattCallback;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattDescriptor;
import android.bluetooth.BluetoothGattService;
import android.bluetooth.BluetoothProfile;
import android.content.Context;
import android.util.Log;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.Arrays;
import java.util.UUID;

import static com.example.android.bluetoothchat.Constants.MOTION_SERVICE_UUID_INT;
import static com.example.android.bluetoothchat.Constants.ACC_CHAR_UUID_INT;
import static com.example.android.bluetoothchat.Constants.GYRO_CHAR_UUID_INT;

//Created by vanya 7/6/18
/**
 * This class represents a BLE GATT connection to a single hexiwear device
 */

public class GattClient {

    private String TAG = "GattClient";

    public int hexID;

    private Context mContext;
    private BluetoothDevice myDevice;
    private BluetoothAdapter mBluetoothAdapter;
    private BluetoothGatt mBluetoothGatt;
    private BluetoothGattService MotionTxService;
    private BluetoothGattCharacteristic accChar;
    private BluetoothGattCharacteristic gyroChar;
    public byte[] accelerometerDataByteArray;
    public byte[] gyroscopeDataByteArray;
    public byte[] dataByteArray;
    //ids for sent data arrays, first byte is the sensor type, second is the hex ID
    private byte[] accelerometerID = new byte[]{0x00, 0x00};
    private byte[] gyroscopeID = new byte[]{0x01, 0x00};

    private static UUID MOTION_SERVICE_UUID = convertFromInteger(MOTION_SERVICE_UUID_INT);
    private static UUID ACC_CHAR_UUID = convertFromInteger(ACC_CHAR_UUID_INT);
    private static UUID GYRO_CHAR_UUID = convertFromInteger(GYRO_CHAR_UUID_INT);

    private Boolean accRead = true;
    private Boolean enabled = true;
    public Boolean isGattConnected = false;
    public Boolean isBlocked = false;

    //for adding the id
    private ByteArrayOutputStream outputStream = new ByteArrayOutputStream();

   /*
    *    Constructor
    */
    public GattClient(int id){
        hexID = id;
        TAG += id;

        //put the hex id in the second byte of accelerometerID and gyroscopeID
        accelerometerID[1] = (byte)id;
        gyroscopeID[1] = (byte)id;
    }

    public BluetoothDevice getBTDevice(){
        return myDevice;
    }

    public void stop(){
        if(mBluetoothGatt == null)
            return;
        mBluetoothGatt.disconnect();
        mBluetoothGatt.close();
        mBluetoothGatt = null;
        Log.i(TAG, "Gatt is closed");
        isGattConnected = false;
    }

    public void disconnect(){
        if(mBluetoothGatt == null)
            return;
        mBluetoothGatt.disconnect();
    }


    public void connectGattClient(BluetoothDevice device, Context context){

        myDevice = device;
        mContext = context;
        mBluetoothGatt = myDevice.connectGatt(context, true, gattCallback, BluetoothDevice.TRANSPORT_LE);
        Log.i(TAG, "Connecting to gatt server");
    }

    public void reconnectGatt(){
        if(mBluetoothGatt == null)
            return;

        if(mBluetoothGatt.connect())
            Log.i(TAG, "Connecting to gatt server");
    }



    //region BLE SCANNING
    /*
    public void scanLeDevice(final boolean enable, BluetoothAdapter bluetoothAdapter, Context context) {
        if (enable) {
            mContext = context;
            mBluetoothAdapter = bluetoothAdapter;
            // Stops scanning after a pre-defined scan period.
            Handler mHandler = new Handler();
            mHandler.postDelayed(new Runnable() {
                @Override
                public void run() {
                    mBluetoothAdapter.stopLeScan(mLeScanCallback);
                    //updateProgressText("scanning for ble devices has timed out");
                    Log.i("info", "scanning for ble devices has timed out");
                }
            }, 10000);
            mBluetoothAdapter.startLeScan(mLeScanCallback);
            Log.i(TAG, "scanning for ble devices");
        } else {
            mBluetoothAdapter.stopLeScan(mLeScanCallback);
            //updateProgressText("can't scan for ble devices");
            Log.i(TAG, "can't scan for ble devices");
        }
    }

    // Device scan callback, the interface used to deliver BLE scan results
    private BluetoothAdapter.LeScanCallback mLeScanCallback = new BluetoothAdapter.LeScanCallback() {
        @Override
        public void onLeScan(final BluetoothDevice device, int rssi, byte[] scanRecord) {
            Log.i(TAG, "Scanning for target device with address: " + HEXIWEAR_ADDRESS);
            Log.i(TAG,  "Device found with address: " + device.getAddress());
            if(device.getAddress().equals(HEXIWEAR_ADDRESS)){
                myDevice = mBluetoothAdapter.getRemoteDevice(HEXIWEAR_ADDRESS);
                Log.i(TAG, "Target device found");
                mBluetoothAdapter.stopLeScan(mLeScanCallback);
                Log.i(TAG, "LE Scan Stopped");

                connectGattClient(device, mContext);
            }
        }
    };
    */
    //endregion


    //region GATT CALLBACK
    private BluetoothGattCallback gattCallback = new BluetoothGattCallback() {
        @Override
        public void onConnectionStateChange(BluetoothGatt gatt, int status, int newState) {

            Log.i(TAG, "onConnectionStateChange: status=>"+status+" newState=>"+newState);

            if (newState == BluetoothProfile.STATE_CONNECTED){
                //Toast.makeText(mContext, "Connected to GATT",Toast.LENGTH_SHORT).show();
                gatt.discoverServices();
                Log.i(TAG, "onConnectionStateChange, STATE_CONNECTED");
                mBluetoothGatt.discoverServices();
                isGattConnected = true;
            }
            else if (newState == BluetoothProfile.STATE_DISCONNECTED) {
                Log.i(TAG, "onConnectionStateChange, STATE_DISCONNECTED");
                isGattConnected = false;
                gatt.connect();
            }
        }
        @Override
        public void onServicesDiscovered(BluetoothGatt gatt, int status) {
            if (status == BluetoothGatt.GATT_SUCCESS) {
                Log.i(TAG, "GATT services discovered");
                if(accChar == null)
                    if(setupCharacteristic(accChar, ACC_CHAR_UUID))
                        Log.i(TAG, "accelerometer char discovered");
                if(gyroChar == null)
                    if(setupCharacteristic(gyroChar, GYRO_CHAR_UUID))
                        Log.i(TAG, "gyroscope char discovered");
            }
        }
        @Override
        public void onCharacteristicRead(BluetoothGatt gatt,BluetoothGattCharacteristic characteristic,int status) {
            if (status == BluetoothGatt.GATT_SUCCESS) {
                isBlocked = false;
                Log.i(TAG, "Characteristic " + characteristic.getUuid() + " was read!");
            }
        }
        @Override
        public void onDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, int status){
        }
        @Override
        public void onCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic) {
            //Log.i("info", "Characteristic " + characteristic.getUuid() + " changed " + characteristic.getValue());
        }

        @Override
        public void onCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
            super.onCharacteristicWrite(gatt, characteristic, status);
        }
    };
    //endregion


    public boolean setupCharacteristic(BluetoothGattCharacteristic characteristic, UUID charUUID){

        //check mBluetoothGatt is available
        if (mBluetoothGatt == null) {
            Log.i(TAG, "lost connection");
            return false;
        }


        MotionTxService = mBluetoothGatt.getService(MOTION_SERVICE_UUID);
        if (MotionTxService == null) {
            Log.i(TAG, "Service not found: " + MOTION_SERVICE_UUID);
            return false;
        }


        characteristic = MotionTxService.getCharacteristic(charUUID);
        if (characteristic == null) {
            Log.i(TAG, "Characteristic not found: " + charUUID);
            return false;
        }

        // To enable notifications on Android, you have to locally enable the notification for the particular characteristic
        mBluetoothGatt.setCharacteristicNotification(characteristic, enabled);
        Log.i(TAG, "Characteristic enabled: " + charUUID);

        //TODO unfortunately without reprogramming the hexiwear bluetooth stack we can't use the onCharacteristicChanged(), there is no CCCD for the motion chars
        //also have to enable notifications on the peer device by writing to the device’s client characteristic configuration descriptor (CCCD)
        /*
        BluetoothGattDescriptor descriptor = characteristic.getDescriptor(convertFromInteger(0x2902));
        if(descriptor == null){
            Log.i("info", "Descriptor not found: " + charUUID);
            return false;
        }
        descriptor.setValue(BluetoothGattDescriptor.ENABLE_INDICATION_VALUE);
        mBluetoothGatt.writeDescriptor(descriptor);
        */

        return true;
    }

    long timeSince = 0;
    long then = 0;

    public boolean readTXCharacteristic() {

            //check mBluetoothGatt is available
            if (mBluetoothGatt == null) {
                Log.i(TAG, "lost connection");
                return false;
            }

            //check the gatt service is available
            //MotionTxService = mBluetoothGatt.getService(MOTION_SERVICE_UUID);
            if (MotionTxService == null) {
                Log.i("info", "Service not found: " + MOTION_SERVICE_UUID);
                return false;
            }

            // alternate between reading accelerometer and gyroscope
            if(accRead){
                accChar = MotionTxService.getCharacteristic(ACC_CHAR_UUID);
                if (accChar == null) {
                    Log.i(TAG, "Characteristic not found: " + ACC_CHAR_UUID);
                    mBluetoothGatt.discoverServices();
                    return false;
                }
                mBluetoothGatt.readCharacteristic(accChar);
                dataByteArray = concatenateByteArrays(accChar.getValue(), accelerometerID);

                if((dataByteArray == null)){
                    Log.i(TAG, "Read from Accelerometer was null");
                    return false;
                }
                // check new readings to see if its a repeat
                //if((Arrays.equals(dataByteArray,accelerometerDataByteArray))){
                //    Log.i(TAG, "Read from Accelerometer was  equal to previous " + parseBluetoothAccData(dataByteArray));
                //    return false;
                //}
                else {
                    timeSince = System.currentTimeMillis() - then;
                    Log.i(TAG, "value read from service: " + MOTION_SERVICE_UUID + ", characteristic: " + ACC_CHAR_UUID
                            + " with data " + parseBluetoothAccData(accChar.getValue()));
                    Log.i(TAG, "time delay of good acc value: " + timeSince);
                    accelerometerDataByteArray = dataByteArray;
                    then = System.currentTimeMillis();
                }
            }
            else {
                gyroChar = MotionTxService.getCharacteristic(GYRO_CHAR_UUID);
                if (gyroChar == null) {
                    Log.i(TAG, "Characteristic not found: " + GYRO_CHAR_UUID);
                    setupCharacteristic(gyroChar, GYRO_CHAR_UUID);
                    return false;
                }
                mBluetoothGatt.readCharacteristic(gyroChar);
                dataByteArray = concatenateByteArrays(gyroChar.getValue(), gyroscopeID);

                if(dataByteArray == null){
                    Log.i(TAG, "Read from Gyroscope was null");
                    return false;
                }
                if(Arrays.equals(dataByteArray,gyroscopeDataByteArray)){
                    Log.i(TAG, "Read from Gyroscope was equal to previous");
                    return false;
                }
                else {
                    Log.i(TAG, "value read from service: " + MOTION_SERVICE_UUID + ", characteristic: " + GYRO_CHAR_UUID
                            + " with data " + parseBluetoothGyroData(gyroChar.getValue()));
                    gyroscopeDataByteArray = dataByteArray;
                }
            }
            //toggle reading acc and gyro
            //accRead = !accRead;
            return true;
        }


    /**
     * converts acc data from byte array sent by hexiwear to a formatted string
     *
     * @param data is the byte array from the acc characteristic read value
     * @return a string of the converted data
     */
    public static String parseBluetoothAccData(byte[] data){
        String unit = "g";

        int xintVal = ((int) data[1] << 8) | (data[0] & 0xff);
        float xfloatVal = (float) xintVal / 100;

        int yintVal = ((int) data[3] << 8) | (data[2] & 0xff);
        float yfloatVal = (float) yintVal / 100;

        int zintVal = ((int) data[5] << 8) | (data[4] & 0xff);
        float zfloatVal = (float) zintVal / 100;

        return String.format("%.2f %s;%.2f %s;%.2f %s", xfloatVal, unit, yfloatVal, unit, zfloatVal, unit);
    }

    /**
     * converts gyro data from byte array sent by hexiwear to a formatted string
     *
     * @param data is the byte array from the gyro characteristic read value
     * @return a string of the converted data
     */
    public static String parseBluetoothGyroData(byte[] data){
        String unit = " ";

        int xintVal = ((int) data[1] << 8) | (data[0] & 0xff);
        float xfloatVal = (float) xintVal * 10;

        int yintVal = ((int) data[3] << 8) | (data[2] & 0xff);
        float yfloatVal = (float) yintVal * 10;

        int zintVal = ((int) data[5] << 8) | (data[4] & 0xff);
        float zfloatVal = (float) zintVal * 10;

        return String.format("%.2f %s;%.2f %s;%.2f %s", xfloatVal, unit, yfloatVal, unit, zfloatVal, unit);
    }



    // utility method for converting short form UUID into UUID
    public static UUID convertFromInteger(int i) {
        final long MSB = 0x0000000000001000L;
        final long LSB = 0x800000805f9b34fbL;
        long value = i & 0xFFFFFFFF;
        return new UUID(MSB | (value << 32), LSB);
    }


    private byte[] concatenateByteArrays(byte[] a, byte[] b){

        if((a == null) || (b == null))
            return null;

        try {
            outputStream.reset();

            outputStream.write(a);
            outputStream.write(b);
        }
        catch(IOException ie) {
            ie.printStackTrace();
        }
        return outputStream.toByteArray();
    }
}
