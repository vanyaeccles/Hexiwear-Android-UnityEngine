using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Text;


// Two approaches to implementing a simple UDP socket for recieving data via a specified port
// 1st uses an asynccallback method
// 2nd uses a seperate thread, adapted from solution given at https://stackoverflow.com/questions/4118219/how-to-use-udpclient-beginreceive-in-a-loop


public class UDPSocket : MonoBehaviour {

    UdpClient listenClient;
    public int receivePort;
    string serverIP;
    public string ipAddress;
    //IPAddress groupIP;
    IPEndPoint remoteEP;

    private Thread listenThread;



    public SensorDataHandler sensorDataHandler;

    // Use this for initialization
    void Start () {


        //First approach using ASyncCallBack
        /*
        IPAddress groupIP = IPAddress.Parse(ipAddress);
        Debug.Log("Starting Client");
        remoteEP = new IPEndPoint(IPAddress.Any, receivePort);
        listenClient = new UdpClient(remoteEP);
        //client.JoinMulticastGroup(groupIP);
        listenClient.BeginReceive(new AsyncCallback(ReceiveServerInfo), null);
        */

        listenThread = new Thread(new ThreadStart(SimplestReceiver));
        listenThread.Start();



        //byte[] datum = new byte[6];
        //datum[0] = 0x44;
        //datum[1] = 0xff;
        //datum[2] = 0x44;
        //datum[3] = 0xff;
        //datum[4] = 0x44;
        //datum[5] = 0xff;

        ////Debug.Log("newer conversion " + parseBluetoothAccData(datum));
        //Debug.Log("old conversion " + parseBluetoothAccDataOld(datum));
    }
	

    void ReceiveServerInfo(IAsyncResult result)
    {
        Debug.Log("Received Server Info");
        byte[] receivedBytes = listenClient.EndReceive(result, ref remoteEP);
        serverIP = Encoding.ASCII.GetString(receivedBytes);
        Debug.Log(serverIP);
    }


    private void SimplestReceiver()
    {
        Debug.Log("Overall listener thread started.");

        IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, receivePort);
        listenClient = new UdpClient(listenEndPoint);
        Debug.Log("listen client started.");

        while (true)
        {
            Byte[] data = null;
            try
            {
                data = listenClient.Receive(ref listenEndPoint);
                //string message = Encoding.ASCII.GetString(data);
                sensorDataHandler.parseIncomingBTByteArray(data);
                if(data != null)
                {
                    Debug.Log("listen client got data ");
                }

                //parseBluetoothAccDataOld(data);
                //Debug.Log("Listener heard: " + xfloatVal + " " + yfloatVal + " " + zfloatVal);
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode != 10060)
                    Debug.Log("a more serious error " + ex.ErrorCode);
                else
                    Debug.Log("expected timeout error");
            }

            //Thread.Sleep(10); // tune for your situation, can usually be omitted
        }
    }

    void OnDestroy() { CleanUp(); }
    void OnDisable() { CleanUp(); }
    // be certain to catch ALL possibilities of exit in your environment,
    // or else the thread will typically live on beyond the app quitting.

    void CleanUp()
    {
        Debug.Log("Cleanup for listener...");

        // note, consider carefully that it may not be running
        listenClient.Close();
        Debug.Log("listen client correctly stopped");

        listenThread.Abort();
        listenThread.Join(5000);
        listenThread = null;
        Debug.Log("listener thread correctly stopped");
    }

    
}
