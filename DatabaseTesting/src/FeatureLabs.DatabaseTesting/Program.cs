using System.Runtime.CompilerServices;
using FeatureLabs.DatabaseTesting.Database;
using FeatureLabs.DatabaseTesting.Repositories;
using FeatureLabs.DatabaseTesting.Workers;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("FeatureLabs.DatabaseTesting.FunctionalTests")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddDbContext<IUsersDataContext, UsersDataContext>((provider, contextOptionsBuilder) =>
{
    contextOptionsBuilder.UseSqlServer(builder.Configuration["ConnectionString"]);
});

services.AddTransient<UsersRepository>();

//services.AddHostedService<MigrationWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

await app.EnsureDatabaseMigrationsAsync();

app.Run();
