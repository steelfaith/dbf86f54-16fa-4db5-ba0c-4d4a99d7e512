using System;
using Common;

namespace Common.Messages.Responses
{
    [Serializable]
    public class CreateBattleInstanceResponse : Message
    {
        public override OperationType OperationType => OperationType.Response;
        public override OperationCode OperationCode => OperationCode.CreateBattleInstanceResponse;
        public override int ClientId { get; set; }
        public Guid InstanceId { get; set; }
    }
}