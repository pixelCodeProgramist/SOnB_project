using Communication;
using Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace memoryNetworkGame.Connection
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
                IPAddress hostadd = IPAddress.Parse("25.83.113.195");
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
            return IPAddress.Parse("127.0.0.1");
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

        public void SendFile(string filePath)
        {
            s.SendFile(filePath);
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
                
                /*if (tmp.StartsWith("PlayersList"))
                {
                    iCommunication = CommunicationFactory.GetCommunication(CommunicationType.ClientCommunication);
                }
                if (tmp.StartsWith("Sending file"))
                {
                    iCommunication = CommunicationFactory.GetCommunication(CommunicationType.FileCommunication);
                }
                if (tmp.StartsWith("Start game"))
                {
                    iCommunication = CommunicationFactory.GetCommunication(CommunicationType.StartGameCommunication);
                }
                if(tmp.StartsWith("RevealCard"))
                {
                    iCommunication = CommunicationFactory.GetCommunication(CommunicationType.ButtonNumberComunication);
                }
                if(tmp.StartsWith("Winner"))
                {
                    iCommunication = CommunicationFactory.GetCommunication(CommunicationType.WinnerCommunication);
                }
                if (tmp.StartsWith("PlayerTurn"))
                {
                    iCommunication = CommunicationFactory.GetCommunication(CommunicationType.PlayerTurnCommunication);
                }
                responseMessage = new ResponseMessage();
                responseMessage.ICommunicationType = iCommunication;
                responseMessage.Message = tmp;
                responseMessage.ReceivedBytes = receivedBytes;*/
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
