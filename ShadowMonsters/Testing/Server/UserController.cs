

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Common.Interfaces.Network;
using Common.Messages;
using Microsoft.Practices.Unity;
using NLog;
using Server.Common;
using Server.Common.Interfaces;

namespace Server
{
    public class UserController : IUserController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ConcurrentDictionary<int, User> _usersByClientId = new ConcurrentDictionary<int, User>();
        private readonly IUsersStorageProvider _usersStorageProvider;
        private readonly IConnectionManager _connectionManager;

        [InjectionConstructor]
        public UserController(IConnectionManager connectionManager, IUsersStorageProvider usersStorageProvider)
        {
            _connectionManager = connectionManager;
            _usersStorageProvider = usersStorageProvider;
        }

        public void AddUser(User user)
        {
            if (!_usersByClientId.ContainsKey(user.Id))
                _usersByClientId[user.Id] = user;
        }

        public bool TryGetUserByClientId(int clientId, out User user)
        {
            if (_usersByClientId.TryGetValue(clientId, out user))
                return true;

            Logger.Warn($"Failed to find user {clientId}");
            return false;
        }

        public bool Send(int clientId, Message message)
        {
            try
            {
                User user;
                if (!TryGetUserByClientId(clientId, out user))
                    return false;

                IClientConnection clientConnection;
                if (_connectionManager.TryGetClientConnection(user.ConnectionId, out clientConnection))
                    clientConnection.Send(message);
                else
                {
                    Logger.Warn($"User {message.ClientId} is not connected. ");//not sure if we want to remove a user yet, if they reconnect maybe we should persist their data for the future?
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
            
        }

        /// <summary>
        /// this seems wrong it shouldnt be on the user controller, but lets fix that later
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IList<Character> GetCharacters(int accountId)
        {
            var task = _usersStorageProvider.GetCharacters(accountId);

            return task.Result;
        }
    }
}
