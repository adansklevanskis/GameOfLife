using GameOfLife.Models;

namespace GameOfLife.Data;
public interface IGameBoardRepository
{
    Task<GameBoard> RetrieveGameBoardByIdAsync(int boardId, CancellationToken cancellationToken);
    Task<int?> StoreGameBoardAsync(GameBoard gameBoard, CancellationToken cancellationToken);
}