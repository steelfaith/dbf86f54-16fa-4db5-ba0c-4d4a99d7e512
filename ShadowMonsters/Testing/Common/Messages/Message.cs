using System;

namespace Common.Messages
{
    [Serializable]
    public abstract class Message
    {
        public abstract OperationType OperationType { get; }
        public abstract OperationCode OperationCode { get; }
        public abstract int ClientId { get; set; }
    }
}