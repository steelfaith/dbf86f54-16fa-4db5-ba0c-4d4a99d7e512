using System;

namespace Common.Messages.Events
{
    [Serializable]
    public class PlayerConnectedEvent : Message
    {
        public override OperationType OperationType => OperationType.Event;
        public override OperationCode OperationCode => OperationCode.PlayerConnectedEvent;
        public override int ClientId { get; set; }
        public object CharacterData { get; set; }
    }
}