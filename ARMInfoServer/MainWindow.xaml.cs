using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ARMInfoServer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Server Server { get; set; } = new Server();
        public MainWindow()
        {
            InitializeComponent();
            Server.StateChanged += (state) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (state == System.ServiceModel.CommunicationState.Opened)
                    {
                        
                        Title = "Connected";
                    }
                    else
                    {
                        Title = "Disconnected";
                    }
                }));

            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Server.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Server.Stop();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ListOVD.ItemsSource = Server.OVDCollection;
        }
    }
}
