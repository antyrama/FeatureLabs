using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FeatureLabs.DatabaseTesting.Database;

public class UsersDataContext(DbContextOptions<UsersDataContext> options)
    : DbContext(options), IUsersDataContext
{
    public DbSet<UserEntity> Users { get; set; }

    public Task MigrateAsync()
    {
        return Database.MigrateAsync();
    }
}

public interface IUsersDataContext
{
    DbSet<UserEntity> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task MigrateAsync();
}

public class UserEntity
{
    [MaxLength(50)]
    public required string Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(100)]
    public required string Email { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
