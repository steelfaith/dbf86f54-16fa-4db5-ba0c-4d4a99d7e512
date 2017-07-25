using System;
using Common.Interfaces.Network;
using Common.Messages;
using Common.Networking;
using Server.Common.Interfaces;

namespace Server.Common
{
    public class User
    {
        public Guid? RegionInstanceId { get; set; }
        public Guid? BattleInstanceId { get; set; }
        public int Id { get; }
        public Guid ConnectionId { get; }
        public User(int id, Guid connectionId)
        {
            Id = id;
            ConnectionId = connectionId;
        }
    }
}