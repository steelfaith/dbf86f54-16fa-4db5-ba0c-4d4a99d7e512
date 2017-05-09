using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        //private readonly ConcurrentDictionary<Guid, User> _usersByConnectionId = new ConcurrentDictionary<Guid, User>();
        //private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<int, User>> _usersByRegion = new ConcurrentDictionary<Guid, ConcurrentDictionary<int, User>>();
        private readonly IUsersStorageProvider _usersStorageProvider;

        [InjectionConstructor]
        public UserController(IUsersStorageProvider usersStorageProvider)
        {
            _usersStorageProvider = usersStorageProvider;
        }

        public void AddUser(User user)
        {
            if(!_usersByClientId.ContainsKey(user.ClientId))
                _usersByClientId[user.ClientId] = user;
            //if(!_usersByConnectionId.ContainsKey(user.ConnectionId))
            //    _usersByConnectionId[user.ConnectionId] = user;
        }

        //public User GetUserByConnectionId(Guid connectionId)
        //{
        //    User user;
        //    _usersByConnectionId.TryGetValue(connectionId, out user);

        //    return user;
        //}

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

        //public void SubscribeToServerInstance(int clientId, Guid instanceId)
        //{
        //    User user;
        //    if (_usersByClientId.TryGetValue(clientId, out user))
        //        SubscribeToServerInstance(user, instanceId);
        //}

        //public void SubscribeToServerInstance(Guid connectionId, Guid instanceId)
        //{
        //    User user;
        //    if (_usersByConnectionId.TryGetValue(connectionId, out user))
        //        SubscribeToServerInstance(user, instanceId);
        //}

        //private void SubscribeToServerInstance(User user, Guid instanceId)
        //{
        //    ConcurrentDictionary<int, User> users;
        //    if (_usersByRegion.TryGetValue(instanceId, out users))
        //    {
        //        users[user.ClientId] = user;
        //        return;
        //    }

        //    var newRegionDictionary = new ConcurrentDictionary<int, User> {[user.ClientId] = user};
        //    _usersByRegion[instanceId] = newRegionDictionary;
        //}

        //public void UnsubscribeFromServerInstance(int clientId, Guid instanceId)
        //{
        //    User user;
        //    if (_usersByClientId.TryGetValue(clientId, out user))
        //        UnsubscribeFromServerInstance(user, instanceId);
        //}

        //public void UnsubscribeFromServerInstance(Guid connectionId, Guid instanceId)
        //{
        //    User user;
        //    if (_usersByConnectionId.TryGetValue(connectionId, out user))
        //        UnsubscribeFromServerInstance(user, instanceId);
        //}

        //private void UnsubscribeFromServerInstance(User user, Guid instanceId)
        //{
        //    ConcurrentDictionary<int, User> users;
        //    if (_usersByRegion.TryGetValue(instanceId, out users))
        //    {
        //        User removedUser;
        //        users.TryRemove(user.ClientId, out removedUser);
        //    }
        //}

        //public List<User> GetUsersByRegion(Guid instanceId)
        //{
        //    ConcurrentDictionary<int, User> usersByRegion;
        //    if (_usersByRegion.TryGetValue(instanceId, out usersByRegion))
        //        return usersByRegion.Values.ToList();

        //    return null;
        //}
    }
}
