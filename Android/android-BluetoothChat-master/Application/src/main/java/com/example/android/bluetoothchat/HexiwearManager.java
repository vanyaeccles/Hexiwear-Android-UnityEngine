package com.example.android.bluetoothchat;

import android.bluetooth.BluetoothDevice;
import android.content.Context;

import com.example.android.common.logger.Log;

import java.util.ArrayList;
import java.util.HashMap;

import static com.example.android.bluetoothchat.Constants.HEXIWEAR_ADDRESS_0;
import static com.example.android.bluetoothchat.Constants.HEXIWEAR_ADDRESS_1;
import static com.example.android.bluetoothchat.Constants.HEXIWEAR_ADDRESS_2;
import static com.example.android.bluetoothchat.Constants.HEXIWEAR_ID_0;
import static com.example.android.bluetoothchat.Constants.HEXIWEAR_ID_1;
import static com.example.android.bluetoothchat.Constants.HEXIWEAR_ID_2;


/**
 * A class that manages the gatt clients that are connected to hexiwears
 */
public class HexiwearManager {

    private static final String TAG = "HexiwearManager";

    /**
     * Map data structure matching MAC addresses to ids
     */
    HashMap<String, Integer> hexiwearList = new HashMap<>();
    /**
     * Member array of ble gatt client objects
     */
    public ArrayList<GattClient> mGattClients = new ArrayList<>();


    public HexiwearManager(){

        //populate gatt client list with null values
        mGattClients.add(0, null);
        mGattClients.add(1, null);
        mGattClients.add(2, null);

        //populate the values in the hashmap
        hexiwearList.put(HEXIWEAR_ADDRESS_0, HEXIWEAR_ID_0);
        hexiwearList.put(HEXIWEAR_ADDRESS_1, HEXIWEAR_ID_1);
        hexiwearList.put(HEXIWEAR_ADDRESS_2, HEXIWEAR_ID_2);
    }


    public void setupHexiwearConnection(int hexID, BluetoothDevice device, Context context){

        if(mGattClients.get(hexID) != null){
            mGattClients.get(hexID).stop();
            mGattClients.set(hexID, null);
        }


        Log.i(TAG, "Setting up Gatt for hexiwear " + hexID);
        GattClient mGattClient = new GattClient(hexID);
        //connect the new gatt client
        mGattClient.connectGattClient(device, context);

        //add the gatt client to the arraylist, indexed by its ID
        mGattClients.set(hexID, mGattClient);
    }


    public void close(){
        for(GattClient g: mGattClients)
            if(g != null)
                g.stop();
    }



    public byte[][] readHexiwears(){

        byte[][] hexData = new byte[mGattClients.size()][];

        for (GattClient g: mGattClients){
            if(g == null)
                continue;

            android.util.Log.i(TAG, "time of read: " + g.hexID + " " + System.currentTimeMillis());

            g.isBlocked = true;
            g.readTXCharacteristic();

            //hexData[g.hexID] = g.dataByteArray;


            //wait until operation is complete before reading the next one
            while(g.isBlocked){}
        }

        return hexData;
    }

    /*public byte[] readHexiwear(){

        for (GattClient g: mGattClients){
            if(g == null)
                return null;

            android.util.Log.i(TAG, "time of read: " + g.hexID + " " + System.currentTimeMillis());

            g.isBlocked = true;
            g.readTXCharacteristic();

            //hexData[g.hexID] = g.dataByteArray;


            //wait until operation is complete before reading the next one
            while(g.isBlocked){}
        }

        return hexData;
    }*/


    /*
        for (GattClient g: mGattClients) {
            if(g == null)
                continue;
            //if(g != null)
            //Log.i(TAG, "Gatt client " + g.hexID + " from list of size " + hexiwearManager.mGattClients.size());
            //}

            //int mConnectionState = getConnectionState(g.getBTDevice(), GATT);

            //if(mConnectionState  == STATE_CONNECTED){

                //if the data is ready, send it as a message
                if(g.readTXCharacteristic())
                    streamMessage(g.dataByteArray);
                //try {
                //    Thread.sleep(500);
                //} catch (InterruptedException e) {}

            //}


            //else{
            //    Log.i(TAG, "Gatt "+ g.hexID + " not connected, attempting reconnect..");
            //    g.reconnectGatt();
            //}
        }
        */

}
