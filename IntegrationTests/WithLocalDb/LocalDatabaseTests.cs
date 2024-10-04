using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests;

public class LocalDatabaseTests : IClassFixture<LocalDatabaseFixture>
{
    private readonly LocalDatabaseFixture _fixture;

    public LocalDatabaseTests(LocalDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void TestLocalDatabaseTests()
    {
        using var connection = new SqlConnection(_fixture.ConnectionString);
        var result = connection.Query<int>("SELECT COUNT(*) FROM usuario").Single();

        Assert.Equal(1, result);
    }
}