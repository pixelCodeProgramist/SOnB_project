using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOnB.Model
{
    public class ClientThreadModelInfo
    {
        private String _clientThreadId;
        private Boolean _isBitChangeError;
        private Boolean _isConnectionError;

        public ClientThreadModelInfo(string clientThreadId)
        {
            ClientThreadId = clientThreadId;
        }

        public String ClientThreadId
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
