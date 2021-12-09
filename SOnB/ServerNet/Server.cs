using SOnB;
using SOnB.Business;
using SOnB.Business.MessageBitDestroyerFolder;
using SOnB.Model;
using System;
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
        private readonly List<ClientThreadModelInfo> _clients;
        private Thread _addClientThread;

        public Server(string port)
        {
            this._listnerPort = SetPort(port);
            _clients = new List<ClientThreadModelInfo>();
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
            this._addClientThread = new Thread(() => this.AddAllClients(socket));
            this._addClientThread.IsBackground = true;
            this._addClientThread.Start();
            while (true)
            {
                TryReciveMessageFromClients();
            }
        }

        private void AddAllClients(Socket socket)
        {
            _counter = 0;
            while (true)
            {
                _counter++;
                Socket client = socket.Accept();
                ClientThreadModelInfo clientInfo = new ClientThreadModelInfo(_counter.ToString(), client);
                _clients.Add(clientInfo);
                _mainWindow.UpdateListOfSockets(clientInfo);
            }
        }

        private void TryReciveMessageFromClients()
        {
            while (_clients.Count() > 0)
            {
                try
                {
                    foreach (ClientThreadModelInfo client in _clients)
                    {
                        try
                        {
                            String messageStr = ReceiveMessage(client);
                            if (messageStr.Trim() == "") HandleException(client);
                            _mainWindow.UpdateLogs(messageStr);
                            if (IsMessageContainError(messageStr))
                            {
                                SendMessage(client.Socket, new CRCMessageLogic(this._mainWindow.GetDataFromTextBox()).GetMessage());
                                messageStr = ReceiveMessage(client);
                                _mainWindow.UpdateLogs(messageStr);
                            }
                        }
                        catch (SocketException ex)
                        {
                            Console.WriteLine(ex.Message);
                            HandleException(client);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void DoDataRetransmission(ClientThreadModelInfo client)
        {
            SendMessage(client.Socket, new CRCMessageLogic(this._mainWindow.GetDataFromTextBox()).GetMessage());
            String messageStr = ReceiveMessage(client);
            _mainWindow.UpdateLogs(messageStr);
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
                for (int i = 0; i < _clients.Count; i++)
                {
                    Thread.Sleep(100);
                    if (threadModelInfos[i].IsBitChangeError)
                    {
                        MessageBitDestroyer messageBitDestroy = new MessageBitDestroyer(message);
                        SendMessage(_clients[i].Socket, messageBitDestroy.destroy());
                    }
                    else
                    {
                        if (threadModelInfos[i].IsRepeatAnswearError)
                        {
                            for(int number = 0; number < 10; number++)
                            {
                                SendMessage(_clients[i].Socket, message);
                                Thread.Sleep(50);
                            }
                        }
                        else
                        {
                            if(threadModelInfos[i].IsConnectionError)
                                SendMessage(_clients[i].Socket, "Connection error");
                            else SendMessage(_clients[i].Socket, message);
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
            _clients.Remove(client);
            _mainWindow.RemoveSocketFromList(client);
        }
    }
}
