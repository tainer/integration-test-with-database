using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests;

public class DatabaseWithDockerDotnetTests : IClassFixture<DatabaseWithDockerDotnetFixture>
{
    private readonly DatabaseWithDockerDotnetFixture _fixture;

    public DatabaseWithDockerDotnetTests(DatabaseWithDockerDotnetFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void TestDatabaseConnectionWithDockerDotnet()
    {
        using var connection = new SqlConnection(_fixture.ConnectionString);
        var result = connection.Query<int>("SELECT COUNT(*) FROM usuario").Single();

        Assert.Equal(1, result);
    }
}