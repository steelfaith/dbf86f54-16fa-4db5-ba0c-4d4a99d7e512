using System;

namespace Common.Messages
{
    /// <summary>
    /// Any message that needs to go to a specific instance needs to be routed again, im wondering if I should already 
    /// get a message bus involved here
    /// </summary>
    [Serializable]
    public class InstanceMessage : Message
    {
        public override OperationType OperationType { get; }
        public override OperationCode OperationCode { get; }
        public override int ClientId { get; set; }
        public Guid InstanceId { get; set; }
    }
}