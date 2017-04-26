using System;
using Common;

namespace Common.Messages.Requests
{
    [Serializable]
    public class ConnectRequest : Message
    {
        public override OperationType OperationType => OperationType.Request;
        public override OperationCode OperationCode => OperationCode.ConnectRequest;
        public override int ClientId { get; set; }
        public bool AcceptChatMessages { get; set; }//this is just an example to make sure serialization worked

        public ConnectRequest(int clientId)
        {
            ClientId = clientId;
        }

    }
}