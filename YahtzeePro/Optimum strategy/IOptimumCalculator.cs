namespace YahtzeePro
{
    public interface IOptimumCalculator
    {
        Dictionary<GameState, GameStateProbabilities> Calculate(
            GameConfiguration gameConfiguration,
            int initialStackCounterToReturnKnownValue,
            int calculationIterations);
    }
}
