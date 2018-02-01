using ChatInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication1;

namespace Client
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]

    public class ClientCallback : IClient
    {
        public void GetMessage(string message, string userName)
        {
            ((MainWindow)Application.Current.MainWindow).TakeMessage(message, userName); 
        }
    }
}
