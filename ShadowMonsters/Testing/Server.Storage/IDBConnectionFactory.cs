using System.Data;

namespace Server.Storage
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
        IDbConnection Create(string connectionString);
    }
}