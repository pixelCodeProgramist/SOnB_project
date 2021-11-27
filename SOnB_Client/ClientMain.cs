using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SOnB.Client
{
    class ClientMain
    {
        static void Main(string[] args)
        {
            ThreadTask threadTask = new ThreadTask();
            Thread[] theThreads = new Thread[9];
            for (int counter = 0; counter < 9; counter++)
            {
                theThreads[counter] = new Thread(new ThreadStart(threadTask.doWork));
                theThreads[counter].IsBackground = true;
                theThreads[counter].Start();
            }
            Thread.CurrentThread.Join();
            Console.ReadLine();
        }
    }
}
