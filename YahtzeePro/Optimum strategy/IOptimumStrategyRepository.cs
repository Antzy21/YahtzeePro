namespace YahtzeePro;

public interface IOptimumStrategyRepository
{
    public List<string> Get();
    public OptimumStrategyData? Get(GameConfiguration gameConfiguration);
    public void Save(GameConfiguration gameConfiguration, OptimumStrategyData optimumStrategyData);
}