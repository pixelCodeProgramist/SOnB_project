using SOnB.Business.MessageBitDestroyerFolder;
using SOnB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SOnB.ServerNet
{
    public class Sender
    {
        private readonly List<ClientThreadModelInfo> clients;

        public Sender(ref List<ClientThreadModelInfo> clients)
        {
            this.clients = clients;
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
                for (int i = 0; i < clients.Count; i++)
                {
                    Thread.Sleep(50);
                    if (clients[i].Socket == threadModelInfos[i].Socket && threadModelInfos[i].IsBitChangeError)
                    {
                        MessageBitDestroyer messageBitDestroy = new MessageBitDestroyer(message);
                        SendMessage(clients[i].Socket, messageBitDestroy.destroy());
                    }
                    else
                    {
                        SendMessage(clients[i].Socket, message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    
}
