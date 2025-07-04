using System.Diagnostics.CodeAnalysis;
using YahtzeePro.Core.Models;
using YahtzeePro.Play.Players;

namespace YahtzeePro.Play;

public interface IGameManagerService
{
    Guid CreateNewGame(GameConfiguration gameConfiguration, IPlayer player1, IPlayer player2);
    IEnumerable<Guid> GetGameIds();
    Game? GetGame(Guid gameId);
    void MakeMove(Guid gameId, MoveChoice moveType);
    bool GameIsOver(Guid gameId, [NotNullWhen(true)] out GameResult? result);
}