﻿namespace YahtzeePro.Play.Players
{
    internal class OptimumPlayer : IPlayer
    {
        private readonly OptimumStrategyData _optimumStrategyData;

        public OptimumPlayer(OptimumStrategyData optimumStrategyData)
        {
            _optimumStrategyData = optimumStrategyData;
        }

        public string Name => "Best";

        public PlayChoice GetMove(GameState gs, GameConfiguration gc)
        {
            if (_optimumStrategyData.GameStateProbabilities[gs].RiskyPlay)
            {
                return PlayChoice.Risky;
            }
            return PlayChoice.Safe;
        }
    }
}
