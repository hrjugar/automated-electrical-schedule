using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace automated_electrical_schedule.Data;

public class DatabaseContext : DbContext
{
    public DbSet<DistributionBoard> DistributionBoards { get; init; }
    public DbSet<Project> Projects { get; init; }
    public DbSet<Circuit> Circuits { get; init; }
    public DbSet<Fixture> Fixtures { get; init; }

    public static string DbPath { get; } = Path.Combine(FileSystem.Current.AppDataDirectory, "aesol.db");

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured) options.UseSqlite($"Data Source={DbPath}");
        options.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .HasIndex(p => p.ProjectName)
            .IsUnique();

        modelBuilder.Entity<DistributionBoard>()
            .HasDiscriminator(b => b.Phase)
            .HasValue<SinglePhaseDistributionBoard>(BoardPhase.SinglePhase)
            .HasValue<ThreePhaseDistributionBoard>(BoardPhase.ThreePhase);

        modelBuilder.Entity<Circuit>()
            .HasDiscriminator(c => c.CircuitType)
            .HasValue<LightingOutletCircuit>(CircuitType.LightingOutlet)
            .HasValue<MotorOutletCircuit>(CircuitType.MotorOutlet)
            .HasValue<ConvenienceOutletCircuit>(CircuitType.ConvenienceOutlet)
            .HasValue<ApplianceEquipmentOutletCircuit>(CircuitType.ApplianceEquipmentOutlet)
            .HasValue<SpaceCircuit>(CircuitType.SpaceOutlet)
            .HasValue<SpareCircuit>(CircuitType.SpareOutlet);
    }
}