using GameOfLife.Configuration;
using GameOfLife.Data;
using GameOfLife.Models;
using GameOfLife.Notifications;
using Microsoft.Extensions.Options;

namespace GameOfLife.Services;

public class GameOfLifeProcessor : IGameOfLifeProcessor
{
    private readonly IGameBoardRepository _boardRepository;
    private readonly GameSettings _gameSettings;
    private readonly Notifier _notifier;

    public GameOfLifeProcessor(IGameBoardRepository boardRepository, IOptions<GameSettings> gameSettings, Notifier notifier)
    {
        _boardRepository = boardRepository;
        _gameSettings = gameSettings.Value;
        _notifier = notifier;
    }

    public Task<int?> SaveNewBoardStateAsync(int[,] cellGrid, CancellationToken cancellationToken)
    {
        if (cellGrid == null || cellGrid.Length == 0)
        {
            _notifier.AddNotification("Invalid board data.", "INVALID_BOARD");
            return Task.FromResult<int?>(null);
        }

        var gameBoard = new GameBoard { CellGrid = cellGrid };
        return _boardRepository.StoreGameBoardAsync(gameBoard, cancellationToken);
    }

    public async Task<int[,]> ComputeNextBoardStateAsync(int boardId, CancellationToken cancellationToken)
    {
        var gameBoard = await _boardRepository.RetrieveGameBoardByIdAsync(boardId, cancellationToken);

        if (gameBoard == null)
        {
            _notifier.AddNotification($"Game board {boardId} not found.", "BOARD_NOT_FOUND");
            return new int[0, 0]; // Return empty grid
        }

        return GenerateNextState(gameBoard.CellGrid);
    }

    public async Task<int[,]> ComputeMultipleNextStatesAsync(int boardId, int stepsAhead, CancellationToken cancellationToken)
    {
        var gameBoard = await _boardRepository.RetrieveGameBoardByIdAsync(boardId, cancellationToken);

        if (gameBoard == null)
        {
            _notifier.AddNotification($"Game board {boardId} not found.", "BOARD_NOT_FOUND");
            return new int[0, 0]; // Return empty grid
        }

        var currentState = gameBoard.CellGrid;

        for (int step = 0; step < stepsAhead; step++)
        {
            currentState = GenerateNextState(currentState);
        }

        return currentState;
    }

    public async Task<int[,]> DetermineFinalBoardStateAsync(int boardId, CancellationToken cancellationToken)
    {
        var gameBoard = await _boardRepository.RetrieveGameBoardByIdAsync(boardId, cancellationToken);

        if (gameBoard == null)
        {
            _notifier.AddNotification($"Game board {boardId} not found.", "BOARD_NOT_FOUND");
            return new int[0, 0]; // Return empty grid
        }

        var currentState = gameBoard.CellGrid;
        var previousStates = new HashSet<string>();

        for (int iteration = 0; iteration < _gameSettings.MaxIterations; iteration++)
        {
            string stateSignature = string.Join(",", currentState.Cast<int>());
            if (previousStates.Contains(stateSignature)) return currentState;

            previousStates.Add(stateSignature);
            currentState = GenerateNextState(currentState);
        }

        _notifier.AddNotification("Board state did not stabilize within the allowed iterations.", "MAX_ITERATIONS_EXCEED");

        return currentState;
    }

    private int[,] GenerateNextState(int[,] currentGrid)
    {
        int rowCount = currentGrid.GetLength(0);
        int columnCount = currentGrid.GetLength(1);
        int[,] nextGrid = new int[rowCount, columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                int liveNeighborCount = CountLiveNeighbors(currentGrid, row, column);
                bool isCellAlive = currentGrid[row, column] == 1;

                bool survives = isCellAlive && (liveNeighborCount == 2 || liveNeighborCount == 3);
                bool reproduces = !isCellAlive && liveNeighborCount == 3;

                nextGrid[row, column] = (survives || reproduces) ? 1 : 0;
            }
        }

        return nextGrid;
    }

    private int CountLiveNeighbors(int[,] grid, int row, int column)
    {
        int neighborCount = 0;
        int[] directionOffsets = { -1, 0, 1 };

        foreach (int rowOffset in directionOffsets)
        {
            foreach (int columnOffset in directionOffsets)
            {
                if (rowOffset == 0 && columnOffset == 0) continue;

                int neighborRow = row + rowOffset;
                int neighborColumn = column + columnOffset;

                if (neighborRow >= 0 && neighborColumn >= 0 &&
                    neighborRow < grid.GetLength(0) && neighborColumn < grid.GetLength(1))
                {
                    neighborCount += grid[neighborRow, neighborColumn];
                }
            }
        }

        return neighborCount;
    }
}