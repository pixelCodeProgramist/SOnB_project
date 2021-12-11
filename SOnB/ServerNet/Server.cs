using SOnB;
using SOnB.Business;
using SOnB.Business.MessageBitDestroyerFolder;
using SOnB.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SOnBServer
{
    class Server
    {
        private const int _MaxMessageSize = 1024 * 1024;
        private readonly int _listnerPort;
        private int _counter = 0;
        private const int _MaxClients = 10;
        private MainWindow _mainWindow;
        private readonly ConcurrentDictionary<string, ClientThreadModelInfo> _clients;

        public Server(string port)
        {
            this._listnerPort = SetPort(port);
            _clients = new ConcurrentDictionary<string, ClientThreadModelInfo>();
        }

        public String GetPort()
        {
            return _listnerPort.ToString();
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
            this._mainWindow = mainWindow;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, _listnerPort);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            socket.Bind(endpoint);
            socket.Listen(_MaxClients);
            while (true)
            {
                ClientThreadModelInfo client = AddAllClients(socket);
                Thread clientThread = new Thread(() => TryReciveMessage(client))
                {
                    IsBackground = true
                };
                clientThread.Start();
            }
        }

        private ClientThreadModelInfo AddAllClients(Socket socket)
        {
            _counter++;
            Socket client = socket.Accept();
            ClientThreadModelInfo clientInfo = new ClientThreadModelInfo(_counter.ToString(), client);
            _clients.TryAdd(clientInfo.SocketId, clientInfo);
            _mainWindow.UpdateListOfSockets(clientInfo);
            return clientInfo;
        }

        private void TryReciveMessage(ClientThreadModelInfo client)
        {
            while (true)
            {
                try
                {
                    String messageStr = ReceiveMessage(client);
                    if (messageStr.Trim() == "")
                    {
                        HandleException(client);
                        Thread.Sleep(200);
                        break;
                    }
                    _mainWindow.UpdateLogs("Socket:" + client.SocketId + " " + messageStr);
                    if (IsMessageContainError(messageStr))
                    {
                        SendMessage(client.Socket, new CRCMessageLogic(this._mainWindow.GetDataFromTextBox()).GetMessage());
                        messageStr = ReceiveMessage(client);
                        _mainWindow.UpdateLogs("Socket:" + client.SocketId + " " + messageStr);
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                    HandleException(client);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private bool IsMessageContainError(String messageStr)
        {
            return messageStr.Contains(CommunicationMessages.IncorrectCRC);
        }

        private String ReceiveMessage(ClientThreadModelInfo client)
        {
            byte[] message = new byte[_MaxMessageSize];
            int length = client.Socket.Receive(message);
            return Encoding.UTF8.GetString(message, 0, length);
        }

        public void SendMessage(Socket client, string message)
        {
            Byte[] bytes = Encoding.UTF8.GetBytes(message);
            client.Send(bytes, bytes.Length, 0);
        }

        public void SendMessageToAllClients(string message, ObservableCollection<ClientThreadModelInfo> threadModelInfos)
        {
            try
            {
                foreach (var client in _clients)
                {
                    Thread.Sleep(100);
                    if (client.Value.IsBitChangeError)
                    {
                        MessageBitDestroyer messageBitDestroy = new MessageBitDestroyer(message);
                        SendMessage(client.Value.Socket, messageBitDestroy.destroy());
                    }
                    else
                    {
                        if (client.Value.IsRepeatAnswearError)
                        {
                            for (int number = 1; number <= 5; number++)
                            {
                                SendMessage(client.Value.Socket, message);
                                Thread.Sleep(300);
                            }
                        }
                        else
                        {
                            if (client.Value.IsConnectionError)
                                SendMessage(client.Value.Socket, "Connection error");
                            else SendMessage(client.Value.Socket, message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleException(ClientThreadModelInfo client)
        {
            bool result = false;
            _mainWindow.RemoveSocketFromList(client);
            while (!result)
                result = _clients.TryRemove(client.SocketId, out client);
            Console.WriteLine(client);
        }
    }
}
