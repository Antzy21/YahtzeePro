namespace YahtzeePro;

[Serializable]
public record OptimumStrategyData(Dictionary<GameState, GameStateProbabilities> GameStateProbabilities)
{
    public OptimumStrategyData(List<KeyValuePair<GameState, GameStateProbabilities>> serializableOptimumStrategyData)
        : this(serializableOptimumStrategyData.ToDictionary(x => x.Key, x => x.Value)) {}
};
