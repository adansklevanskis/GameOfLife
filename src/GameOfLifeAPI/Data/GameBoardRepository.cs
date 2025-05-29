using GameOfLife.Models;
using GameOfLife.Notifications;
using Microsoft.EntityFrameworkCore;

namespace GameOfLife.Data;

public class GameBoardRepository : IGameBoardRepository
{
    private readonly GameOfLifeDbContext _dbContext;
    private readonly ILogger<GameBoardRepository> _logger;
    private readonly Notifier _notifier;

    public GameBoardRepository(GameOfLifeDbContext dbContext, ILogger<GameBoardRepository> logger, Notifier notifier)
    {
        _dbContext = dbContext;
        _logger = logger;
        _notifier = notifier;
    }

    public async Task<int?> StoreGameBoardAsync(GameBoard gameBoard, CancellationToken cancellationToken)
    {
        if (gameBoard.CellGrid.Length == 0)
        {
            _notifier.AddNotification("Board data cannot be empty.", "EMPTY_BOARD");
            return null;
        }


        await _dbContext.GameBoards.AddAsync(gameBoard, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Board {gameBoard.BoardId} stored successfully.");
        return gameBoard.BoardId;
    }

    public async Task<GameBoard> RetrieveGameBoardByIdAsync(int boardId, CancellationToken cancellationToken)
    {
        var board = await _dbContext.GameBoards.FirstOrDefaultAsync(b => b.BoardId == boardId, cancellationToken);

        if (board == null)
        {
            _notifier.AddNotification($"Game board {boardId} not found.", "BOARD_NOT_FOUND");
            _logger.LogWarning($"Game board {boardId} not found.");
            return null;
        }


        _logger.LogInformation($"Board {boardId} retrieved successfully.");


        return board;
    }
}