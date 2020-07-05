using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class SensorDataHandler : MonoBehaviour {

    public float xFloatValAcc = 1.0f;
    public float yFloatValAcc = 1.0f;
    public float zFloatValAcc = 1.0f;


    public float xFloatValGyro = 1.0f;
    public float yFloatValGyro = 1.0f;
    public float zFloatValGyro = 1.0f;


    public Vector3 sphereScale;
    public Vector3 cubeScale;

    private const byte accelerometerID = 0x00;
    private const byte gyroscopeID = 0x01;
    private const byte hexID_0 = 0x00;
    private const byte hexID_1 = 0x01;
    private const byte stopID = 0xFF;

    public List<GamePlayer> players = new List<GamePlayer>();



    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update (){
    }

    // A box for displaying the frame rate
    //void OnGUI()
    //{
    //    GUI.Box(new Rect(0, 0, 100, 50), (1 / Time.deltaTime).ToString());
    //}



    #region DATA PROCESSING

    public void parseIncomingBTByteArray(byte[] btData)
    {
        // incoming data will have an id for sensor type 
        byte sensorID = btData[6];
        //and an id for the hexiwear device that sent it
        byte deviceID = btData[7];

        // Get GamePlayer object for corresponding device ID
        switch (deviceID)
        {
            case hexID_0:
                {
                    processSensorData(players[hexID_0], sensorID, btData);
                    break;
                }
            case hexID_1:
                {
                    Debug.Log("Hex1");
                    processSensorData(players[hexID_1], sensorID, btData);
                    break;
                }
            case stopID:
                {
                    Debug.Log("Stream Stopped");
                    break;
                }
            default:
                {
                    Debug.LogError("Incoming hexiwear device ID not recognised, can't process data");
                    break;
                }
        }  
    }




    private void processSensorData(GamePlayer player, byte sensorID, byte[] btData) {

        //process the data based on sensor type and send to the gameplayer object
        switch (sensorID)
        {
            case accelerometerID:
                {
                    parseAccByteArray(btData);
                    player.setAccelerometer(xFloatValAcc, yFloatValAcc, zFloatValAcc);
                    break;
                }
            case gyroscopeID:
                {
                    parseGyroByteArray(btData);
                    player.setGyroscope(xFloatValGyro, yFloatValGyro, zFloatValGyro);
                    break;
                }

            default:
                {
                    Debug.LogError("Incoming sensor ID not recognised, can't process data");
                    // quit the function early
                    return;
                }
        }
    }





    private void parseAccByteArray(byte[] data) {
        //arrives as a byte array of 3 int16_t values, need to convert to floats
        short x = (short)(data[0] + (data[1] << 8));
        xFloatValAcc = (float)x / 100;

        short y = (short)(data[2] + (data[3] << 8));
        yFloatValAcc = (float)y / 100;

        short z = (short)(data[4] + (data[5] << 8));
        zFloatValAcc = (float)z / 100;

        Debug.Log("acc data received " + xFloatValAcc + " " + yFloatValAcc + " " + zFloatValAcc + DateTime.Now.ToString("hh.mm.ss.ffffff"));
    }


    private void parseGyroByteArray(byte[] data) {
        //arrives as a byte array of 3 int16_t values, need to convert to floats
        short x = (short)(data[0] + (data[1] << 8));
        xFloatValGyro = (float)x * 10;

        short y = (short)(data[2] + (data[3] << 8));
        yFloatValGyro = (float)y * 10;

        short z = (short)(data[4] + (data[5] << 8));
        zFloatValGyro = (float)z * 10;

        //Debug.Log("gyro data received " + xFloatValGyro + " " + yFloatValGyro + " " + zFloatValGyro + DateTime.Now.ToString("hh.mm.ss.ffffff"));
    }






    //float timeSince = 0;
    //float then = 0;
    //double unixTimestamp;

    //private void updateSpheres() {

    //    timeSince = Time.time - then;

    //    unixTimestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

    //    //Debug.Log("acc data received " + xFloatValAcc + " " + yFloatValAcc + " " + zFloatValAcc + " with time " + timeSince + " with unix timestamp " +  unixTimestamp);

    //    then = Time.time;

    //    sphereScale[0] = 1.0f * xFloatValAcc;
    //    sphereScale[1] = 1.0f * yFloatValAcc;
    //    sphereScale[2] = 1.0f * zFloatValAcc;
    //}




    //private void updateCubes()
    //{
    //    cubeScale[0] = 1.0f * xFloatValAcc;
    //    cubeScale[1] = 1.0f * yFloatValAcc;
    //    cubeScale[2] = 1.0f * zFloatValAcc;
    //}


    public static string ByteArrayToString(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }


    #endregion
}
