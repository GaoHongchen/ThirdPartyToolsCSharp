using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GHCLibs
{
    class SocketServer
    {
        Socket socketServer;
        //网络Socket
        SocketServer(IPAddress ipAddress, int port)
        {
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socketServer.Bind(ipEndPoint);
                socketServer.Listen(10);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + "\n请重新检查配置文件中的IP地址。", "网络连接错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Environment.Exit(0);

            }
            Thread threadListen = new Thread(ThreadListen);
            threadListen.Start();
        }

        private void ThreadListen()
        {
            while (true)
            {
                Socket socketClient = socketServer.Accept();
                //socketClient.Send(Encoding.ASCII.GetBytes("连接服务器成功！"));
                Thread threadReceiveMessage = new Thread(ThreadReceiveMessage);
                threadReceiveMessage.Start(socketClient);
            }
        }

        private void ThreadReceiveMessage(object clientSocket)
        {
            Socket socketClient = (Socket)clientSocket;
            byte[] bufferRX = new byte[1024];
            while (true)
            {
                try
                {
                    int numRX = socketClient.Receive(bufferRX);
                    string strRX = Encoding.ASCII.GetString(bufferRX, 0, numRX);
                    //string strRX = System.Text.Encoding.Default.GetString(Encoding.Unicode.GetBytes(System.Text.Encoding.Default.GetString(bufferRX)));
                    //string strRX = UTF8Encoding.UTF8.GetString(bufferRX);
                    //string[] arrRX = strRX.Split(',');
                    //string strType = arrRX[0];
                    //string strValue = arrRX[1];
                    //if (strRX == "update")
                    // {
                    //RemoteUpdate(strRX, "");
                    // break;
                    //  }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    socketClient.Shutdown(SocketShutdown.Both);
                    socketClient.Close();
                    break;
                }
            }
        }
    }
}
