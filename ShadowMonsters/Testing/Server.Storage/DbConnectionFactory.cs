using System.Data;
using System.Data.SqlClient;

namespace Server.Storage
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private const string _connectionString = @"Server=localhost\SQLEXPRESS;Initial Catalog=ShadowMonsters;Persist Security Info=False;Integrated Security=SSPI;;MultipleActiveResultSets=False;";
        public string ConnectionString { get; set; }

        public IDbConnection Create()
        {
            return Create(_connectionString);
        }

        public IDbConnection Create(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}