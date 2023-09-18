using MobilitywaysAPI.Infrastructure.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlPersistence();

var app = builder.Build();

app.Run();
