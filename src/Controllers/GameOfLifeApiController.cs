using FluentValidation;
using GameOfLife.Models;
using GameOfLife.Notifications;
using GameOfLife.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLife.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameOfLifeController : ControllerBase
{
    private readonly IGameOfLifeProcessor _gameProcessor;
    private readonly ILogger<GameOfLifeController> _logger;
    private readonly IValidator<GameBoard> _boardValidator;
    private readonly Notifier _notifier;

    public GameOfLifeController(IGameOfLifeProcessor gameProcessor, ILogger<GameOfLifeController> logger, Notifier notifier, IValidator<GameBoard> boardValidator)
    {
        _gameProcessor = gameProcessor;
        _logger = logger;
        _notifier = notifier;
        _boardValidator = boardValidator;
    }

    [HttpPost("board")]
    public async Task<IActionResult> UploadNewBoardAsync([FromBody] GameBoard gameBoard, CancellationToken cancellationToken)
    {
        var validationResult = _boardValidator.Validate(gameBoard);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var boardId = await _gameProcessor.SaveNewBoardStateAsync(gameBoard.CellGrid, cancellationToken);

        if (_notifier.HasNotifications())
        {
            return BadRequest(new { notifications = _notifier.GetNotifications() });
        }

        _logger.LogInformation($"Board {boardId} uploaded successfully.");
        return Ok(new { boardId });
    }


    [HttpGet("board/{boardId}/next")]
    public async Task<IActionResult> GetNextBoardStateAsync([FromRoute] int boardId, CancellationToken cancellationToken)
    {
        var nextState = await _gameProcessor.ComputeNextBoardStateAsync(boardId, cancellationToken);

        if (_notifier.HasNotifications())
        {
            return BadRequest(new { notifications = _notifier.GetNotifications() });
        }

        _logger.LogInformation($"Next state computed for board {boardId}.");
        return Ok(new { nextState });
    }


    [HttpGet("board/{boardId}/next/{steps}")]
    public async Task<IActionResult> GetMultipleFutureStatesAsync([FromRoute] int boardId, [FromRoute] int steps, CancellationToken cancellationToken)
    {
        var nextState = await _gameProcessor.ComputeMultipleNextStatesAsync(boardId, steps, cancellationToken);
        if (_notifier.HasNotifications())
        {
            return BadRequest(new { notifications = _notifier.GetNotifications() });
        }

        _logger.LogInformation($"Nexts states computed for board {boardId}.");
        return Ok(new { nextState });
    }

    [HttpGet("board/{boardId}/final")]
    public async Task<IActionResult> GetFinalBoardStateAsync([FromRoute] int boardId, CancellationToken cancellationToken)
    {
        var finalState = await _gameProcessor.DetermineFinalBoardStateAsync(boardId, cancellationToken);
        _logger.LogInformation($"Final states computed for board {boardId}.");
        if (_notifier.HasNotifications())
        {
            return BadRequest(new { notifications = _notifier.GetNotifications() });
        }

        return Ok(new { finalState });
    }
}