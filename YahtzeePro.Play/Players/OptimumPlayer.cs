using System.Collections.Generic;
using YahtzeePro.Core.Models;

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

        public MoveChoice GetMove(GameState gs, GameConfiguration gc)
        {
            if (_optimumStrategyData[gs].RiskyPlay)
            {
                return MoveChoice.Risky;
            }
            return MoveChoice.Safe;
        }
    }
}
