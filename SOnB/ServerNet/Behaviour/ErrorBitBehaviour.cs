using SOnB.Model;
using SOnBServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SOnB.ServerNet.Behaviour
{
    class ErrorBitBehaviour : ErrorBehaviourI
    {
        public void react(ClientThreadModelInfo client, String message, MainWindow mainWindow, List<ClientThreadModelInfo> clients)
        {
            Sender sender = new Sender(ref clients);
            client.IsBitChangeError = false;
            message += "Retransmission message to socket: " + client.SocketId;
            mainWindow.UpdateLogs(message);
            sender.SendMessage(client.Socket, message);
        }
    }
}
