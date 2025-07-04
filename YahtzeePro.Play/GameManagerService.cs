using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;

namespace YahtzeePro.Play;

public class GameManagerService(ILogger<IGameManagerService> logger) : IGameManagerService
{
    private readonly ILogger<IGameManagerService> _logger = logger;
    private readonly Dictionary<Guid, Game> games = [];

    public Guid CreateNewGame(GameConfiguration gameConfiguration, IPlayer player1, IPlayer player2)
    {
        var newGameGuid = Guid.NewGuid();

        _logger.LogInformation("Creating a new game {gameId}, with {winningValue} to win and {diceCount} dice against {opponent}", newGameGuid, winningValue, diceCount, opponent.Name);

        games.Add(newGameGuid, new Game(gameConfiguration, player1, player2));
        return newGameGuid;
    }

    public IEnumerable<Guid> GetGameIds()
    {
        return games.Keys;
    }

    public Game? GetGame(Guid gameId)
    {
        if (games.TryGetValue(gameId, out Game? game))
        {
            _logger.LogInformation("Retrieved game {gameId}: {game}", gameId, game.GameState);
            return game;
        }
        else
        {
            _logger.LogInformation("Unable to find game with id {gameId}", gameId);
            return null;
        }
    }

    public void MakeMove(Guid gameId, MoveChoice moveType)
    {
        if (games.TryGetValue(gameId, out Game? game))
        {
            _logger.LogInformation("Making {move} move, on {gameId}:, {game}", moveType, gameId, game.GameState);
            game.MakeMove(moveType);
            _logger.LogInformation("Last dice roll: {lastDiceRoll}", game.LastDiceRoll?.ToString() ?? "Banked");
            _logger.LogInformation("Game state after making {move} move: {game}", moveType, game.GameState);
        }
        else
        {
            _logger.LogInformation("Unable to find game with id {gameId}", gameId);
        }
    }

    public bool GameIsOver(Guid gameId, [NotNullWhen(true)] out GameResult? gameResult)
    {
        if (games.TryGetValue(gameId, out Game? game))
        {
            gameResult = null;
            if (game.GameState.OpponentScore >= game.GameState.GameConfiguration.WinningValue)
            {
                gameResult = new(game.GameState.OpponentScore, game.GameState.PlayerScore, game.GetOpponent().Name);
                return true;
            }
            return false;
        }
        else
        {
            _logger.LogInformation("Unable to find game with id {gameId}", gameId);
            throw new KeyNotFoundException($"Game with id {gameId} not found.");
        }
    }
}
