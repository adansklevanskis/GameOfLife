using GameOfLife.Configuration;
using GameOfLife.Data;
using GameOfLife.Models;
using GameOfLife.Notifications;
using GameOfLife.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace GameOfLifeUnitTests;

public class GameOfLifeProcessorTests
{
    private readonly Mock<IGameBoardRepository> _mockRepository;
    private readonly Mock<IOptions<GameSettings>> _mockIoption;
    private readonly Notifier _notifier;
    private readonly GameOfLifeProcessor _processor;


    public GameOfLifeProcessorTests()
    {
        _mockRepository = new Mock<IGameBoardRepository>();
        _mockIoption = new Mock<IOptions<GameSettings>>();
        _notifier = new Notifier();
        _processor = new(_mockRepository.Object, _mockIoption.Object, _notifier);

    }
    [Fact]
    public async Task ComputeNextBoardState_ShouldGenerateSameGridResult()
    {

        _mockIoption.Setup(s => s.Value).Returns(new GameSettings
        {
            MaxIterations = 10
        });

        _mockRepository.Setup(s => s.RetrieveGameBoardByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GameBoard
            {
                CellGrid = new int[,]
                {
                    { 0, 1, 0 },
                    { 1, 0, 1 },
                    { 0, 1, 0 }
                }
            });

        var expectedNextState = new int[,]
        {
            { 0, 1, 0 },
            { 1, 0, 1 },
            { 0, 1, 0 }
        };

        var nextState = await _processor.ComputeNextBoardStateAsync(1, CancellationToken.None);

        Assert.Equal(expectedNextState, nextState);
    }

    [Fact]
    public async Task ComputeNextBoardState_ShouldGenerateCorrectNextState()
    {

        _mockIoption.Setup(s => s.Value).Returns(new GameSettings
        {
            MaxIterations = 10
        });

        _mockRepository.Setup(s => s.RetrieveGameBoardByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GameBoard
            {
                CellGrid = new int[,]
                {
                    { 1, 1, 0 },
                    { 0, 1, 1 },
                    { 1, 0, 1 }
                }
            });

        var expectedNextState = new int[,]
        {
            { 1, 1, 1 },
            { 0, 0, 1 },
            { 0, 0, 1 }
        };

        var nextState = await _processor.ComputeNextBoardStateAsync(1, CancellationToken.None);

        Assert.Equal(expectedNextState, nextState);
    }

    [Fact]
    public async Task ComputeFinalState_ShouldDetectStableStateAsync()
    {
        var board = new GameBoard();
        board.CellGrid = new int[,] {
            { 1, 1, 0 },
            { 0, 1, 1 },
            { 1, 0, 1 }
        };

        var finalState = await _processor.DetermineFinalBoardStateAsync(board.BoardId, CancellationToken.None);

        Assert.NotNull(finalState);
    }
}
