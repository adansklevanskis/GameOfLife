using GameOfLife.Configuration;
using Microsoft.Extensions.Options;

namespace GameOfLifeUnitTests;
public class GameSettingsTests
{
    [Fact]
    public void MaxIterations_ShouldMatchConfiguredValue()
    {
        var config = Options.Create(new GameSettings { MaxIterations = 300 });
        var settings = config.Value;

        Assert.Equal(300, settings.MaxIterations);
    }
}
