using System;
using System.Collections.Generic;
using Common.Messages;

namespace Server.Common.Interfaces
{
    public interface IUserController
    {
        void AddUser(User user);
        IList<Character> GetCharacters(int accountId);
        bool Send(int clientId, Message message);
        bool TryGetUserByClientId(int clientId, out User user);
    }
}