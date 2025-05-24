using FCG.Domain.Entity;
using FCG.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserMap());
    }
}
