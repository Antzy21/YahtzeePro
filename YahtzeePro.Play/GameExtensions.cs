using System.Diagnostics.CodeAnalysis;
using YahtzeePro.Core;
using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;

namespace YahtzeePro.Play;

public static class GameExtensions
{
    public static IPlayer GetCurrentPlayer(this Game game)
    {
        return game.Turns % 2 == 0 ? game.Player1 : game.Player2;
    }

    public static bool GameIsOver(this Game game, [NotNullWhen(true)] out GameResult? gameResult)
    {
        gameResult = null;
        if (game.GameState.PlayerScore + game.GameState.CachedScore >= game.GameState.GameConfiguration.WinningValue)
        {
            gameResult = new(game.GameState.PlayerScore + game.GameState.CachedScore, game.GameState.OpponentScore, game.GetCurrentPlayer().Name);
            return true;
        }
        return false;
    }

    public static void ResolveRolledDice(this Game game, DiceCombination rolledDice)
    {
        if (rolledDice.Score == 0)
        {
            game.Turns++;
            game.GameState = game.GameState.Fail();
        }
        else if (rolledDice.AllDiceAreScoring)
        {
            game.GameState = game.GameState.RollOver(rolledDice.Score);
        }
        else
        {
            game.GameState = game.GameState.AddRolledScore(rolledDice.Score, rolledDice.NumberOfScoringDice);
        }
    }
}
