using Connection;
using System;
using System.Net.Sockets;
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

       public void DoWork(int port) {
            if (tcpConnection.Connect(port)){
                Console.WriteLine("Connected");

                while (true) 
                {
                    if ((responseMessage = tcpConnection.ReceiveMessage()) == null)
                        break;

                    if (responseMessage.Message.Equals("Connection error")) {
                            tcpConnection.GetSocket().Close();
                            DoWork(8001);  
                      }

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
