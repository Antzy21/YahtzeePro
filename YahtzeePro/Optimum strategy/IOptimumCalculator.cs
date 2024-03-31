namespace YahtzeePro
{
    public interface IOptimumCalculator
    {
        OptimumStrategyData Calculate(
            GameConfiguration gameConfiguration,
            int initialStackCounterToReturnKnownValue,
            int calculationIterations);
    }
}
