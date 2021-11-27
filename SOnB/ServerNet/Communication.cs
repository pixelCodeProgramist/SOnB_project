using SOnB;
using SOnBServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SOnBCommunication
{
    class Communication
    {
        private const int MAX_MESSAGE_SIZE = 1024 * 1024 * 10;
        public static Server server;
        public Socket client;

        public Communication(Socket client)
        {
            this.client = client;
        }

        public void ReceiveMessage()
        {
            while (true)
            {
                byte[] message = new byte[MAX_MESSAGE_SIZE];
                try
                {
                    int length = client.Receive(message);
                    if (!HandleMessage(Encoding.UTF8.GetString(message, 0, length)))
                        break;
                }
                catch (SocketException ex)
                {
                    HandleException(ex);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private MessageType getMessageType(string message)
        {
            if (message.Contains(CommunicationMessages.START_GAME))
                return MessageType.START_GAME;
            if (message.Contains(CommunicationMessages.CLOSE_CONNECTION))
                return MessageType.CLOSE_CONNECTION;
            throw new Exception("Invalid message exception");
        }

        private bool HandleMessage(string message)
        {
            try
            {
                MessageType type = getMessageType(message);
                switch (type)
                {
                    case MessageType.START_GAME: return StartGame();
                    case MessageType.CLOSE_CONNECTION:
                        {
                            HandleException(new Exception("Client closed connection"));
                            return false;
                        }
                    default: return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return true;
            }
        }


        private bool StartGame()
        {
            server.sendMessageToAllClients("aaa");
            return true;
        }

 
        public void SendMessage(string message)
        {
            try
            {
                Byte[] bytes = Encoding.UTF8.GetBytes(message);
                client.Send(bytes, bytes.Length, 0);
            }
            catch(SocketException ex)
            {
                HandleException(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleException(Exception ex)
        {
            Console.WriteLine(ex.Message);
            server.removeClient(client.RemoteEndPoint.ToString());
        }
    }
}
