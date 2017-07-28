using System;
using Common;

namespace Common.Messages.Requests
{
    [Serializable]
    public class BattleInstanceRunRequest : InstanceMessage
    {
        public override OperationType OperationType => OperationType.Request;
        public override OperationCode OperationCode => OperationCode.BattleInstanceRunRequest;
        public override int ClientId { get; set; }

        public BattleInstanceRunRequest()
        {
        }
    }
}