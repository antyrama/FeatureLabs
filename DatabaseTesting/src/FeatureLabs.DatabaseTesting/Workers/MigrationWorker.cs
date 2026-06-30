using FeatureLabs.DatabaseTesting.Database;

namespace FeatureLabs.DatabaseTesting.Workers;

internal sealed class MigrationWorker(IServiceScopeFactory scope) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var serviceScope = scope.CreateAsyncScope();
        var dataContext = serviceScope.ServiceProvider.GetRequiredService<IUsersDataContext>();

        await dataContext.MigrateAsync();
    }


}
