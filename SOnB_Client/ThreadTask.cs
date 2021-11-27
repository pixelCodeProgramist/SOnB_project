using Connection;
using System;

namespace SOnB.Client
{
    public  class ThreadTask
    {
        TcpConnection tcpConnection;
        ResponseMessage responseMessage;

       public ThreadTask() {
            tcpConnection = new TcpConnection();
            responseMessage = new ResponseMessage();
        }

       public void DoWork() {
            if (tcpConnection.Connect()){
                Console.WriteLine("Connection");
                responseMessage = tcpConnection.ReceiveMessage();
                Console.WriteLine(responseMessage.Message);
                if (CRCAlgorithm.ComputeCRC(responseMessage.Message))
                    tcpConnection.Send("CRC Passed");
                else
                    tcpConnection.Send("CRC Error");
            }
            else
                Console.WriteLine("Connection error");
        }
    }
}
