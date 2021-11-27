using Communication;
using Connection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SOnB.Client
{
    public class TcpConnection
    {
        private TcpListener tcpLsn;
        private Socket s;
        private ICommunication iCommunication;

        public Boolean connect()
        {
            try
            {
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress hostadd = IPAddress.Parse("192.168.56.1");
                int port = 8000;
                IPEndPoint EPhost = new IPEndPoint(hostadd, port);
                s.Connect(EPhost);
                return true;
            }
            catch (SocketException e)
            {
                Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);
                return false;
            }

        }

        public IPAddress GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipAddress;
                }
            }
            return IPAddress.Parse("192.168.56.1");
        }
        public Socket GetSocket()
        {
            return s;
        }

        public void Send(string message)
        {
            Byte[] byteData = System.Text.Encoding.ASCII.GetBytes(message.ToCharArray());
            s.Send(byteData, byteData.Length, 0);
        }

        public ResponseMessage ReceiveMessage()
        {
            ResponseMessage responseMessage = null;
            try
            {
                byte[] receivedBytes = new byte[1024 * 1024 * 10];
                int ret = s.Receive(receivedBytes);
                string tmp = null;
                tmp = System.Text.Encoding.UTF8.GetString(receivedBytes, 0, ret);


                responseMessage = new ResponseMessage();
                responseMessage.ICommunicationType = iCommunication;
                responseMessage.Message = tmp;
                responseMessage.ReceivedBytes = receivedBytes;
                return responseMessage;

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                if (tcpLsn != null) tcpLsn.Stop();

            }
            return responseMessage;
        }
    }
}
