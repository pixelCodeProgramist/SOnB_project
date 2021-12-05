using SOnB.Model;
using SOnBServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SOnB.ServerNet
{
    interface ErrorBehaviourI
    {
        void react(ClientThreadModelInfo client, String message, MainWindow mainWindow, List<ClientThreadModelInfo> clients);
    }
}
