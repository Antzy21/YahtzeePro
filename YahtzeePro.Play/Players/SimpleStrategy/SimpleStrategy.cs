using System.Linq;
using YahtzeePro.models;

namespace YahtzeePro.Play.Players.SimpleStrategy
{
    internal class SimpleStrategy : IPlayer
    {
        private SimpleStrategyConfiguration _configuration;

        public SimpleStrategy(SimpleStrategyConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Name => string.Join(",", _configuration.WhenToBankWithNumberOfDice.Select(kv => $"{kv.Key}-{kv.Value}"));

        public PlayChoice GetMove(GameState gs, GameConfiguration gc)
        {
            if (gs.CachedScore + gs.PlayerScore >= gc.WinningValue) {
                return PlayChoice.Safe;
            }

            if (!_configuration.WhenToBankWithNumberOfDice.TryGetValue(gs.DiceToRoll, out var whenToBank)) {
                whenToBank = int.MaxValue;
            }

            if (gs.CachedScore >= whenToBank)
            {
                return PlayChoice.Safe;
            }
            return PlayChoice.Risky;
        }
    }
}