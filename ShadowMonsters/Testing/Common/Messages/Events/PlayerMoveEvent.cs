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
        public Dictionary<int, PositionForwardTuple> UpdatedPositions { get; set; }

        public PlayerMoveEvent(Dictionary<int, PositionForwardTuple> udpatedPositions)
        {
            UpdatedPositions = udpatedPositions;
        }

    }
}