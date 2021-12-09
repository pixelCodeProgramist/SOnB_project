using Connection;
using System;
using System.Collections.Generic;
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
            List<ResponseMessage> responseMessagesList = new List<ResponseMessage>();
           
            if (tcpConnection.Connect(port)){
                Console.WriteLine("Connected to port "+ port);
                
                while (true) 
                {
                    if ((responseMessage = tcpConnection.ReceiveMessage()) == null)
                        break;

                    if (responseMessage.Message.Equals("Connection error"))
                        ChangeServer();
                                   
                    responseMessagesList.Add(responseMessage);
                    if (responseMessagesList.Count > 1) {
                        if (responseMessagesList[responseMessagesList.Count - 2].Message == responseMessagesList[responseMessagesList.Count - 1].Message)
                        {
                            if (responseMessagesList.Count > 9)
                                ChangeServer();
                        }
                        else
                        {
                            responseMessagesList.Clear();
                            SendResponse();
                        }
                    }
                    else
                        SendResponse();
                }               
            }
            else
                Console.WriteLine("Connection error");
        }

        private void ChangeServer() {
            tcpConnection.GetSocket().Close();
            DoWork(8001);
        }

        private void SendResponse()
        {
            Console.WriteLine(Thread.CurrentThread.Name + " " + responseMessage.Message);
            if (CRCAlgorithm.IsCrcCorect(responseMessage.Message))
                tcpConnection.Send(Thread.CurrentThread.Name + ": CRC Passed");
            else
                tcpConnection.Send(Thread.CurrentThread.Name + ": CRC Error");
        }
    }
}
