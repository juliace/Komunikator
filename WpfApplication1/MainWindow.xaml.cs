using System;
using System.Collections.Generic;
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
using System.Net;
using System.Net.Sockets;
using System.IO;
using ChatInterface;
using System.ServiceModel;
using Client;
using System.Globalization;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Connection {

    }

    public partial class MainWindow : Window
    {
        public static IChatService server;
        private static DuplexChannelFactory<IChatService> _channelFactory;
        
        public MainWindow()
        {
            InitializeComponent();
            _channelFactory = new DuplexChannelFactory<IChatService>(new ClientCallback(), "ChatServiceEndPoint");
            server = _channelFactory.CreateChannel();            
        }

        public void TakeMessage(string message, string userName)
        {
            DateTime TodayTime = DateTime.Now;
            string time = TodayTime.ToShortTimeString();
            displayTextBox.Text += time + " " + userName + ": " + message + "\n";
            displayTextBox.ScrollToEnd();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int returnValue = server.Login(userNameTextBox.Text);
            if (returnValue == 1)
            {
                MessageBox.Show("You are already logged in. Try again!");
            }
            else if (returnValue == 2)
            {
                MessageBox.Show("Too many users! Try again later!");
            }
            else if (returnValue == 0)
            {
                MessageBox.Show("You logged in!");
                userNameTextBox.IsEnabled = false;
                connectButton.IsEnabled = false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            server.SendMessageToAll(MessageTextBox.Text, userNameTextBox.Text);
            TakeMessage(MessageTextBox.Text, "You: ");
            MessageTextBox.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            server.Disconnect(userNameTextBox.Text);
            sendButton.IsEnabled = false;
            MessageBox.Show("You logged out!");
            Application.Current.Shutdown();
        }
    }
}
