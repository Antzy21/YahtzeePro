using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using YahtzeePro.Core.Models;
using YahtzeePro.Play;
using YahtzeePro.Play.Players.AutoPlayers;

namespace YahtzeePro.tests;

public class SimulatorServiceTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(100)]
    public void SimulateGames_ForGivenTotalGames_ReturnsThatManyGameResults(int numberOfGames)
    {
        // Arrange
        var logger = new Mock<ILogger<SimulatorService>>();
        var gameManagerService = new Mock<IGameManagerService>();

        GameResult gameResult = new(0, 0, "");
        
        gameManagerService
            .Setup(g => g.GameIsOver(It.IsAny<Guid>(), out gameResult!))
            .Returns(true);

        var player1 = new Mock<IAutoPlayer>();
        player1.Setup(p => p.Name).Returns("Player 1");
        var player2 = new Mock<IAutoPlayer>();
        player2.Setup(p => p.Name).Returns("Player 2");

        var simulatorService = new SimulatorService(logger.Object, gameManagerService.Object);
        var gameConfiguration = new GameConfiguration(50, 5);

        // Act
        var gameSetResult = simulatorService.SimulateGames(player1.Object, player2.Object, numberOfGames, gameConfiguration);

        // Assert
        Assert.Equal(numberOfGames, gameSetResult.GameResults.Count());
    }
}
