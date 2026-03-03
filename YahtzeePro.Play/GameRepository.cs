namespace YahtzeePro.Play;

public class InMemoryGameRepository : IGameRepository
{
    private readonly Dictionary<Guid, Game> _games = [];

    public Guid AddGame(Game game)
    {
        var gameId = Guid.NewGuid();
        if (!_games.ContainsKey(gameId))
        {
            _games[gameId] = game;
        }
        return gameId;
    }

    public Game? GetGame(Guid gameId)
    {
        _games.TryGetValue(gameId, out var game);
        return game;
    }

    public IEnumerable<Guid> GetAllGameIds()
    {
        return _games.Keys;
    }

    public void RemoveGame(Guid gameId)
    {
        _games.Remove(gameId);
    }
}