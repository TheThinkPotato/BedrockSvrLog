using Microsoft.EntityFrameworkCore;
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
    public DbSet<Paper> Paper => Set<Paper>();
    public DbSet<Article> Article => Set<Article>();

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
            .HasOne(pd => pd.Player)
            .WithMany()
            .HasForeignKey(pd => pd.Xuid)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PlayerDeaths>()
            .HasOne(pd => pd.Killer)
            .WithMany()
            .HasForeignKey(pd => pd.KillerXuid)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PlayerKills>()
            .HasOne(pk => pk.Player)
            .WithMany()
            .HasForeignKey(pk => pk.Xuid)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Paper>()
            .HasMany( p => p.Articles)
            .WithOne(a => a.Paper)
            .HasForeignKey(a => a.PaperId);

        modelBuilder.Entity<Article>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.PlayerXuid)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Article>()
            .HasOne(a => a.PlayerDeath)
            .WithMany()
            .HasForeignKey(a => a.PlayerDeathId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Article>()
            .HasOne(a => a.PlayerKills)
            .WithMany()
            .HasForeignKey(a => a.PlayerKillsId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Article>()
            .HasOne(a => a.RealmEvent)
            .WithMany()
            .HasForeignKey(a => a.RealmEventId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
