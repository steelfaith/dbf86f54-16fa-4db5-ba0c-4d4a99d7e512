using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using log4net;
using Microsoft.Practices.Unity;
using Server.Common;
using Server.Common.Interfaces;

namespace Server
{
    public class UserController : IUserController
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(UserController));

        private readonly ConcurrentDictionary<int, User> _usersByClientId = new ConcurrentDictionary<int, User>();
        private readonly ConcurrentDictionary<Guid, User> _usersByConnectionId = new ConcurrentDictionary<Guid, User>();
        private readonly IUsersStorageProvider _usersStorageProvider;

        [InjectionConstructor]
        public UserController(IUsersStorageProvider usersStorageProvider)
        {
            _usersStorageProvider = usersStorageProvider;
        }

        /// <summary>
        /// clean this up later right now I just want constant performance for user lookups
        /// this should all probably be try gets that wrap the underlying dictionaries
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            if(!_usersByClientId.TryAdd(user.ClientId, user))
                _logger.Warn("Failed to add user to user controller.");
            if(!_usersByConnectionId.TryAdd(user.TcpConnectionId, user))
                _logger.Warn("Failed to add user to user controller.");
        }

        public User GetUserByConnectionId(Guid connectionId)
        {
            User user;
            _usersByConnectionId.TryGetValue(connectionId, out user);

            return user;
        }

        public User GetUserByClientId(int clientId)
        {
            User user;
            _usersByClientId.TryGetValue(clientId, out user);

            return user;
        }

        public IList<Character> GetCharacters(int accountId)
        {
            var task = _usersStorageProvider.GetCharacters(accountId);

            return task.Result;
        }
    }
}