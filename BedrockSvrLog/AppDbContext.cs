using Microsoft.EntityFrameworkCore;
namespace BedrockSvrLog.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> User => Set<User>();
    public DbSet<Login> Login => Set<Login>();
    public DbSet<RealmEvent> RealmEvent => Set<RealmEvent>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
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
    }
}
