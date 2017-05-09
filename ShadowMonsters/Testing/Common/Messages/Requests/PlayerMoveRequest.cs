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

        public PlayerMoveRequest(int clientId, Vector3 position)
        {
            ClientId = clientId;
            Position = position;
        }
    }
}