using Microsoft.Data.SqlClient;

namespace ClientesLibrary.Database
{
    internal class DB
    {
        private readonly string _connectionString;

        public DB()
        {
            _connectionString = @"Data Source=localhost;Initial Catalog=master;User ID=sa;Password=Password12345;TrustServerCertificate=True";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(this._connectionString);
        }
    }
}
