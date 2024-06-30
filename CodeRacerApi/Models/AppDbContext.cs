using CodeRacerApi.Models.SQLite;
using Microsoft.EntityFrameworkCore;

namespace CodeRacerApi.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<SQLite.Lobby> Lobbys { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SQLite.Lobby>()
            .HasMany(e => e.Users)
            .WithMany(e => e.Lobbys);
        
        modelBuilder.Entity<User>()
            .HasMany(e => e.Lobbys)
            .WithMany(e => e.Users);
    }
}