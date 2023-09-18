using MobilitywayAPI.Shared;
using MobilitywaysAPI.API.Services;
using MobilitywaysAPI.Application.Interfaces;
using MobilitywaysAPI.Infrastructure.Persistence.DependencyInjection;
using MobilitywaysAPI.Infrastructure.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlPersistence();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.Load("MobilitywaysAPI.Application")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/user/CreateUser", async (UserDto user, IUserService userService) =>
{
    var result = await userService.CreateUser(user);

    if (!result.IsSuccess)
    {
        return Results.BadRequest(result.Message);
    }

    return Results.Created("/api/user/CreateUser", result.Message);
});

app.MapPost("api/user/GetJWTToken", async (UserLoginDto userLogin, IUserService userService) => await userService.GetJwtToken(userLogin));

app.MapGet("api/user/ListUsers", async (IUserService userService) => await userService.GetAllUsers());

app.Run();
