using YahtzeePro.models;

namespace YahtzeePro.Optimum;

public class CalculationManager : ICalculationManager
{
    private readonly IOptimumCalculator _optimumCalculator;
    private readonly Queue<GameConfiguration> _queue = new();
    private readonly SemaphoreSlim _semaphoreSlim = new(1);

    public CalculationManager(IOptimumCalculator optimumCalculator)
    {
        _optimumCalculator = optimumCalculator;
    }

    public IEnumerable<GameConfiguration> Queue => _queue;

    public void QueueCalculation(GameConfiguration gc)
    {
        if (!_queue.Contains(gc))
            _queue.Enqueue(gc);
        if (_semaphoreSlim.CurrentCount != 0)
            Task.Run(RunCalculations);
    }

    private Task RunCalculations()
    {
        _semaphoreSlim.Wait();
        while (_queue.Count > 0)
        {
            var gc = _queue.Dequeue();
            _optimumCalculator.Calculate(gc);
        }
        _semaphoreSlim.Release();
        return Task.CompletedTask;
    }
}