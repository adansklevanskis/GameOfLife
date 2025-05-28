using FluentValidation;
using GameOfLife.Models;

namespace GameOfLife.Validation;

public class GameBoardValidator : AbstractValidator<GameBoard>
{
    public GameBoardValidator()
    {
        RuleFor(board => board.CellGrid)
            .NotEmpty().WithMessage("CellGrid cannot be empty.")
            .Must(ContainOnlyZeroOrOne).WithMessage("CellGrid must only contain 0 or 1.");
    }

    private bool ContainOnlyZeroOrOne(int[,] cellGrid)
    {
        if (cellGrid == null) return false;

        foreach (var value in cellGrid)
        {
            if (value != 0 && value != 1) return false;
        }
        return true;
    }
}

