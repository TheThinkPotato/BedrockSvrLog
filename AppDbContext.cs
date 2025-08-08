using System;
using Microsoft.EntityFrameworkCore;
using BedrockSvrLog.Models;

namespace BedrockSvrLog.Data;

public class AppDbContext : DbContext
{
	public DbSet<User> User => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite("Data Source=app.db");
    }
}
