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

namespace IntegrationTests;

public class DatabaseCmdFixture : IDisposable
{
    public string ConnectionString { get; private set; }

    public DatabaseCmdFixture()
    {
        var setupScriptPath = @".\assets\setup_database.bat";
        var process = System.Diagnostics.Process.Start(setupScriptPath);
        process.WaitForExit();

        ConnectionString = "Server=localhost,1433;Database=master;User Id=sa;Password=YourStrong@Passw0rd;Encrypt=True;TrustServerCertificate=True;";
    }

    private void ApplySqlScript(string scriptPath)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            var script = File.ReadAllText(scriptPath);
            connection.Execute(script);
        }
    }

    public void Dispose()
    {
        var stopContainerScript = "docker stop sqlserver-test && docker rm sqlserver-test";
        var process = System.Diagnostics.Process.Start("cmd.exe", $"/c {stopContainerScript}");
        process.WaitForExit();
    }
}