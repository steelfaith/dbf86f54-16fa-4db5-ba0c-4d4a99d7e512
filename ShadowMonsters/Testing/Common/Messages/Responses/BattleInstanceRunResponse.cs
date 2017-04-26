using Common;

namespace Common.Messages.Responses
{
    public class BattleInstanceRunResponse : Message
    {
        public override OperationType OperationType => OperationType.Response;
        public override OperationCode OperationCode => OperationCode.BattleInstanceRunResponse;
        public override int ClientId { get; set; }
        public bool Successful { get; set; }
    }
}