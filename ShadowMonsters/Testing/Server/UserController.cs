using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Common;
using log4net;
using Microsoft.Practices.Unity;
using Server.Common;
using Server.Common.Interfaces;

namespace Server
{
    public class UserController : IUserController
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(UserController));
        private static readonly AsyncLogger AsyncLogger = new AsyncLogger(Logger);
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
            if (!_usersByClientId.ContainsKey(user.ClientId))
            {
                if (!_usersByClientId.TryAdd(user.ClientId, user))
                    throw new InvalidOperationException("Failed to add user to user controller.");
            }
            else
                AsyncLogger.WarnFormat($"User with Client Id {user.ClientId} was already in the collection.");

            if (!_usersByConnectionId.ContainsKey(user.TcpConnectionId))
            {
                if (!_usersByConnectionId.TryAdd(user.TcpConnectionId, user))
                    throw new InvalidOperationException("Failed to add user to user controller.");
            }
            else
                AsyncLogger.WarnFormat($"User with Connection Id {user.TcpConnectionId} was already in the collection.");


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