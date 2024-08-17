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

    // public async Task InitAsync()
    // {
    //     if (_conn is not null) return;
    //
    //     _conn = new SQLiteAsyncConnection(DbPath);
    //     await _conn.ExecuteAsync("PRAGMA foreign_keys = ON;");
    //
    //     if (!await IsTableExisting(ConductorType.TableName))
    //     {
    //         await _conn.CreateTableAsync<ConductorType>();
    //         await _conn.InsertAllAsync(ConductorTypeData.All);
    //     }
    //
    //     if (!await IsTableExisting(DistributionBoard.TableName)) await _conn.CreateTableAsync<DistributionBoard>();
    // }
    //
    // private async Task<bool> IsTableExisting(string tableName)
    // {
    //     if (_conn is null) throw new InvalidOperationException("Database is not initialized.");
    //
    //     var tableCount = await _conn.ExecuteScalarAsync<int>(
    //         "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name = ?;", tableName
    //     );
    //
    //     return tableCount > 0;
    // }
    //
    // public async Task<List<ConductorType>> GetConductorTypesAsync()
    // {
    //     await InitAsync();
    //     return await _conn!.Table<ConductorType>().ToListAsync();
    // }
}