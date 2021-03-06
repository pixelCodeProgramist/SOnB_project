using SOnB.Business;
using SOnB.Model;
using SOnBServer;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

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
        private bool CheckBoxEnabled;
        
        public MainWindow(string port, string title) : this(port)
        {
            this.Title = title;
            this.CheckBoxEnabled = title == "Slave server" ? false : true;
        }
        public MainWindow(string port)
        {
            InitializeComponent();
            this.port = port;
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
            client.CheckBoxEnabled = this.CheckBoxEnabled;
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

        public bool GetBitChangeError(int i)
        {
            bool error = false;
            this.Dispatcher.Invoke(() =>
            {
                error = threadModelInfos[i].IsBitChangeError;
            });
            return error;
        }

        public bool GetRepeatAnswearError(int i)
        {
            bool error = false;
            this.Dispatcher.Invoke(() =>
            {
                error = threadModelInfos[i].IsRepeatAnswearError;
            });
            return error;
        }

        public bool GetConnectionError(int i)
        {
            bool error = false;
            this.Dispatcher.Invoke(() =>
            {
                error = threadModelInfos[i].IsConnectionError;
            });
            return error;
        }

        private void StartServer()
        {
            this._server = new Server(this.port);
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
            string message = MessageBox.Text;
            Thread senderThread = new Thread(() => _server.SendMessageToAllClients(message, threadModelInfos))
            {
                IsBackground = true
            };
            senderThread.Start();
        }


        private void LogTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogTextBox.Focus();
            LogTextBox.CaretIndex = LogTextBox.Text.Length;
            LogTextBox.ScrollToEnd();
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            ClientThreadModelInfo client = checkBox.DataContext as ClientThreadModelInfo;
            if (checkBox.Name == "BitChangeCheckBox")
            {
                client.IsConnectionError = false;
                client.IsRepeatAnswearError = false;
            }

            if (checkBox.Name == "ConnectionErrorCheckBox")
            {
                client.IsBitChangeError = false;
                client.IsRepeatAnswearError = false;
            }

            if (checkBox.Name == "RepeatAnswearCheckBox")
            {
                client.IsBitChangeError = false;
                client.IsConnectionError = false;
            }
            ClientThreadListView.Items.Refresh();
        }
        private void ClearLogs_Click(object sender, RoutedEventArgs e)
        {
            LogTextBox.Text = "";

        }
    }
}
