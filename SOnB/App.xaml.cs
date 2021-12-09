using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SOnB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            if(e.Args.Length==0)
            {
                MainWindow mainWindow = new MainWindow("8000", "Server");
                mainWindow.Show();
            }
            else
            {
                if(e.Args.Length == 1)
                {
                    string port = (checkPort(e.Args[0])) ? e.Args[0] : "8000";
                    MainWindow mainWindow = new MainWindow(port, "Server");
                    mainWindow.Show();
                }
                if (e.Args.Length == 2)
                {
                    string port1 = (checkPort(e.Args[0])) ? e.Args[0] : "8000";
                    string port2 = (checkPort(e.Args[1])) ? e.Args[1] : "8001";
                    MainWindow mainWindow = new MainWindow(port1, "Server");
                    mainWindow.Show();
                    MainWindow mainWindow2 = new MainWindow(port2, "Slave server");
                    mainWindow2.Show();
                }

               
            }
           
        }

        bool checkPort(string port)
        {
            return port.All(char.IsDigit); ;
        }
    }
}
