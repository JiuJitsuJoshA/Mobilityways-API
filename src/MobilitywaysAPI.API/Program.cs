using MobilitywaysAPI.Infrastructure.Persistence.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlPersistence();
builder.Services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.Load("MobilitywaysAPI.Application")));

var app = builder.Build();

app.Run();
