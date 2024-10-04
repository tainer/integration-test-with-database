using Docker.DotNet.Models;
using Docker.DotNet;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.Common;

namespace IntegrationTests;

public class LocalDatabaseFixture : IDisposable
{
    public string ConnectionString { get; private set; }
    private string _databaseName = "TestDb";

    public LocalDatabaseFixture()
    {
        ConnectionString = "Server=(localdb)\\mssqllocaldb;Integrated Security=true;";

        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            // Carrega e executa o script de criação da estrutura
            var structureScript = File.ReadAllText(@".\assets\db_structure.sql");
            connection.Execute(structureScript);

            // Carrega e executa o script de inserção de dados
            var dataScript = File.ReadAllText(@".\assets\db_data.sql");
            connection.Execute(dataScript);
        }
    }

    public void Dispose()
    {
        using (var connection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Integrated Security=true;"))
        {
            connection.Open();
            connection.Execute($"DROP DATABASE [{_databaseName}]");
        }
    }
}