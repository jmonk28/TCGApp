namespace TCGApp.Server.Tests;
using System.Data.SqlClient;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

public class DatabaseTest
{

    private readonly string _connectionString;
    public DatabaseTest()
    {
        // Load the connection string from appsettings.json
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("C:/Users/monkj/source/repos/TCGApp/TCGApp/TCGApp.Server/appsettings.json")
            .Build();
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    [Fact]
    public void TestDatabaseConnection()
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        Assert.Equal(System.Data.ConnectionState.Open, connection.State);
    }
}
