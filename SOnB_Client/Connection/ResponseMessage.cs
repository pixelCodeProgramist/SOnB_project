using SOnB_Client.Connection;

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

        public MessageType Type
        {
            get;
            set;
        }
    }
}
