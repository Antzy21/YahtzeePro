using YahtzeePro.Play;

namespace YahtzeePro.Optimum_strategy
{
    internal class OptimumPlayer : OptimumStrategy, IPlayer
    {
        public OptimumPlayer(int winningValue = 5000, int totalDice = 5) : base(winningValue, totalDice)
        {
            ReadDataFromFile();
        }

        public string Name => "Best";

        public PlayChoice GetMove(GameState gs)
        {
            if (gameStateProbabilitiesRisky[gs] > gameStateProbabilitiesSafe[gs])
            {
                return PlayChoice.Risky;
            }
            return PlayChoice.Safe;
        }
    }
}
