using Microsoft.Extensions.Logging;
using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;
using YahtzeePro.Play.Players.AutoPlayers;

namespace YahtzeePro.Play;

public class SimulatorService(ILogger<SimulatorService> logger) : ISimulatorService
{
    public GameResult SimulateGame(IPlayer player1, IPlayer player2, GameConfiguration gameConfiguration)
    {
        var game = new Game(gameConfiguration, player1, player2);

        while (true)
        {
            if (game.GameIsOver(out var result))
            {
                return result;
            }
            var currentPlayer = (IAutoPlayer)game.GetCurrentPlayer();
            var move = currentPlayer.GetMove(game.GameState, game.GameState.GameConfiguration);
            game.MakeMove(move);
            logger.LogInformation("{player} is making move, {move}", currentPlayer, move);
        }
    }

    public GameSetResult SimulateGames(IPlayer player1, IPlayer player2, int totalGames, GameConfiguration gameConfiguration)
    {
        int player1WinCount = 0;
        int player2WinCount = 0;
        List<GameResult> gameResults = [];

        // Player 1 goes first
        for (int i = 0; i < totalGames / 2; i++)
        {
            var gameResult = SimulateGame(player1, player2, gameConfiguration);
            gameResults.Add(gameResult);
            if (gameResult.WinningPlayer == player1.Name)
                player1WinCount++;
            else
                player2WinCount++;
        }

        // Player 2 goes first
        for (int i = 0; i < totalGames / 2; i++)
        {
            var gameResult = SimulateGame(player2, player1, gameConfiguration);
            gameResults.Add(gameResult);
            if (gameResult.WinningPlayer == player1.Name)
                player1WinCount++;
            else
                player2WinCount++;
        }

        return new GameSetResult(gameResults, player1WinCount, player2WinCount);
    }

    public IEnumerable<GameSetResult> SimulateSetsOfGames(IPlayer player1, IPlayer player2, int totalGames, int totalSets, GameConfiguration gameConfiguration)
    {
        int player1SetWins = 0;
        int player2SetWins = 0;
        List<GameSetResult> gameSetResults = [];

        logger.LogInformation("Player 1 \"{player1Name}\"", player1.Name);
        logger.LogInformation("Player 2 \"{player2Name}\"", player2.Name);
        logger.LogInformation("Games per set: {totalGames}\n", totalGames);
        logger.LogInformation("Set | P1 : P2");

        for (int set = 1; set <= totalSets; set++)
        {
            var gameSetResult = SimulateGames(player1, player2, totalGames, gameConfiguration);
            gameSetResults.Add(gameSetResult);
            if (gameSetResult.PlayerOneWinCount > gameSetResult.PlayerTwoWinCount)
                player1SetWins++;
            else if (gameSetResult.PlayerTwoWinCount > gameSetResult.PlayerOneWinCount)
                player2SetWins++;

            logger.LogInformation("\r{set,3} |{player1SetWins,3} :{player2SetWins,3}", set, player1SetWins, player2SetWins);
        }

        return gameSetResults;
    }
}