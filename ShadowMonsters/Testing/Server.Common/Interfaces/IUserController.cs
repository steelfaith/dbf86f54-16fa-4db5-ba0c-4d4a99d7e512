using System;
using System.Collections.Generic;

namespace Server.Common.Interfaces
{
    public interface IUserController
    {
        void AddUser(User user);
        //User GetUserByConnectionId(Guid connectionId);
        User GetUserByClientId(int clientId);
        IList<Character> GetCharacters(int accountId);
        //void SubscribeToServerInstance(int clientId, Guid instanceId);
        //void SubscribeToServerInstance(Guid connectionId, Guid instanceId);
        //List<User> GetUsersByRegion(Guid instanceId);
    }
}