using SOnB;
using SOnB.Business.MessageBitDestroyerFolder;
using SOnB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SOnBServer
{
    class Server
    {
        private const int MAX_MESSAGE_SIZE = 1024 * 1024;
        private int listnerPort;
        private int counter = 0;
        private const int MAX_CLIENTS = 10;
        List<Socket> clients;
        
        public Server(string[] args)
        {
            this.listnerPort = SetPort(args[0]);
            clients = new List<Socket>();
        }

        public String GetPort()
        {
            return listnerPort.ToString();
        }

        private int SetPort(string text)
        {
            int defaultPort = 8000;
            try
            {
                return int.Parse(text);
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine(ex.Message);
                return defaultPort;
            }
        }

        public void Start(MainWindow mainWindow)
        {
            Console.WriteLine("Starting TCP listener...");

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, listnerPort);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            socket.Bind(endpoint);
            socket.Listen(MAX_CLIENTS);
            while (clients.Count() < 9)
            {
                counter++;
                Socket client = socket.Accept();
                clients.Add(client);
                mainWindow.UpdateListOfSockets(new ClientThreadModelInfo(counter.ToString(), client));
            }
            foreach(Socket s in clients)
            {
                byte[] message = new byte[MAX_MESSAGE_SIZE];
                try
                {
                    int length = s.Receive(message);
                    String messageStr = Encoding.UTF8.GetString(message, 0, length);
                    mainWindow.UpdateLogs(messageStr);
                }
                catch (SocketException ex)
                {
                    HandleException(ex,s);
                    
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

  

        public void sendMessage(Socket client, string message)
        {
            Byte[] bytes = Encoding.UTF8.GetBytes(message);
            client.Send(bytes, bytes.Length, 0);
        }

        public void sendMessageToAllClients(string message, ObservableCollection<ClientThreadModelInfo> threadModelInfos)
        {
            try
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    Thread.Sleep(200);
                    if (clients[i] == threadModelInfos[i].Socket && threadModelInfos[i].IsBitChangeError)
                    {
                        MessageBitDestroyer messageBitDestroy = new MessageBitDestroyer(message);
                        sendMessage(clients[i], messageBitDestroy.destroy());
                    }
                    else
                    {
                        sendMessage(clients[i], message);
                    }
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleException(Exception ex, Socket s)
        {
            clients.Remove(s);
            
        }

    }
}
