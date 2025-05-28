using GameOfLife.Models;

namespace GameOfLifeUnitTests;
public static class TestUtilities
{
    public static int[,] SampleBoard() => new int[,]
    {
        { 0, 1, 0 },
        { 0, 1, 1 },
        { 1, 0, 0 }
    };

    public static GameBoard CreateGameBoard()
    {
        var board = new GameBoard();
        board.CellGrid = SampleBoard();
        return board;
    }
}