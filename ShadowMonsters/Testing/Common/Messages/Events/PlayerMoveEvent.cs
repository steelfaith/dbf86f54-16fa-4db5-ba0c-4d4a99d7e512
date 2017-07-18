using System;
using System.Collections.Generic;

namespace Common.Messages.Events
{
    [Serializable]
    public class PlayerMoveEvent : Message
    {
        public override OperationType OperationType => OperationType.Event;
        public override OperationCode OperationCode => OperationCode.PlayerMoveEvent;
        public override int ClientId { get; set; }
        public Dictionary<int, Vector3> UpdatedPositions { get; set; }

        public PlayerMoveEvent(Dictionary<int, Vector3> udpatedPositions)
        {
            UpdatedPositions = udpatedPositions;
        }

    }
}