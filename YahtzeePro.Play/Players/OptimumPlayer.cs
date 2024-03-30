using YahtzeePro.Play;

namespace YahtzeePro.Optimum_strategy
{
    internal class OptimumPlayer : IPlayer
    {
        private readonly OptimumStrategyData _optimumStrategyData;

        public OptimumPlayer(OptimumStrategyData optimumStrategyData)
        {
            _optimumStrategyData = optimumStrategyData;
        }

        public string Name => "Best";

        public PlayChoice GetMove(GameState gs)
        {
            if (_optimumStrategyData.GameStateProbabilities[gs].RiskyPlay)
            {
                return PlayChoice.Risky;
            }
            return PlayChoice.Safe;
        }
    }
}
