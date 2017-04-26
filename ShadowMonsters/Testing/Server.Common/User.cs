using System;

namespace Server.Common
{
    public class User
    {
        public Guid? RegionInstanceId { get; set; }
        public Guid? BattleInstanceId { get; set; }
        public int ClientId { get; }
        public Guid TcpConnectionId { get; }
  
        public User(int clientId, Guid tcpConnectionId)
        {
            ClientId = clientId;
            TcpConnectionId = tcpConnectionId;
        }
    }
}