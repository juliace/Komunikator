using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatInterface
{
    [ServiceContract(CallbackContract=typeof(IClient))]
    public interface IChatService
    {
        [OperationContract]
       int Login(string userName);
        [OperationContract]
       void SendMessageToAll(string message, string userName);
        [OperationContract]
        void Disconnect(string userName);
        
    }
}
