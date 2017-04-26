using System;

namespace Common.Messages.Requests
{
    [Serializable]
    public class CreateBattleInstanceRequest : Message
    {
        public override OperationType OperationType => OperationType.Request;
        public override OperationCode OperationCode => OperationCode.CreateBattleInstanceRequest;
        public override int ClientId { get; set; }

    }
}