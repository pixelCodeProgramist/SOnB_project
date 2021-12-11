using Connection;
using SOnB_Client.Connection;
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
                Console.WriteLine(Thread.CurrentThread.Name + ": connected to port " + port);

                while (true)
                {
                    responseMessage = tcpConnection.ReceiveMessage();
                    if (responseMessage == null || responseMessage.Type == MessageType.CommunicationClosed)
                        break;
                    Console.WriteLine(Thread.CurrentThread.Name + " " + responseMessage.Message);
                    if (responseMessage.Message.Equals("Connection error"))
                    {
                        responseMessagesList.Clear();
                        ChangeServer();
                        break;
                    }

                    responseMessagesList.Add(responseMessage);
                    if (IsListContainsTwoMessages())
                    {
                        if (IsLastTwoMessagesEquals())
                        {
                            if (IsServerSendsSameMessage(5))
                            {
                                responseMessagesList.Clear();
                                ChangeServer();
                                break;
                            }
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
                Console.WriteLine(Thread.CurrentThread.Name + ": connection error");
        }

        private bool IsListContainsTwoMessages()
        {
            return responseMessagesList.Count > 1;
        }

        private bool IsLastTwoMessagesEquals()
        {
            return responseMessagesList[responseMessagesList.Count - 2].Message == responseMessagesList[responseMessagesList.Count - 1].Message;
        }

        private bool IsServerSendsSameMessage(int messageCount)
        {
            return responseMessagesList.Count >= messageCount;
        }

        private void ChangeServer()
        {
            Thread.Sleep(1000);
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
