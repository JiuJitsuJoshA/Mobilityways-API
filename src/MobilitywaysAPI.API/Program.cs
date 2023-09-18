using MobilitywayAPI.Shared;
using MobilitywaysAPI.API.Services;
using MobilitywaysAPI.Infrastructure.Persistence.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlPersistence();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.Load("MobilitywaysAPI.Application")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/user/CreateUser", async (UserDto user, IUserService userService) => await userService.CreateUser(user));

app.MapPost("api/user/GetJWTToken", async (UserLoginDto userLogin, IUserService userService) => await userService.GetJwtToken(userLogin));

app.MapGet("api/user/ListUsers", async (IUserService userService) => await userService.GetAllUsers());

app.Run();
