using Microsoft.Extensions.Logging;
using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;
using YahtzeePro.Play.Players.AutoPlayers;

namespace YahtzeePro.Play;

public class GameManagerService(ILogger<IGameManagerService> logger) : IGameManagerService
{
    private readonly ILogger<IGameManagerService> _logger = logger;
    private readonly Dictionary<Guid, Game> games = [];

    public Guid CreateNewGame(int winningValue, int diceCount, IAutoPlayer opponent)
    {
        var newGameConfiguration = new GameConfiguration(winningValue, diceCount);
        var newGameGuid = Guid.NewGuid();

        _logger.LogInformation("Creating a new game {gameId}, with {winningValue} to win and {diceCount} dice against {opponent}", newGameGuid, winningValue, diceCount, opponent.Name);

        games.Add(newGameGuid, new Game(
            gameConfiguration: newGameConfiguration,
            player1: new HumanPlayer(),
            player2: opponent
        ));
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
            _logger.LogInformation("Game state after making {move} move: {game}", moveType, game.GameState);

        }
        else
        {
            _logger.LogInformation("Unable to find game with id {gameId}", gameId);
        }
    }
}
