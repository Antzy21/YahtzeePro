namespace YahtzeePro;

public record OptimumStrategyData(
    Dictionary<GameState, GameStateProbabilities> GameStateProbabilities
);
