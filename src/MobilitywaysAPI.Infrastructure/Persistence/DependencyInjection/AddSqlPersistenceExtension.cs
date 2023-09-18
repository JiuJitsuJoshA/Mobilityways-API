using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobilitywaysAPI.Domain;

namespace MobilitywaysAPI.Infrastructure.Persistence.DependencyInjection;

public static class AddSqlPersistenceExtension
{
    public static IServiceCollection AddSqlPersistence(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddDbContext<MobilitywaysDbContext>(_ => _.UseInMemoryDatabase("Users"), optionsLifetime: ServiceLifetime.Transient)
                                .AddTransient(typeof(IRepository<>), typeof(Repository<>));
    }
}
