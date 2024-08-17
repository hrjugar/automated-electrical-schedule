using automated_electrical_schedule.Models;
using Microsoft.EntityFrameworkCore;

namespace automated_electrical_schedule.Data;

public class DatabaseContext : DbContext
{
    public DbSet<ConductorType> ConductorTypes { get; init; }
    public DbSet<DistributionBoard> DistributionBoards { get; init; }
    public DbSet<Project> Projects { get; init; }

    public static string DbPath { get; } = Path.Combine(FileSystem.Current.AppDataDirectory, "aesol.db");

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured) options.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        for (var i = 0; i < ConductorTypeSeed.All.Count; i++) ConductorTypeSeed.All[i].Id = i + 1;

        modelBuilder.Entity<ConductorType>().HasData(ConductorTypeSeed.All);
    }
}