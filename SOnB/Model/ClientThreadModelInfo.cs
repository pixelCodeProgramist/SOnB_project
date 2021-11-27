using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SOnB.Model
{
    public class ClientThreadModelInfo
    {
        public ClientThreadModelInfo(string socketId, Socket socket)
        {
            SocketId = socketId;
            Socket = socket;
            IsBitChangeError = false;
            IsConnectionError = false;
        }

        public Socket Socket
        {
            get;
            set;
        }
        public String SocketId
        {
            get;
            set;
        }

        public Boolean IsBitChangeError
        {
            get;
            set;
        }

        public Boolean IsConnectionError
        {
            get;
            set;
        }

    }
}
