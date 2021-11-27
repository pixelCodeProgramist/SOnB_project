using Communication;
using Connection;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SOnB.Client
{
    public class TcpConnection
    {
        private TcpListener _tcpLsn;
        private Socket _socket;
        private ICommunication _iCommunication;

        public Boolean Connect()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress hostadd = IPAddress.Parse("192.168.0.15");
                int port = 8000;
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

                responseMessage = new ResponseMessage
                {
                    ICommunicationType = _iCommunication,
                    Message = tmp,
                    ReceivedBytes = receivedBytes
                };
                return responseMessage;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                if (_tcpLsn != null) _tcpLsn.Stop();
            }
            return responseMessage;
        }
    }
}
