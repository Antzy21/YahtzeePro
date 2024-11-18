
using YahtzeePro.models;

internal class GameManager
{
    private readonly Dictionary<Guid, GameState> games = new();

    internal Guid CreateNewGame(int winningValue, int diceCount)
    {
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
            return null;
        };
        return gameState;
    }
}