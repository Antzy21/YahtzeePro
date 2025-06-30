using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players.AutoPlayers;

namespace YahtzeePro.Play;

public interface IGameManagerService
{
    Guid CreateNewGame(int winningValue, int diceCount, IAutoPlayer opponent);
    IEnumerable<Guid> GetGameIds();
    Game? GetGame(Guid guid);
    void MakeMove(Guid gameId, MoveChoice moveType);
}