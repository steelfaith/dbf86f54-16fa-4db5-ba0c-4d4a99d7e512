using System;

namespace Common.Messages.Events
{
    [Serializable]
    public class PositionForwardTuple
    {
        public Vector3 Position { get; set; }
        public Vector3 Forward { get; set; }
    }
}