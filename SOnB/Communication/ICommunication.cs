using Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Communication
{
    public interface ICommunication
    {
        void Process(Socket s, ResponseMessage responseMessage);
        
       
    }
}
