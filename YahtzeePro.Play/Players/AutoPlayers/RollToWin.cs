using YahtzeePro.Core.Models;

namespace YahtzeePro.Play.Players.AutoPlayers;

internal class RollToWin : IAutoPlayer
{
    public string Name => "RollToWin";

    public MoveChoice GetMove(GameState gs, GameConfiguration gc)
    {
        if (gs.CachedScore >= gc.WinningValue)
        {
            return MoveChoice.Safe;
        }
        return MoveChoice.Risky;
    }
}
