using System;
using System.Collections.Generic;
using YahtzeePro.Core.Models;

namespace YahtzeePro.Play;

public interface IGameManagerService
{
    Guid CreateNewGame(int winningValue, int diceCount, IPlayer opponent);
    IEnumerable<Guid> GetGameIds();
    Game? GetGame(Guid guid);
    void MakeMove(Guid gameId, MoveChoice moveType);
}