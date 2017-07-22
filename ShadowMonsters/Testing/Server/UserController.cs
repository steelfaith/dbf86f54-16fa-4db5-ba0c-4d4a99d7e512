
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        [InjectionConstructor]
        public UserController(IUsersStorageProvider usersStorageProvider)
        {
            _usersStorageProvider = usersStorageProvider;
        }

        public void AddUser(User user)
        {
            if(!_usersByClientId.ContainsKey(user.Id))
                _usersByClientId[user.Id] = user;
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
