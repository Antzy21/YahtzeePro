namespace YahtzeePro.Play;

public interface IGameRepository
{
    Guid AddGame(Game game);
    Game? GetGame(Guid gameId);
    IEnumerable<Guid> GetAllGameIds();
    void RemoveGame(Guid gameId);
}