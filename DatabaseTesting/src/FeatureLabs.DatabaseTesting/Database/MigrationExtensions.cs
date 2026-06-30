namespace FeatureLabs.DatabaseTesting.Database;

public static class MigrationExtensions
{
    public static async Task EnsureDatabaseMigrationsAsync(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<UsersDataContext>();
        await context.MigrateAsync();
    }

}
