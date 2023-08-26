namespace YahtzeePro.Play.Players
{
    internal class RollToWin : IPlayer
    {
        public string Name => "RollToWin";

        public PlayChoice GetMove(GameState gs)
        {
            if (gs.CachedScore >= 5000)
            {
                return PlayChoice.Safe;
            }
            return PlayChoice.Risky;
        }
    }
}
