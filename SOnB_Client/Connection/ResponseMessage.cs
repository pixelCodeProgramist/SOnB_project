using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection
{
    public class ResponseMessage
    {


        public byte[] ReceivedBytes
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public ICommunication ICommunicationType
        {
            get;
            set;
        }
    }
}
