using System;

namespace Common.Messages.Events
{
    [Serializable]
    public class ShadowCreatedEvent : Message
    {
        public override OperationType OperationType => OperationType.Event;
        public override OperationCode OperationCode => OperationCode.ShadowCreatedEvent;
        public override int ClientId { get; set; }
    }
}