using Microsoft.EntityFrameworkCore;
using MobilitywaysAPI.Domain.Entities;

namespace MobilitywaysAPI.Infrastructure.Persistence;

public class MobilitywaysDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public MobilitywaysDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
}
