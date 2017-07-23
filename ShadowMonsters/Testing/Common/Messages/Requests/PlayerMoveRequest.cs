using System;

namespace Common.Messages.Requests
{
    [Serializable]
    public class PlayerMoveRequest : Message
    {
        public override OperationType OperationType => OperationType.Request;
        public override OperationCode OperationCode => OperationCode.PlayerMoveRequest;
        public override int ClientId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Forward { get; set; }

        public PlayerMoveRequest(Vector3 position, Vector3 forward)
        {
            Position = position;
            Forward = forward;
        }
    }
}