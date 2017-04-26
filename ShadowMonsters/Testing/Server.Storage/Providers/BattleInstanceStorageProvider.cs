using Microsoft.Practices.Unity;
using Server.Common.Interfaces;

namespace Server.Storage.Providers
{
    public class BattleInstanceStorageProvider : IBattleInstanceStorageProvider
    {
        private readonly IDbConnectionFactory _connectionFactory;

        [InjectionConstructor]
        public BattleInstanceStorageProvider(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void LogBattleInstanceCreation()
        {
            
        }
    }
}