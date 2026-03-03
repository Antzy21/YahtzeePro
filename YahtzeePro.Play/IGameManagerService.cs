using System.Diagnostics.CodeAnalysis;
using YahtzeePro.Core.Models;

namespace YahtzeePro.Play;

public interface IGameManagerService
{
    void MakeMove(Game game, MoveChoice moveType);
}