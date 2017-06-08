using System;
using System.Collections.Generic;
using Common.Networking;
using Server.Common.Interfaces;

namespace Server.Common
{
    public class User
    {
        public Guid? RegionInstanceId { get; set; }
        public Guid? BattleInstanceId { get; set; }
        public int Id { get; }
        public IClientConnection ClientConnection { get; }
        public User(int id, IClientConnection clientConnection)
        {
            Id = id;
            ClientConnection = clientConnection;
        }
    }
}