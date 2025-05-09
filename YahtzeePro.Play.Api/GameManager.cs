using YahtzeePro.Core.Models;

namespace YahtzeePro.Play.Api;

public class GameManager(ILogger<GameManager> logger)
{
    private readonly ILogger<GameManager> _logger = logger;
    private readonly Dictionary<Guid, GameState> games = new();

    internal Guid CreateNewGame(int winningValue, int diceCount)
    {
        _logger.LogInformation("Creating a new game with winning value {winningValue} and dice count {diceCount}", winningValue, diceCount);

        var newGameConfiguration = new GameConfiguration(winningValue, diceCount);
        var newGameGuid = Guid.NewGuid();
        games.Add(newGameGuid, new GameState(){
            PlayerScore = 0,
            OpponentScore = 0,
            CachedScore = 0,
            DiceToRoll = diceCount,
            GameConfiguration = newGameConfiguration
        });
        return newGameGuid;
    }

    internal IEnumerable<Guid> GetGameIds()
    {
        return games.Keys;
    }

    internal GameState? GetGame(Guid guid)
    {
        if(!games.TryGetValue(guid, out GameState gameState)) {
            _logger.LogInformation("Unable to find game for with id: {guid}", guid);
            return null;
        };
        _logger.LogInformation("Retrieved game: {game}", gameState);
        return gameState;
    }

    internal GameState MakeMove(Guid gameId, MoveChoice moveType)
    {
        var game = GetGame(gameId);
        _logger.LogInformation("Unable to find game for with id: {gameId}", gameId);
        throw new NotImplementedException();
    }
}