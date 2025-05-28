using FluentAssertions;
using GameOfLife.Data;
using GameOfLife.Notifications;
using GameOfLife.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace GameOfLifeUnitTests;
public class GameBoardRepositoryTests
{
    private GameBoardRepository CreateRepository()
    {
        var options = new DbContextOptionsBuilder<GameOfLifeDbContext>()
            .UseInMemoryDatabase("GameOfLifeTestDb").Options;

        var dbContext = new GameOfLifeDbContext(options);
        var mockLogger = new Mock<ILogger<GameBoardRepository>>();
        return new GameBoardRepository(dbContext, mockLogger.Object, new Notifier());
    }

    [Fact]
    public async Task StoreGameBoard_ShouldSaveBoardAsync()
    {
        var repository = CreateRepository();
        var board = TestUtilities.CreateGameBoard();

        var boardId = await repository.StoreGameBoardAsync(board, CancellationToken.None);

        var retrievedBoard = await repository.RetrieveGameBoardByIdAsync(boardId.Value, CancellationToken.None);
        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new IntArrayJsonConverter() },
            WriteIndented = false
        };

        var str = JsonSerializer.Serialize(board.CellGrid, jsonOptions);
        var res = JsonSerializer.Deserialize<int[,]>(str, jsonOptions);

        retrievedBoard.Should().NotBeNull();
        retrievedBoard.Should().Be(board.CellGrid);
        
    }
}
