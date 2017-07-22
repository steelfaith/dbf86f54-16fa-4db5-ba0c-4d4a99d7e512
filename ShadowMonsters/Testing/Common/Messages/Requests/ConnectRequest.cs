using System;
using Common;

namespace Common.Messages.Requests
{
    /// <summary>
    /// For now the server will accept all incoming connection attempts,
    /// will need to replace this with some security in the future
    /// </summary>
    [Serializable]
    public class ConnectRequest : Message
    {
        public override OperationType OperationType => OperationType.Request;
        public override OperationCode OperationCode => OperationCode.ConnectRequest;
        public override int ClientId { get; set; }

    }
}