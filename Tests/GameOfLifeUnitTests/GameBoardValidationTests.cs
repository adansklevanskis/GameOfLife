using GameOfLife.Models;
using GameOfLife.Validation;

namespace GameOfLifeUnitTests;
public class GameBoardValidationTests
{
    private readonly GameBoardValidator _validator = new GameBoardValidator();

    [Fact]
    public void ValidateGameBoard_ShouldPassForValidBoard()
    {
        var board = TestUtilities.CreateGameBoard();
        var result = _validator.Validate(board);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void ValidateGameBoard_ShouldFailForEmptyGrid()
    {
        var board = new GameBoard { };
        var result = _validator.Validate(board);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void ValidateGameBoard_ShouldFailForInvalidGridValues()
    {
        var board = new GameBoard
        {
            BoardId = 0,
            CellGrid = new int[,] { { 0, 1 }, { 0, 5 }}
        };
        var result = _validator.Validate(board);

        Assert.False(result.IsValid);
    }
}
