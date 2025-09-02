using Microsoft.EntityFrameworkCore;
using BedrockSvrLog.Model;
using BedrockSvrLog.Models;

namespace BedrockSvrLog.Data;

public class AppDbContext : DbContext
{
    private readonly string? _connectionString;

    public DbSet<User> User => Set<User>();
    public DbSet<Login> Login => Set<Login>();
    public DbSet<RealmEvent> RealmEvent => Set<RealmEvent>();
    public DbSet<World> World => Set<World>();

    public DbSet<PlayerDeaths> PlayerDeaths => Set<PlayerDeaths>();
    public DbSet<PlayerKills> PlayerKills => Set<PlayerKills>();

    public AppDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && !string.IsNullOrWhiteSpace(_connectionString))
        {
            optionsBuilder.UseSqlite(_connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Logins)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.Xuid);

        modelBuilder.Entity<User>()
            .HasMany(u => u.RealmEvents)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.Xuid);

        modelBuilder.Entity<PlayerDeaths>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(pd => pd.Xuid)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PlayerDeaths>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(pd => pd.KillerXuid)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PlayerKills>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(pk => pk.Xuid)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
