﻿using System;
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
                Thread.Sleep(100);
                threads[counter] = new Thread(() => new ThreadTask().DoWork())
                {
                    IsBackground = true,
                    Name = "Thread " + (counter + 1)
                };
                threads[counter].Start();
            }
            Thread.CurrentThread.Join();
        }
    }
}
