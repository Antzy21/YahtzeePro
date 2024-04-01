using YahtzeePro.models;

namespace YahtzeePro.Optimum;

public class CalculationManager : ICalculationManager
{
    private readonly IOptimumCalculator _optimumCalculator;

    private readonly Queue<GameConfiguration> queue = new();

    public CalculationManager(IOptimumCalculator optimumCalculator)
    {
        _optimumCalculator = optimumCalculator;
    }

    public void QueueCalculation(GameConfiguration gc)
    {
        if (!queue.Contains(gc))
            queue.Enqueue(gc);
    }
}