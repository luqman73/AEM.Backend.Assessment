using Microsoft.EntityFrameworkCore;
using AEM.Backend.Assessment.Models;

namespace AEM.Backend.Assessment.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Well> Wells { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Platform>()
            .HasMany(p => p.Well)
            .WithOne()
            .HasForeignKey(w => w.PlatformId);
    }
}