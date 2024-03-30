namespace YahtzeePro;

public interface IOptimumStrategyRepository
{
    public List<string> Get();
    public OptimumStrategyData? Get(int winningValue, int totalDice);
    public void Save(int winningValue, int totalDice, OptimumStrategyData optimumStrategyData);
}