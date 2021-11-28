using SOnB.Business;
using SOnB.Model;
using SOnBServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private Thread communicationThread;
        private CRCMessageLogic crcMessageLogic;
        private Server server;
        String port;
        ObservableCollection<ClientThreadModelInfo> threadModelInfos;
        public MainWindow()
        {
            InitializeComponent();
            this.port = "";
            this.communicationThread = new Thread(StartServer);
            this.communicationThread.IsBackground = true;
            this.communicationThread.Start();
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

        public void UpdateLogs(String message)
        {
            this.Dispatcher.Invoke(() =>
            {
                LogTextBox.Text += message + "\n";
            });
        }

        private void StartServer()
        {
            this.server = new Server(new string[] {"8000" });
            this.Dispatcher.Invoke(() =>
            {
                ServerPortLabel.Content += " " + server.GetPort();
            });
            server.Start(this);
        }
        private void DataToCRC_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.crcMessageLogic = new CRCMessageLogic(DataToCRC.Text);
            
            if (3 < DataToCRC.Text.Length) {
                
                if (Regex.IsMatch(DataToCRC.Text, @"^[0-1]+$"))
                {
                    ErrorInfoDataToCRC.Text = "";
                    MessageBox.Text = this.crcMessageLogic.getMessage();
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

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            this.server.sendMessageToAllClients(MessageBox.Text, threadModelInfos);
        }

    }
}
