using System;
using System.Threading;

namespace SOnB.Client
{
    class ClientMain
    {
        static void Main()
        {
            ThreadTask threadTask = new ThreadTask();
            Thread[] threads = new Thread[9];
            for (int counter = 0; counter < 9; counter++)
            {
                threads[counter] = new Thread(() => new ThreadTask().DoWork())
                {
                    IsBackground = true
                };
                Thread.Sleep(200);
                threads[counter].Start();
            }
            Thread.CurrentThread.Join();
        }
    }
}
