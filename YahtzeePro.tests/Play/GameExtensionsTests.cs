using System;
using Xunit;
using YahtzeePro.Play;
using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;
using YahtzeePro.Core;

namespace YahtzeePro.tests.Play;

public class GameExtensionsTests
{
    private class TestPlayer(string name) : IPlayer
    {
        public string Name { get; set; } = name;
    }

    [Fact]
    public void GetCurrentPlayer_ReturnsPlayer1_WhenTurnsEven()
    {
        var game = new Game(new GameConfiguration(5000, 5), new TestPlayer("P1"), new TestPlayer("P2"))
        {
            Turns = 0
        };
        var player = game.GetCurrentPlayer();
        Assert.Equal("P1", player.Name);
    }

    [Fact]
    public void GetCurrentPlayer_ReturnsPlayer2_WhenTurnsOdd()
    {
        var game = new Game(new GameConfiguration(5000, 5), new TestPlayer("P1"), new TestPlayer("P2"))
        {
            Turns = 1
        };
        var player = game.GetCurrentPlayer();
        Assert.Equal("P2", player.Name);
    }

    [Fact]
    public void GameIsOver_ReturnsFalse_WhenScoreBelowWinning()
    {
        var game = new Game(new GameConfiguration(5000, 5), new TestPlayer("P1"), new TestPlayer("P2"));
        var result = game.GameIsOver(out var gameResult);
        Assert.False(result);
        Assert.Null(gameResult);
    }

    [Fact]
    public void GameIsOver_ReturnsTrue_WhenScoreAtOrAboveWinning()
    {
        var game = new Game(new GameConfiguration(5000, 5), new TestPlayer("P1"), new TestPlayer("P2"));
        game.GameState = game.GameState.RollOver(5000); // Simulate reaching winning score
        var result = game.GameIsOver(out var gameResult);
        Assert.True(result);
        Assert.NotNull(gameResult);
        Assert.Equal(5000, gameResult!.WinnerScore);
        Assert.Equal("P1", gameResult.WinningPlayer);
    }

    [Fact]
    public void ResolveRolledDice_ScoreZero_IncrementsTurnAndCallsFail()
    {
        var game = new Game(new GameConfiguration(5000, 5), new TestPlayer("P1"), new TestPlayer("P2"));
        var originalTurns = game.Turns;
        var dice = DiceCombinationGenerator.Empty();
        game.ResolveRolledDice(dice);
        Assert.Equal(originalTurns + 1, game.Turns);
        // Can't check GameState.Fail() result directly, but can check IsStartOfTurn reset
        Assert.True(game.GameState.IsStartOfTurn);
    }

    [Fact]
    public void ResolveRolledDice_AllDiceScoring_CallsRollOver()
    {
        var game = new Game(new GameConfiguration(5000, 4), new TestPlayer("P1"), new TestPlayer("P2"));
        var dice = DiceCombinationGenerator.FromDieList([1, 1, 5, 5]); // All scoring dice
        game.ResolveRolledDice(dice);
        // Can't check internals, but can check that CachedScore increased
        Assert.Equal(300, game.GameState.CachedScore);
        Assert.Equal(0, game.Turns); // Should not increment turns
        Assert.False(game.GameState.IsStartOfTurn); // Should not be start of turn
    }

    [Fact]
    public void ResolveRolledDice_PartialScore_AddsRolledScore()
    {
        var game = new Game(new GameConfiguration(5000, 4), new TestPlayer("P1"), new TestPlayer("P2"));
        var dice = DiceCombinationGenerator.FromDieList([1, 2, 3, 4]); // Only 1 scoring die
        game.ResolveRolledDice(dice);
        Assert.Equal(100, game.GameState.CachedScore);
        Assert.Equal(0, game.Turns); // Should not increment turns
        Assert.False(game.GameState.IsStartOfTurn); // Should not be start of turn
    }
}
