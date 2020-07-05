using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BluetoothToTCP
{
    class CSVWriter
    {

        private static StringBuilder csv = new StringBuilder();

        // path where the csv will be created and written
        private static string filePath = "C:\\Hexiwear-Unity\\StreamData\\";

        public static byte startByte = 0xFF;
        public static byte stopByte = 0xFF;

        public static void WriteNewLineToCSV(byte[] _data) {

            csv.AppendLine(parseAccByteArray(_data));
        }

        public static void WriteToFile() {

            File.WriteAllText(filePath + DateTime.Now.ToString("dd,MM,yyyy_HH,mm,ss") + ".csv", csv.ToString());

            // clear the stringbuilder
            csv.Remove(0, csv.Length);
        }

        public static void newCSV() {
            csv = new StringBuilder();
        }


        //converts acc data byte array into floats for acc on each axis
        private static string parseAccByteArray(byte[] data)
        {
            //arrives as a byte array of 3 int16_t values, need to convert to floats
            short x = (short)(data[0] + (data[1] << 8));
            float xFloatValAcc = (float)x / 100;

            short y = (short)(data[2] + (data[3] << 8));
            float yFloatValAcc = (float)y / 100;

            short z = (short)(data[4] + (data[5] << 8));
            float zFloatValAcc = (float)z / 100;


            int hexID = data[7];

            string newLine = string.Format("{0}, {1}, Acc,{2},{3},{4}", DateTime.Now.ToString("h:mm:ss:ms tt"), hexID, xFloatValAcc, yFloatValAcc, zFloatValAcc);

            return newLine;
        }

    }
}
