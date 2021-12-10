using Connection;
using SOnB_Client.Connection;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SOnB.Client
{
    public class TcpConnection
    {
        private Socket _socket;
        private MessageType _messageType;

        public Boolean Connect(int port)
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress hostadd = GetIPAddress();

               
                IPEndPoint EPhost = new IPEndPoint(hostadd, port);
                _socket.Connect(EPhost);
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

            return IPAddress.Parse("192.168.0.15");

        }
        public Socket GetSocket()
        {
            return _socket;
        }

        public void Send(string message)
        {
            Byte[] byteData = Encoding.ASCII.GetBytes(message.ToCharArray());
            _socket.Send(byteData, byteData.Length, 0);
        }

        public ResponseMessage ReceiveMessage()
        {
            ResponseMessage responseMessage = null;
            try
            {
                byte[] receivedBytes = new byte[1024 * 1024 * 10];
                int ret = _socket.Receive(receivedBytes);
                string tmp = null;
                tmp = Encoding.UTF8.GetString(receivedBytes, 0, ret);
                _messageType = MessageTypeUtility.GetMessage(tmp);

                responseMessage = new ResponseMessage
                {
                    Type = _messageType,
                    Message = tmp,
                    ReceivedBytes = receivedBytes
                };
                return responseMessage;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            return null;
        }
    }
}
