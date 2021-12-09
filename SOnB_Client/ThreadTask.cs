using Connection;
using System;
using System.Threading;

namespace SOnB.Client
{
    public  class ThreadTask
    {
        private TcpConnection tcpConnection;
        private ResponseMessage responseMessage;

       public ThreadTask() {
            tcpConnection = new TcpConnection();
            responseMessage = new ResponseMessage();
        }

       public void DoWork() {
            if (tcpConnection.Connect()){
                Console.WriteLine("Connection");

                while (true) 
                {
                    if ((responseMessage = tcpConnection.ReceiveMessage()) == null)
                        break;

                    Console.WriteLine(Thread.CurrentThread.Name + " " + responseMessage.Message);
                    if (CRCAlgorithm.IsCrcCorect(responseMessage.Message))
                        tcpConnection.Send(Thread.CurrentThread.Name + ": CRC Passed");
                    else
                        tcpConnection.Send(Thread.CurrentThread.Name + ": CRC Error");
                }               
            }
            else
                Console.WriteLine("Connection error");
        }
    }
}
