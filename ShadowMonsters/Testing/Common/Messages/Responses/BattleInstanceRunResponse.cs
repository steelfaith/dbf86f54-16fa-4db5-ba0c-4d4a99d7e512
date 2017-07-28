using System;
using Common;

namespace Common.Messages.Responses
{
    [Serializable]
    public class BattleInstanceRunResponse : InstanceMessage
    {
        public override OperationType OperationType => OperationType.Response;
        public override OperationCode OperationCode => OperationCode.BattleInstanceRunResponse;
        public override int ClientId { get; set; }
        public bool Successful { get; set; }
    }
}