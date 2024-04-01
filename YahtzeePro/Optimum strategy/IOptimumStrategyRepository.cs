namespace YahtzeePro;

public interface IOptimumStrategyRepository
{
    public List<string> Get();
    public Dictionary<GameState, GameStateProbabilities>? Get(GameConfiguration gameConfiguration);
    public void Save(GameConfiguration gameConfiguration, Dictionary<GameState, GameStateProbabilities> optimumStrategyData);
}