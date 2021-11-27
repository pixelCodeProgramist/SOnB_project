using Connection;
using System.Net.Sockets;

namespace Communication
{
    public interface ICommunication
    {
        void Process(Socket s, ResponseMessage responseMessage);
    }
}
