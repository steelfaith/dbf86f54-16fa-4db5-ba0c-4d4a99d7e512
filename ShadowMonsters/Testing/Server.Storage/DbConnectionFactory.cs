using System.Data;
using System.Data.SqlClient;

namespace Server.Storage
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private const string _connectionString = "Server=tcp:testsm.database.windows.net,1433;Initial Catalog=TestDB;Persist Security Info=False;User ID=BMAN;Password=Testpassword!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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