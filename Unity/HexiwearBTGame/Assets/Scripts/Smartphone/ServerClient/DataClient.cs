using UnityEngine;
using System.Collections;
using ServerClient.TCP;

namespace ServerClient
{
	public class DataClient : MonoBehaviour
	{
		public delegate void GetData(byte[] data);
		public event GetData getDataDelegate;

		private LockFreeLinkPool<byte[]> dataIn;

		// Use this for initialization
		void Start()
		{
			dataIn = new LockFreeLinkPool<byte[]>();
			TransmitterSingleton.Instance.clientStartListening(new TransmitterSingleton.GetData(clientGetData));
		}
		
		// Update is called once per frame
		void Update()
		{
			// Check Concurrent Pool for data
			SingleLinkNode<byte[]> node = null;
			if (dataIn.Pop(out node))
			{
                byte[] data = node.Item;
				getDataDelegate(data);
			}
		}

		void OnDestroy()
		{
			TransmitterSingleton.Instance.clientStopListening();
		}

		public void sendData(byte[] data)
		{
			TransmitterSingleton.Instance.sendDataToServer(data);
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












