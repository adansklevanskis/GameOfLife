using GameOfLife.Models;
using GameOfLife.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GameOfLife.Data;

public class GameOfLifeDbContext : DbContext
{

    public DbSet<GameBoard> GameBoards { get; set; }

    public GameOfLifeDbContext(DbContextOptions<GameOfLifeDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new IntArrayJsonConverter() },
            WriteIndented = false
        };

        modelBuilder.Entity<GameBoard>()
            .HasKey(b => b.BoardId);

        modelBuilder.Entity<GameBoard>()
            .Property(b => b.CellGrid)
            .HasConversion(
              v => JsonSerializer.Serialize(v, jsonOptions),
              v => JsonSerializer.Deserialize<int[,]>(v, jsonOptions));
    }
}
