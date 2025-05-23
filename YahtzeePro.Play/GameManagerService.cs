using Microsoft.Extensions.Logging;
using YahtzeePro.Core.Models;

namespace YahtzeePro.Play;

public class GameManagerService(ILogger<IGameManagerService> logger) : IGameManagerService
{
    private readonly ILogger<IGameManagerService> _logger = logger;
    private readonly Dictionary<Guid, Game> games = [];

    public Guid CreateNewGame(int winningValue, int diceCount, IPlayer opponent)
    {
        _logger.LogInformation("Creating a new game with winning value {winningValue} and dice count {diceCount} against {opponent}", winningValue, diceCount, opponent.Name);

        var newGameConfiguration = new GameConfiguration(winningValue, diceCount);
        var newGameGuid = Guid.NewGuid();
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

    public Game? GetGame(Guid guid)
    {
        if(!games.TryGetValue(guid, out Game? game)) {
            _logger.LogInformation("Unable to find game for with id: {guid}", guid);
            return null;
        };
        _logger.LogInformation("Retrieved game: {game}", game);
        return game;
    }

    public void MakeMove(Guid gameId, MoveChoice moveType)
    {
        var game = GetGame(gameId);
        _logger.LogInformation("Unable to find game for with id: {gameId}", gameId);
        throw new NotImplementedException();
    }
}

internal class HumanPlayer : IPlayer
{
    public string Name => throw new NotImplementedException();

    public MoveChoice GetMove(GameState gs, GameConfiguration gc)
    {
        throw new NotImplementedException();
    }
}