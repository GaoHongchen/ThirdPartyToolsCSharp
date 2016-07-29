using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace GHCLibs
{
    class SerialPortUser
    {
        SerialPort serialPort;

        StringBuilder stringBuider;

        public SerialPortUser(string strPortName, int nBauRate)
        {
            serialPort = new SerialPort();
            serialPort.PortName = strPortName;
            serialPort.BaudRate = nBauRate;
            serialPort.ReceivedBytesThreshold = 17;

            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
        }

        public void OpenSerialPort()
        {
            while (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.Open();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void SendData(byte[] bufferTX,int nDataByteTX = 14)
        {
            //串口发送
            serialPort.Write(bufferTX, 0, nDataByteTX);
        }

        
        /// <summary>
        /// 串口数据接收事件
        /// </summary>
        protected void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Thread.Sleep(100);
            int nDataByteRX = serialPort.BytesToRead;
            byte[] bufferRX = new byte[nDataByteRX];
            serialPort.Read(bufferRX, 0, nDataByteRX);
            serialPort.DiscardInBuffer();
            
            stringBuider.Clear();
            /*
            foreach (byte b in bufferRX)
            {
                依次拼接出16进制字符串
                stringBuider.Append(b.ToString("X2")+" ");
            }
            */
            //直接按ASCII规则转换成字符串
            stringBuider.Append(Encoding.ASCII.GetString(bufferRX));
        }
    }
}
