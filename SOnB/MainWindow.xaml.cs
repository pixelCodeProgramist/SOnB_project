using SOnB.Business;
using SOnB.Model;
using SOnBServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SOnB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Thread _communicationThread;
        private CRCMessageLogic _crcMessageLogic;
        private Server _server;
        String port;
        readonly ObservableCollection<ClientThreadModelInfo> threadModelInfos;
        public MainWindow()
        {
            InitializeComponent();
            this.port = "";
            this._communicationThread = new Thread(StartServer)
            {
                IsBackground = true
            };
            this._communicationThread.Start();
            SendButton.Visibility = Visibility.Hidden;
            DataToCRC.Text = "";
            MessageBox.Text = "";
            this.threadModelInfos = new ObservableCollection<ClientThreadModelInfo>();
        }

        public void UpdateListOfSockets(ClientThreadModelInfo client)
        {
            this.threadModelInfos.Add(client);
            this.Dispatcher.Invoke(() =>
            {
                ClientThreadListView.Items.Add(client);
            });
        }

        public void RemoveSocketFromList(ClientThreadModelInfo client)
        {
            this.threadModelInfos.Remove(client);
            this.Dispatcher.Invoke(() =>
            {
                ClientThreadListView.Items.Remove(client);
            });
        }

        public void UpdateLogs(String message)
        {
            this.Dispatcher.Invoke(() =>
            {
                LogTextBox.Text += message + "\n";
            });
        }

        private void StartServer()
        {
            this._server = new Server(new string[] {"8000" });
            this.Dispatcher.Invoke(() =>
            {
                ServerPortLabel.Content += " " + _server.GetPort();
            });
            _server.Start(this);
        }
        private void DataToCRC_TextChanged(object sender, TextChangedEventArgs e)
        {
            this._crcMessageLogic = new CRCMessageLogic(DataToCRC.Text);
            
            if (3 < DataToCRC.Text.Length) {
                
                if (Regex.IsMatch(DataToCRC.Text, @"^[0-1]+$"))
                {
                    ErrorInfoDataToCRC.Text = "";
                    MessageBox.Text = this._crcMessageLogic.GetMessage();
                    SendButton.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Text = "";
                    ErrorInfoDataToCRC.Text = "Dane powinny zawierać tylko znaki 0 i 1";
                    SendButton.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                MessageBox.Text = "";
                ErrorInfoDataToCRC.Text = "Długość musi być dłuższa niż 3 znaków";
                SendButton.Visibility = Visibility.Hidden;
            }
           
        }

        public String GetDataFromTextBox()
        {
            String data = "";
            this.Dispatcher.Invoke(() => 
            {
                data = this.DataToCRC.Text;
            });
            return data;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            this._server.SendMessageToAllClients(MessageBox.Text, threadModelInfos);
        }

        private void ClearLogs_Click(object sender, RoutedEventArgs e)
        {
            LogTextBox.Text = "";
        }
    }
}
