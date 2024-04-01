using YahtzeePro.models;

namespace YahtzeePro.Play.Players
{
    internal class RollToWin : IPlayer
    {
        public string Name => "RollToWin";

        public PlayChoice GetMove(GameState gs, GameConfiguration gc)
        {
            if (gs.CachedScore >= gc.WinningValue)
            {
                return PlayChoice.Safe;
            }
            return PlayChoice.Risky;
        }
    }
}
