using Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOnB.Client
{
    public  class ThreadTask
    {
        TcpConnection tcpConnection;
        ResponseMessage responseMessage;
        CRCAlgorithm crcAlgorithm;

       public ThreadTask() {
            tcpConnection = new TcpConnection();
            responseMessage = new ResponseMessage();
            crcAlgorithm = new CRCAlgorithm();
        }

       public void doWork() {
            if (tcpConnection.connect()){
                Console.WriteLine("Connection");
                responseMessage = tcpConnection.ReceiveMessage();
                if (crcAlgorithm.computeCRC(responseMessage.Message))
                    tcpConnection.Send("CRC Passed");
                else
                    tcpConnection.Send("CRC Error");
            }
            else {
                Console.WriteLine("Connection error");
            }
        }
    }
}
