using MobilitywaysAPI.Domain;
using MobilitywaysAPI.Domain.Common;
using MobilitywaysAPI.Infrastructure.Persistence;

namespace MobilitywaysAPI.Infrastructure;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly MobilitywaysDbContext _mobilitywaysDbContext;

    public Repository(MobilitywaysDbContext mobilitywaysDbContext)
    {
        _mobilitywaysDbContext = mobilitywaysDbContext;
    }

    public async Task AddAsync(T entity)
    {
        await _mobilitywaysDbContext.Set<T>().AddAsync(entity);
    }

    public IQueryable<T> GetAll()
    {
        return _mobilitywaysDbContext.Set<T>();
    }

    public async Task SaveChangesAsync()
    {
        await _mobilitywaysDbContext.SaveChangesAsync();
    }
}
