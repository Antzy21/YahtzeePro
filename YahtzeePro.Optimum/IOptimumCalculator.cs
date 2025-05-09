using YahtzeePro.Core.Models;

namespace YahtzeePro.Optimum
{
    public interface IOptimumCalculator
    {
        Dictionary<GameState, GameStateProbabilities> Calculate(
            GameConfiguration gameConfiguration,
            int initialStackCounterToReturnKnownValue = 3,
            int calculationIterations = 2);
    }
}
