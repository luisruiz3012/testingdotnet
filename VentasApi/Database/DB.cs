using Microsoft.Data.SqlClient;
using System.Data.SqlClient;

public class DB
{
    private readonly string _connectionString;
    
    public DB()
    {
        _connectionString = @"Data Source=localhost;Initial Catalog=master;User ID=sa;Password=Password12345;TrustServerCertificate=True";
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public void TestConnection()
    {
        using(var conn = GetConnection())
        {
            conn.Open();
            Console.WriteLine("Connection Open!");
        }
    }
}