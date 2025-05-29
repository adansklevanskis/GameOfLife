
namespace GameOfLife.Services;

public interface IGameOfLifeProcessor
{
    Task<int[,]> ComputeMultipleNextStatesAsync(int boardId, int stepsAhead, CancellationToken cancellationToken);
    Task<int[,]> ComputeNextBoardStateAsync(int boardId, CancellationToken cancellationToken);
    Task<int[,]> DetermineFinalBoardStateAsync(int boardId, CancellationToken cancellationToken);
    Task<int?> SaveNewBoardStateAsync(int[,] cellGrid, CancellationToken cancellationToken);
}