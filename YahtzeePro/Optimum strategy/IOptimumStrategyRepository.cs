namespace YahtzeePro;

public interface IOptimumStrategyRepository
{
    public List<string> Get();
    public OptimumStrategyData Get(int winningValue, int totalDice);
}