using FeatureLabs.DatabaseTesting.Database;
using Microsoft.EntityFrameworkCore;

namespace FeatureLabs.DatabaseTesting.Repositories;

public class UsersRepository(IUsersDataContext dataContext)
{
    public async Task<UserEntity?> GetUserByIdAsync(string id)
    {
        return await dataContext.Users.FindAsync(id);
    }
    public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
    {
        return await dataContext.Users.ToListAsync();
    }
    public async Task AddUserAsync(UserEntity user)
    {
        await dataContext.Users.AddAsync(user);
        await dataContext.SaveChangesAsync();
    }
    public async Task UpdateUserAsync(UserEntity user)
    {
        dataContext.Users.Update(user);
        await dataContext.SaveChangesAsync();
    }
    public async Task DeleteUserAsync(string id)
    {
        var user = await GetUserByIdAsync(id);
        if (user != null)
        {
            dataContext.Users.Remove(user);
            await dataContext.SaveChangesAsync();
        }
    }
}
