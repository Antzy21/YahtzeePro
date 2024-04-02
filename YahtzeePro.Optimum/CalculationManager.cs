using YahtzeePro.models;

namespace YahtzeePro.Optimum;

public class CalculationManager : ICalculationManager
{
    private readonly IOptimumCalculator _optimumCalculator;
    private readonly Queue<GameConfiguration> _queue = new();
    private readonly SemaphoreSlim _semaphoreSlim = new(1);
    private IOptimumStrategyRepository _optimumStrategyRepository;

    public CalculationManager(IOptimumCalculator optimumCalculator, IOptimumStrategyRepository optimumStrategyRepository)
    {
        _optimumCalculator = optimumCalculator;
        _optimumStrategyRepository = optimumStrategyRepository;
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
            var optimum = _optimumCalculator.Calculate(gc);
            _optimumStrategyRepository.Save(gc, optimum);
        }
        _semaphoreSlim.Release();
        return Task.CompletedTask;
    }
}