using Connection;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace SOnB.Client
{
    public class ThreadTask
    {
        private TcpConnection tcpConnection;
        private ResponseMessage responseMessage;
        private List<ResponseMessage> responseMessagesList;

        public ThreadTask()
        {
            tcpConnection = new TcpConnection();
            responseMessage = new ResponseMessage();
            responseMessagesList = new List<ResponseMessage>();
        }

        public void DoWork(int port)
        {
            if (tcpConnection.Connect(port))
            {
                Console.WriteLine("Connected to port " + port);

                while (true)
                {
                    if ((responseMessage = tcpConnection.ReceiveMessage()) == null)
                        break;
                    Console.WriteLine(Thread.CurrentThread.Name + " " + responseMessage.Message);
                    if (responseMessage.Message.Equals("Connection error"))
                        ChangeServer();

                    responseMessagesList.Add(responseMessage);
                    if (responseMessagesList.Count > 1)
                    {
                        if (responseMessagesList[responseMessagesList.Count - 2].Message == responseMessagesList[responseMessagesList.Count - 1].Message)
                        {
                            if (responseMessagesList.Count > 4)
                                ChangeServer();
                        }
                        else
                        {
                            responseMessagesList.RemoveAt(0);
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

        private void ChangeServer()
        {
            Thread.Sleep(200);
            tcpConnection.GetSocket().Close();
            DoWork(8001);
        }

        private void SendResponse()
        {
            if (CRCAlgorithm.IsCrcCorect(responseMessage.Message))
                tcpConnection.Send(Thread.CurrentThread.Name + ": CRC Passed");
            else
                tcpConnection.Send(Thread.CurrentThread.Name + ": CRC Error");
        }
    }
}
