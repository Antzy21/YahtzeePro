using Microsoft.Extensions.Logging;
using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;

namespace YahtzeePro.Play;

public class GameManagerService(ILogger<IGameManagerService> logger) : IGameManagerService
{
    private readonly ILogger<IGameManagerService> _logger = logger;
    private readonly Dictionary<Guid, Game> games = [];

    public Guid CreateNewGame(int winningValue, int diceCount, IPlayer opponent)
    {
        var newGameConfiguration = new GameConfiguration(winningValue, diceCount);
        var newGameGuid = Guid.NewGuid();

        _logger.LogInformation("Creating a new game {gameId}, with winning value {winningValue} and dice count {diceCount} against {opponent}", newGameGuid, winningValue, diceCount, opponent.Name);

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
            _logger.LogInformation("Retrieved game for guid {gameId}: {game}", gameId, game);
            return game;
        }
        _logger.LogInformation("Unable to find game for with id: {gameId}", gameId);
        return null;
    }

    public void MakeMove(Guid gameId, MoveChoice moveType)
    {
        if (games.TryGetValue(gameId, out Game? game))
        {
            _logger.LogInformation("Making move, {move}, on game: {gameId}", moveType, gameId);
            game.MakeMove(moveType);
        }
        _logger.LogInformation("Unable to find game for with id: {gameId}", gameId);
        return;
    }
}
