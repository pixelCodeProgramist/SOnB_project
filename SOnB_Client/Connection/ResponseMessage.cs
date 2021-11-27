using Communication;

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
