using System;
using System.Collections.Generic;
using System.Net.Sockets;


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
            IsRepeatAnswearError = false;
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

        public Boolean IsRepeatAnswearError
        {
            get;
            set;
        }

        public Boolean CheckBoxEnabled
        {
            get;
            set;
        }
    }
}
