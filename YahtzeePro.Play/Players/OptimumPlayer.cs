using System.Collections.Generic;

namespace YahtzeePro.Play.Players
{
    internal class OptimumPlayer : IPlayer
    {
        private readonly Dictionary<GameState, GameStateProbabilities> _optimumStrategyData;

        public OptimumPlayer(Dictionary<GameState, GameStateProbabilities> optimumStrategyData)
        {
            _optimumStrategyData = optimumStrategyData;
        }

        public string Name => "Best";

        public PlayChoice GetMove(GameState gs, GameConfiguration gc)
        {
            if (_optimumStrategyData[gs].RiskyPlay)
            {
                return PlayChoice.Risky;
            }
            return PlayChoice.Safe;
        }
    }
}
