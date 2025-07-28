using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MsSql;

namespace FeatureLabs.DatabaseTesting.FunctionalTests;

public class ContainerizedApplicationFactory : WebApplicationFactory<IHostMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithPassword("Password1")
        .WithPortBinding(1433, 1433)
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}
