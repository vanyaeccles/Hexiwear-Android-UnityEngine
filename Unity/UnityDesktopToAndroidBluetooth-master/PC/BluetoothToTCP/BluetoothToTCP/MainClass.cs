using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerClient;

namespace BluetoothToTCP
{
    class MainClass
    {

        private Form1 originalForm;
        private IContainer components;
        private DataClient client;
        private Timer timer;
        private LockFreeLinkPool<byte[]> BTDataIn;



        public MainClass(Form form)
        {
            originalForm = (Form1)form;
            Console.WriteLine("Starting BluetoothToTCP");
            createTrayIcon();

            client = new DataClient();
            client.getDataDelegate += getTCPData;
            client.Start();

            BTDataIn = new LockFreeLinkPool<byte[]>();
            timer = new Timer();
            timer.Tick += new EventHandler(Update);
            //timer.Interval = 1000 / 60; // 60 FPS, in miliseconds
            timer.Interval = 1000 / 120; // 120 FPS, in miliseconds
            timer.Start();

            BTTransmitterSingleton.Instance.getDataDelegate += getBTDataFromThread;
            BTTransmitterSingleton.Instance.StartBluetooth();
        }

        void Update(object sender, EventArgs e)
        {
            // Check Concurrent Pool for data
            List<byte[]> dataList = new List<byte[]>();
            SingleLinkNode<byte[]> node = null;

            while (BTDataIn.Pop(out node))
            {
                dataList.Add(node.Item);
            }
            if (dataList.Count > 0)
            {
                // Turn the LIFO list into a FIFO list
                dataList.Reverse();
                foreach (byte[] data in dataList)
                {
                    getBTData(data);
                }
            }
        }

        public void Dispose()
        {
            if (components != null)
                components.Dispose();

            client.OnDestroy();
        }



        private void getTCPData(byte[] data)
        {
            originalForm.setLastTCPMessage(data);

            // Send it by Bluetooth
            BTTransmitterSingleton.Instance.Send(data);
        }

        // Can be called from another thread
        private void getBTDataFromThread(byte[] data)
        {
            SingleLinkNode<byte[]> node = new SingleLinkNode<byte[]>();
            node.Item = data;
            BTDataIn.Push(node);
        }

        private void getBTData(byte[] data)
        {
            //Send to the display box
            //originalForm.setLastBTMessage(data);

            

            // Send it by TCP
            client.sendData(data);
        }






        private void createTrayIcon()
        {
            components = new Container();
            NotifyIcon trayIcon = new NotifyIcon(components);
            trayIcon.Text = "Bluetooth To TCP";
            trayIcon.Icon = BluetoothToTCP.Properties.Resources.Icon;
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = new MenuItem();
            contextMenu.MenuItems.AddRange(new MenuItem[] { menuItem });
            menuItem.Index = 0;
            menuItem.Text = "E&xit";
            menuItem.Click += new System.EventHandler(this.menuItem_Click);
            trayIcon.ContextMenu = contextMenu;
            trayIcon.DoubleClick += new System.EventHandler(this.trayIcon_DoubleClick);
            trayIcon.Visible = true;
        }

        private void trayIcon_DoubleClick(object Sender, EventArgs e)
        {
            originalForm.Show();
            originalForm.WindowState = FormWindowState.Normal;
        }

        private void menuItem_Click(object Sender, EventArgs e)
        {
            originalForm.Close();
        }
    }
}
