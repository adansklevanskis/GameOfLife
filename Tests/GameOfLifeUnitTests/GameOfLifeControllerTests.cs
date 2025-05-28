using FluentAssertions;
using FluentValidation;
using GameOfLife.Controllers;
using GameOfLife.Models;
using GameOfLife.Notifications;
using GameOfLife.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameOfLifeUnitTests;
public class GameOfLifeControllerTests
{
    private readonly Mock<IGameOfLifeProcessor> _mockProcessor = new();
    private readonly GameOfLifeController _controller;
    private readonly Notifier _notifier = new();

    public GameOfLifeControllerTests()
    {
        var logger = new Mock<ILogger<GameOfLifeController>>();
        var validator = new Mock<IValidator<GameBoard>>();
        validator.Setup(s => s.Validate(It.IsAny<GameBoard>())).Returns(new FluentValidation.Results.ValidationResult());
        _controller = new GameOfLifeController(_mockProcessor.Object, logger.Object, _notifier, validator.Object);
    }

    [Fact]
    public async Task UploadNewBoard_ShouldReturnBoardIdAsync()
    {
        var board = TestUtilities.CreateGameBoard();
        _mockProcessor.Setup(p => p.SaveNewBoardStateAsync(It.IsAny<int[,]>(), CancellationToken.None)).ReturnsAsync(1);


        var result = await _controller.UploadNewBoardAsync(board, CancellationToken.None);

        result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetNextBoardState_ShouldReturnNextStateAsync()
    {
        _mockProcessor.Setup(p => p.ComputeNextBoardStateAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(TestUtilities.SampleBoard());

        var result = await _controller.GetNextBoardStateAsync(1, CancellationToken.None);

        result.Should().NotBeNull();
    }
}
