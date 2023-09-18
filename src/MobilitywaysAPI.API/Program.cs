using MobilitywayAPI.Shared;
using MobilitywaysAPI.API.Services;
using MobilitywaysAPI.Infrastructure.Persistence.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlPersistence();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.Load("MobilitywaysAPI.Application")));

var app = builder.Build();

app.MapPost("/api/users/createuser", async (UserDto user, IUserService userService) => await userService.CreateUser(user));

app.Run();
