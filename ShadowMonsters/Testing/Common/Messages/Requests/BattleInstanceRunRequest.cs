using System;
using Common;

namespace Common.Messages.Requests
{
    public class BattleInstanceRunRequest : Message
    {
        public override OperationType OperationType => OperationType.Request;
        public override OperationCode OperationCode => OperationCode.BattleInstanceRunRequest;
        public override int ClientId { get; set; }

        public BattleInstanceRunRequest()
        {
        }
    }
}