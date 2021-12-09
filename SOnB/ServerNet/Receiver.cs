using SOnB.Model;
using SOnB.ServerNet.Behaviour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SOnB.ServerNet
{
    public class Receiver
    {
        private const int _MaxMessageSize = 1024 * 1024;
        private readonly List<ClientThreadModelInfo> clients;
        private MainWindow mainWindow;
        public Receiver(ref List<ClientThreadModelInfo> clients, ref MainWindow mainWindow)
        {
            this.clients = clients;
            this.mainWindow = mainWindow;
        }

        public void TryReciveMessageFromClients()
        {
            while (clients.Count() > 0)
            {
                try
                {
                    foreach (ClientThreadModelInfo client in clients)
                    {
                        byte[] message = new byte[_MaxMessageSize];
                        try
                        {
                            int length = client.Socket.Receive(message);
                            String messageStr = Encoding.UTF8.GetString(message, 0, length);
                            messageStr = "Socket " + client.SocketId + ": " + messageStr;
                            ErrorBehaviourI errorBehaviourI = GetBehaviour(messageStr);
                            errorBehaviourI.react(client, messageStr, mainWindow, clients);
                            mainWindow.UpdateLogs(messageStr);
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

        private void HandleException(ClientThreadModelInfo client)
        {
            clients.Remove(client);
            mainWindow.RemoveSocketFromList(client);
        }

        ErrorBehaviourI GetBehaviour(String message)
        {
            ErrorBehaviourI errorBehaviour = null;
            if (message.Contains("CRC Error"))
            {
                errorBehaviour = new ErrorBitBehaviour();
                //errorBehaviour.react(client, message, _mainWindow, sender);

            }
            return errorBehaviour;

        }
    }


}
