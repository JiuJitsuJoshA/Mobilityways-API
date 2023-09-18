using MobilitywaysAPI.Domain.Common;

namespace MobilitywaysAPI.Domain
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        IQueryable<T> GetAll();
        Task SaveChangesAsync();
    }
}