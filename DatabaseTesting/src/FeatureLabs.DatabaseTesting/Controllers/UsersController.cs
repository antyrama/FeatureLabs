using FeatureLabs.DatabaseTesting.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FeatureLabs.DatabaseTesting.Controllers;

[ApiController]
[Route("api")]
public class UsersController(UsersRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return await repository.GetAllUsersAsync() is { } users
            ? Ok(users)
            : NotFound("No users found.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return await repository.GetUserByIdAsync(id) is { } user
            ? Ok(user)
            : NotFound($"User with ID {id} not found.");
    }

}
