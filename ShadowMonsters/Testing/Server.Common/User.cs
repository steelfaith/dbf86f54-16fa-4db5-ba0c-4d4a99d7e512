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
        public int ClientId { get; }
        public IClientConnection ClientConnection { get; }
        public Character ActiveCharacter { get; set; }
        public User(int clientId, IClientConnection clientConnection)
        {
            ClientId = clientId;
            ClientConnection = clientConnection;
        }
    }
}