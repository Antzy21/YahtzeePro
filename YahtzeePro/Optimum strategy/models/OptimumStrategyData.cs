namespace YahtzeePro;

public class OptimumStrategyData
{
    public required Dictionary<GameState, double> gameStateProbabilities;
    public required Dictionary<GameState, double> gameStateProbabilitiesRisky;
    public required Dictionary<GameState, double> gameStateProbabilitiesSafe;
}
