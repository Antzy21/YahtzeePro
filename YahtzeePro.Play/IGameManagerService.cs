using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;

namespace YahtzeePro.Play;

public interface IGameManagerService
{
    Guid CreateNewGame(int winningValue, int diceCount, IPlayer opponent);
    IEnumerable<Guid> GetGameIds();
    Game? GetGame(Guid guid);
    void MakeMove(Guid gameId, MoveChoice moveType);
}