using System.Collections.Generic;
using System.Threading.Tasks;
using Insight.Database;
using Microsoft.Practices.Unity;
using Server.Common;
using Server.Common.Interfaces;

namespace Server.Storage.Providers
{
    public class UsersStorageProvider : IUsersStorageProvider
    {
        private readonly IDbConnectionFactory _connectionFactory;

        [InjectionConstructor]
        public UsersStorageProvider(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IList<Character>> GetCharacters(int accountId)
        {
            var input = new
            {
                accountId = accountId
            };

            Results<Character> results;
            using (var connection = _connectionFactory.Create())
                results = await connection.ExecuteProcedureAsync<Character>("Users.GetCharacters", input);

            return results.Set1;
        }
    }
}