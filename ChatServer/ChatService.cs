using ChatInterface;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
       public class ChatService : IChatService
    {
        Program program = new Program();
        private ConcurrentDictionary<string, ConnectedClient> _connectedClients = new ConcurrentDictionary<string, ConnectedClient>();
        bool tooManyClients = false;
        public int Login(string userName)
        {
            //if anyone is logged with the same name
            foreach (var client in _connectedClients)
            {
                if (client.Key.ToLower() == userName.ToLower())
                {
                    return 1;
                }
            }

            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            ConnectedClient newClient = new ConnectedClient();
            newClient.connection = establishedUserConnection;
            newClient.UserName = userName;

            if (_connectedClients.Count == 10)
            {
                return 2;
            }
            else
            {
                _connectedClients.TryAdd(userName, newClient);
            }
            return 0;
        }

        public void SendMessageToAll(string message, string userName)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Key.ToLower() != userName.ToLower())
                {
                    client.Value.connection.GetMessage(message, userName);
                }
            }
        }
        public void Disconnect(string userName) 
        {
            ((IDictionary)_connectedClients).Remove(userName);
            if (_connectedClients.Count == 0)
            {
                ServiceHost host = new ServiceHost(this);
                host.Close();
            }
        }
    }
}
