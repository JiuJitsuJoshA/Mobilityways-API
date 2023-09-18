using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MobilitywayAPI.Shared;
using MobilitywaysAPI.API.Services;
using MobilitywaysAPI.Application.Interfaces;
using MobilitywaysAPI.Application.Result;
using MobilitywaysAPI.Infrastructure.Configurations;
using MobilitywaysAPI.Infrastructure.Persistence.DependencyInjection;
using MobilitywaysAPI.Infrastructure.Services;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(_ =>
{
    _.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlPersistence();
builder.Services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.Load("MobilitywaysAPI.Application")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/user/CreateUser", async (UserDto user, IUserService userService) =>
{
    var result = await userService.CreateUserAsync(user);

    if (!result.IsSuccess)
    {
        return Results.BadRequest(result.Message);
    }

    return Results.Created("/api/user/CreateUser", result.Message);
});

app.MapPost("api/user/GetJWTToken", async (UserLoginDto userLogin, IUserService userService) =>
{
    var result = await userService.GetJwtTokenAsync(userLogin);

    if (!result.IsSuccess)
    {
        return Results.BadRequest(result.Message);
    }

    return Results.Ok(result.Value);
});

app.MapGet("api/user/ListUsers", async (IUserService userService) =>
{
    var result = await userService.GetAllUsersAsync();

    if (!result.IsSuccess)
    {
        if (result.Type == ResultType.NotFound)
        {
            return Results.NotFound(result.Message);
        }
    }

    return Results.Ok(result.Value);
})
.RequireAuthorization();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
