using Bogus;
using FeatureLabs.DatabaseTesting.Database;
using FeatureLabs.DatabaseTesting.Repositories;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using RestSharp;

namespace FeatureLabs.DatabaseTesting.FunctionalTests;

[Collection("Tests")]
public class UsersTests(ContainerizedApplicationFactory factory)
{
    [Fact]
    public async Task ShouldGetAllUsers()
    {
        // Arrange
        await using var scope = factory.Services.CreateAsyncScope();

        var repository = scope.ServiceProvider.GetRequiredService<UsersRepository>();

        var connection = new SqlConnection("Server=localhost;Database=FeatureLabs;User Id=sa;Password=Password1;TrustServerCertificate=True;");
        await connection.OpenAsync();

        var reSpawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            SchemasToInclude =
            [
                "dbo"
            ]
        });

        try
        {
            var now = DateTime.UtcNow;

            var faker = new Faker<UserEntity>();
            faker.UseSeed(1234);
            faker.RuleFor(u => u.Id, f => f.Random.Guid().ToString());
            faker.RuleFor(u => u.Name, f => f.Name.FullName());
            faker.RuleFor(u => u.Email, f => f.Internet.Email());
            faker.RuleFor(u => u.CreatedAt, f => now);
            faker.RuleFor(u => u.UpdatedAt, f => now);

            foreach (var _ in Enumerable.Range(1, 100))
            {
                await repository.AddUserAsync(faker.Generate());
            }

            // Act
            var request = new RestRequest("api");
            var client = new RestClient(factory.CreateClient());

            var response = await client.ExecuteGetAsync<UserEntity[]>(request);

            // Assert
            response.IsSuccessful.Should().BeTrue();
            await Verify(response.Data);
        }
        finally
        {
            await reSpawner.ResetAsync(connection);
        }
    }
}
