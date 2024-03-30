namespace YahtzeePro
{
    public interface IOptimumCalculator
    {
        OptimumStrategyData Calculate(
            int winningValue,
            int totalDice,
            int initialStackCounterToReturnKnownValue,
            int calculationIterations);
    }
}
