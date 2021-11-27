using SOnBCommunication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SOnBServer
{
    class Server
    {
        private int listnerPort;
        private int counter = 0;
        private const int MAX_CLIENTS = 10;
        Dictionary<string, Socket> clients;
        
        public Server(string[] args)
        {
            this.listnerPort = SetPort(args[0]);
            clients = new Dictionary<string, Socket>();
        }

        public String GetPort()
        {
            return listnerPort.ToString();
        }

        private int SetPort(string text)
        {
            int defaultPort = 8000;
            try
            {
                return int.Parse(text);
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine(ex.Message);
                return defaultPort;
            }
        }

        public void Start()
        {
            Console.WriteLine("Starting TCP listener...");

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, listnerPort);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            socket.Bind(endpoint);
            socket.Listen(MAX_CLIENTS);
            Communication.server = this;

            Console.WriteLine(">> Server has been started");

            while (true)
            {
                counter++;
                Socket client = socket.Accept();
                clients.Add(client.RemoteEndPoint.ToString(), client);
                Console.WriteLine(" >> Thread No:" + Convert.ToString(counter) + " started");
            }
        }

        public void removeClient(string client)
        {
            clients.Remove(client);
        }

        public void sendMessage(Socket client, string message)
        {
            Byte[] bytes = Encoding.UTF8.GetBytes(message);
            client.Send(bytes, bytes.Length, 0);
        }

        public void sendMessageToAllClients(string message)
        {
            try
            {
                for (int i = 0; i < clients.Count; i++)
                    sendMessage(clients.ElementAt(i).Value, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
