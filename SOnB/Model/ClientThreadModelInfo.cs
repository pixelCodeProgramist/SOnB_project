using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOnB.Model
{
    public class ClientThreadModelInfo
    {
        public ClientThreadModelInfo(string clientThreadId)
        {
            ClientThreadId = clientThreadId;
            IsBitChangeError = false;
            IsConnectionError = false;
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
