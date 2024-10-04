using Dapper;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Data.SqlClient;

namespace IntegrationTests;

public class DatabaseWithDockerDotnetFixture : IDisposable
{
    private readonly DockerClient _dockerClient;
    private CreateContainerResponse _container;
    private string _containerId;

    public string ConnectionString { get; private set; }

    public DatabaseWithDockerDotnetFixture()
    {
        _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();

        CreateAndStartContainer().Wait();

        ApplySqlScript(@".\assets\db_structure.sql");

        ApplySqlScript(@".\assets\db_data.sql");
    }

    private async System.Threading.Tasks.Task CreateAndStartContainer()
    {
        var containerConfig = new CreateContainerParameters
        {
            Name = "sqlserver-for-unittest",
            Image = "mcr.microsoft.com/mssql/server:2019-latest",
            Env = new List<string> { "ACCEPT_EULA=Y", "SA_PASSWORD=YourStrong@Passw0rd" },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    { "1433/tcp", new List<PortBinding> { new PortBinding { HostPort = "1433" } } }
                }
            }
        };

        var response = await _dockerClient.Containers.CreateContainerAsync(containerConfig);
        _containerId = response.ID;

        await _dockerClient.Containers.StartContainerAsync(_containerId, null);

        // Aguardar o container SQL Server inicializar
        Thread.Sleep(20000);

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
        if (!string.IsNullOrEmpty(_containerId))
        {
            _dockerClient.Containers.StopContainerAsync(_containerId, new ContainerStopParameters()).Wait();
            _dockerClient.Containers.RemoveContainerAsync(_containerId, new ContainerRemoveParameters()).Wait();
        }

        _dockerClient.Dispose();
    }
}