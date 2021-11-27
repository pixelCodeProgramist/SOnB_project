using SOnB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        ObservableCollection<ClientThreadModelInfo> threadModelInfos;
        public MainWindow()
        {
            InitializeComponent();
            this.threadModelInfos = new ObservableCollection<ClientThreadModelInfo>();
            this.threadModelInfos.Add(new ClientThreadModelInfo("1"));

            ClientThreadListView.ItemsSource = this.threadModelInfos;
        }
    }
}
