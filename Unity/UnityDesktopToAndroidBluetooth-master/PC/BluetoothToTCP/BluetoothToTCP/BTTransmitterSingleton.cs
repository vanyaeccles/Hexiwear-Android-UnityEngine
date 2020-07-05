// Based on sample code from
// 32feet.NET - Personal Area Networking for .NET
// By In The Hand Ltd. & Alan J. McFarlane.

using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.Text;

namespace BluetoothToTCP
{
    public class BTTransmitterSingleton
    {
        private static BTTransmitterSingleton instance;
        private BTTransmitterSingleton()
        {

        }
        ~BTTransmitterSingleton()
        {

        }
        public static BTTransmitterSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BTTransmitterSingleton();
                }
                return instance;
            }
        }

        /************************************************************/

        public string connectionState = "Offline";
        public delegate void GetData(byte[] data);
        public event GetData getDataDelegate;

        //readonly Guid OurServiceClassId = new Guid("{f8a8bae3-3eba-493f-89e9-c221964b449b}");
        readonly Guid OurServiceClassId = new Guid("{00001234-0000-1000-8000-00805f9b34fb}");
        readonly string OurServiceName = "BluetoothToTCP";
        //
        volatile bool _closing;
        TextWriter _connWtr;
        BluetoothListener _lsnr;

        public void StartBluetooth()
        {
            //Socket s = new Socket(AddressFamily32.Bluetooth, SocketType.Stream, BluetoothProtocolType.RFComm);

            connectionState = "Starting Bluetooth Transmitter";
            try
            {
                new BluetoothClient();
            }
            catch (Exception ex)
            {
                connectionState = "Bluetooth init failed: " + ex;
                Console.WriteLine(connectionState);
                //Debug.LogError(msg);
                //throw new InvalidOperationException(connectionState, ex);
                return;
            }
            StartListener();
        }

        public bool Send(byte[] message)
        {
            if (_connWtr == null)
            {
                connectionState = "No connection.";
                return false;
            }
            try
            {
                _connWtr.WriteLine(message);
                _connWtr.Flush();
                return true;
            }
            catch (Exception ex)
            {
                connectionState = "Connection lost! (" + ex.Message + ")";
                ConnectionCleanup();
                return false;
            }
        }


        private void StartListener()
        {
            var lsnr = new BluetoothListener(OurServiceClassId);
            lsnr.ServiceName = OurServiceName;
            lsnr.Start();
            _lsnr = lsnr;
            ThreadPool.QueueUserWorkItem(ListenerAccept_Runner, lsnr);
        }

        void ListenerAccept_Runner(object state)
        {
            var lsnr = (BluetoothListener)_lsnr;
            connectionState = "Listening for client";
            while (true)
            {
                var conn = lsnr.AcceptBluetoothClient();
                var peer = conn.GetStream();
                SetConnection(peer, false, conn.RemoteEndPoint);
                //ReadMessagesToEnd(peer);
                ReadFromStreamBytes(peer);
            }
        }

        private void SetConnection(Stream peerStream, bool outbound, BluetoothEndPoint remoteEndPoint)
        {
            if (_connWtr != null)
            {
                Console.WriteLine("Already Connected!");
                //Debug.Log("Already Connected!");
                return;
            }
            _closing = false;
            var connWtr = new StreamWriter(peerStream);
            connWtr.NewLine = "\0";
            _connWtr = connWtr;
            connectionState = (outbound ? "Connected to " : "Connection from ") + remoteEndPoint.Address;
            //Debug.Log((outbound ? "Connected to " : "Connection from ") + remoteEndPoint.Address);
        }

        ////vanya edit
        //private void ReadMessagesToEnd(Stream peer)
        //{

        //    connectionState += ", listening for messages";

        //    var rdr = new StreamReader(peer);
        //    string lines = "";
        //    string line;
        //    StringBuilder lineString = new StringBuilder();
        //    while (true)
        //    {
        //        line = "";
        //        //StringBuilder lineString = new StringBuilder();
        //        try
        //        {

                   
        //            //clear the linestring
        //            lineString.Length = 0;
        //            while (rdr.Peek() >= 0)
        //            {
        //                lineString.Append((char)rdr.Read());
        //            }

        //            line = lineString.ToString();
        //            //line = rdr.ReadToEnd();
        //            //line = rdr.Read().ToString();
        //        }
        //        catch (IOException ioex)
        //        {
        //            connectionState = "Exception";
        //            if (_closing)
        //            {
        //                // Ignore the error that occurs when we're in a Read
        //                // and _we_ close the connection.
        //                connectionState = "Connection was closed hard (read).  ";
        //            }
        //            else
        //            {
        //                connectionState = "Connection was closed hard (read).  " + ioex.Message;
        //                //Debug.Log("Connection was closed hard (read).  " + ioex.Message);
        //            }
        //            break;
        //        }
        //        if (line == null)
        //        {
        //            connectionState = "Connection was closed (read).";
        //            //Debug.Log("Connection was closed (read).");
        //            break;
        //        }

        //        //Console.WriteLine(line);
        //        //Debug.Log(line);


        //        //lines += line;
        //        //string data = lines.Substring(0, lines.Length - 1);
        //        //connectionState = "Have something " + line;

        //        if(line.Length > 0)
        //            if (getDataDelegate != null)
        //                getDataDelegate(line); //send to delegate

                

        //        //lines += line;
        //        //if (lines.EndsWith("\0"))
        //        //{
        //        //    string data = lines.Substring(0, lines.Length - 1);
        //        //    lines = "";
        //        //    if (getDataDelegate != null)
        //        //        getDataDelegate(data); //send to delegate
        //        //}
        //        //else
        //        //{
        //        //    lines += "\r\n";
        //        //}
        //    }
        //    ConnectionCleanup();
        //}

        //private void ReadMessagesToEnd(Stream peer)
        //{
        //    var rdr = new StreamReader(peer);
        //    string lines = "";
        //    while (true)
        //    {
        //        string line;
        //        try
        //        {
        //            line = rdr.ReadLine();
        //        }
        //        catch (IOException ioex)
        //        {
        //            if (_closing)
        //            {
        //                // Ignore the error that occurs when we're in a Read
        //                // and _we_ close the connection.
        //            }
        //            else
        //            {
        //                connectionState = "Connection was closed hard (read).  " + ioex.Message;
        //                //Debug.Log("Connection was closed hard (read).  " + ioex.Message);
        //            }
        //            break;
        //        }
        //        if (line == null)
        //        {
        //            connectionState = "Connection was closed (read).";
        //            //Debug.Log("Connection was closed (read).");
        //            break;
        //        }
        //        //Console.WriteLine(line);
        //        //Debug.Log(line);

        //        lines += line;
        //        if (lines.EndsWith("\0"))
        //        {
        //            string data = lines.Substring(0, lines.Length - 1);
        //            lines = "";
        //            if (getDataDelegate != null)
        //                getDataDelegate(data);
        //        }
        //        else
        //        {
        //            lines += "\r\n";
        //        }
        //    }
        //    ConnectionCleanup();
        //}


        //// function added by vanya to test the reading of the stream


        //private void ReadFromStreamString(NetworkStream stream)
        //{
        //    StringBuilder myCompleteMessage = new StringBuilder();
        //    //byte[] myReadBuffer = new byte[1024];
        //    int numberOfBytesRead = 0;

        //    while (true) {

        //        numberOfBytesRead = 0;
        //        myCompleteMessage.Length = 0;
        //        byte[] myReadBuffer = new byte[1024];

        //        while (stream.DataAvailable)
        //        {
        //            numberOfBytesRead = stream.Read(myReadBuffer, 0, 20);

        //            myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

        //            if (getDataDelegate != null)
        //                getDataDelegate(myCompleteMessage.ToString());

        //        }
        //    }
        //    ConnectionCleanup();
        //}

        //// function added by vanya to test the reading of the stream



        private void ReadFromStreamBytes(NetworkStream stream)
        {
            int numberOfBytesRead = 0;

            while (true)
            {
                numberOfBytesRead = 0;
                byte[] myReadBuffer = new byte[1024];

                while (stream.DataAvailable)
                {
                    //blocking until something is available
                    numberOfBytesRead = stream.Read(myReadBuffer, 0, 8);

                    byte[] dataArray = new byte[8];

                    Array.Copy(myReadBuffer, dataArray, numberOfBytesRead);

                    if (getDataDelegate != null)
                        getDataDelegate(dataArray);

                    //get a copy of the final byte, usually reserved for the device ID
                    byte deviceID = dataArray[7];


                    //check for stop signal, if sent then write to file
                    if (deviceID == CSVWriter.stopByte)
                        CSVWriter.WriteToFile();

                    //check for start signal, if sent then start a new file
                    if (deviceID == CSVWriter.startByte)
                        CSVWriter.newCSV();

                    // put data in csv
                    CSVWriter.WriteNewLineToCSV(dataArray);

                }
                
            }
            ConnectionCleanup();
        }



        private void ConnectionCleanup()
        {
            _closing = true;
            var wtr = _connWtr;
            //_connStrm = null;
            _connWtr = null;
            if (wtr != null)
            {
                try
                {
                    wtr.Close();
                }
                catch (Exception)
                {
                    connectionState = "Problem while cleaning up";
                    //Debug.Log("ConnectionCleanup close ex: " + ex.Message);
                }
            }
        }
    }
}
