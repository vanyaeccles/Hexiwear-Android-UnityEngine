using UnityEngine;
using System.Collections;
using ServerClient.TCP;
using System;

namespace ServerClient
{
	public class DataServer : MonoBehaviour
	{
		public delegate void GetData(byte[] data);
		public event GetData getDataDelegate;
		public delegate void ClientConnected();
		public event ClientConnected clientConnectedDelegate;
		
		private LockFreeLinkPool<byte[]> dataIn;

        public SensorDataHandler dataHandler;


        private float average_last_ten = 0;
        private float average_last_hundred = 0;
        private int count = 0;
        private int big_count = 0;
        DateTime before;
        DateTime after;
        TimeSpan duration;

        // Use this for initialization
        void Start()
		{
			dataIn = new LockFreeLinkPool<byte[]>();
			TransmitterSingleton.Instance.clientConnectedDelegate += OnClientConnected;
			TransmitterSingleton.Instance.startServer(new TransmitterSingleton.GetData(getData));

            before = DateTime.Now;
        }
		
		// Update is called once per frame
		void Update()
		{

            // Check Concurrent Pool for data
            SingleLinkNode<byte[]> node = null;

            int dataInSize = 0;

			while (dataIn.Pop(out node))
			{
                dataInSize++;

                byte[] data = node.Item;
				getDataDelegate(data);
                //Debug.Log("DataServerUpdate() got data");

                // parse the data byte array into float values
                dataHandler.parseIncomingBTByteArray(data);


                ////profile time spent
                //after = DateTime.Now;
                //duration = after.Subtract(before);
                //before = DateTime.Now;
                //average_last_ten += duration.Milliseconds;
                //average_last_hundred += duration.Milliseconds;
                //count++;
                //big_count++;


                //if (big_count >= 100)
                //{
                //    Debug.Log("average of last hundred " + average_last_hundred / big_count);
                //    big_count = 0;
                //    average_last_hundred = 0;
                //}

                //if (count >= 10)
                //{
                //    Debug.Log("average of last ten " + average_last_ten / count);
                //    count = 0;
                //    average_last_ten = 0;
                //}

            }

            //if(dataInSize > 0)
                //Debug.Log("size of data q " + dataInSize);


            if (Input.GetKeyDown("space"))
            {
                //sendData("space key was pressed");

                //var unixTimestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
                //Debug.Log("unix time ms " + unixTimestamp);

                Debug.Log("time ms " + Time.time);
            }

        }

		// test
		void OnGUI()
		{
			//GUI.Label(new Rect(10, Screen.height - 30, 300, 20), "Smartphone Helper: " + TransmitterSingleton.Instance.connectionState);
		}
		
		void OnDestroy()
		{
			TransmitterSingleton.Instance.stopServer();
		}

		public void sendData(string data)
		{
			TransmitterSingleton.Instance.sendDataToClient(data);
		}
		
		void getData(byte[] data)
		{
            //Debug.Log("getData() + " + data);


			// In order to get back to the main thread
			// (Unity cannot handle scene modifications on other threads),
			// we put the data in a concurrent pool
			SingleLinkNode<byte[]> node = new SingleLinkNode<byte[]>();
			node.Item = data;
			dataIn.Push(node);
			// Next part of the pipeline is DataServer.Update()
		}

		void OnClientConnected()
		{
			clientConnectedDelegate.Invoke();
		}
	}
}












