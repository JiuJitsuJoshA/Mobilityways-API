using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MobilitywayAPI.Shared;
using MobilitywaysAPI.API.Endpoints;
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

app.MapUserEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
