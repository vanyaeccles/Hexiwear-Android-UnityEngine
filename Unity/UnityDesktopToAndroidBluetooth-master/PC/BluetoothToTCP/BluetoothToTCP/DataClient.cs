using System.Collections;
using System.Collections.Generic;
using ServerClient;
using System.Windows.Forms;
using System;

namespace ServerClient
{
	public class DataClient
	{
		public delegate void GetData(byte[] data);
		public event GetData getDataDelegate;

        private Timer timer;
		private LockFreeLinkPool<byte[]> dataIn;

		// Use this for initialization
		public void Start()
		{
			dataIn = new LockFreeLinkPool<byte[]>();
			TransmitterSingleton.Instance.clientStartListening(new TransmitterSingleton.GetData(clientGetData));

            timer = new Timer();
            timer.Tick += new EventHandler(Update);
            //timer.Interval = 1000 / 60; // 60 FPS, in miliseconds
            timer.Interval = 1000 / 120; // 120 FPS, in miliseconds
            timer.Start();
		}
		
		// Update is called once per frame
        void Update(object sender, EventArgs e)
		{
			// Check Concurrent Pool for data
			List<byte[]> dataList = new List<byte[]>();
			SingleLinkNode<byte[]> node = null;
			while (dataIn.Pop(out node))
			{
				dataList.Add(node.Item);
			}
			if (dataList.Count > 0)
			{
				// Turn the LIFO list into a FIFO list
				dataList.Reverse();
				foreach (byte[] data in dataList)
				{
					getDataDelegate(data);
				}
			}
		}

        public void OnDestroy()
		{
			TransmitterSingleton.Instance.clientStopListening();
		}

		public void sendData(byte[] data)
		{
			TransmitterSingleton.Instance.sendDataToServer(data);
		}

		public string setIP(string ipText)
		{
			string ret = "";

			if (!TransmitterSingleton.Instance.isValidIPAddress(ipText))
			{
				ret = "Invalid IP Address";
			}
			else if (!TransmitterSingleton.Instance.restartClientListening(new TransmitterSingleton.GetData(clientGetData), ipText))
			{
				ret = "Already restarting communications";
			}

			return ret;
		}

		public string restartAutomaticIP()
		{
			if (!TransmitterSingleton.Instance.restartClientListening(new TransmitterSingleton.GetData(clientGetData)))
			{
				return "Already restarting communications";
			}

			return "";
		}

		void clientGetData(byte[] data)
		{
			// In order to get back to the main thread
			// (Unity cannot handle scene modifications on other threads),
			// we put the data in a concurrent pool
			SingleLinkNode<byte[]> node = new SingleLinkNode<byte[]>();
			node.Item = data;
			dataIn.Push(node);
			// Next part of the pipeline is DataHandler.Update()
		}
	}
}












