using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests;

public class DatabaseWithCmdConfigTests : IClassFixture<DatabaseCmdFixture>
{
    private readonly DatabaseCmdFixture _fixture;

    public DatabaseWithCmdConfigTests(DatabaseCmdFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void TestDatabaseConnectionWithCmdConfig()
    {
        using var connection = new SqlConnection(_fixture.ConnectionString);
        var result = connection.Query<int>("SELECT COUNT(*) FROM usuario").Single();

        Assert.Equal(1, result);
    }
}