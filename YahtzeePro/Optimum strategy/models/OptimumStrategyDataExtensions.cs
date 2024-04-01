namespace YahtzeePro;

public static class OptimumStrategyDataExtensions {
    public static List<KeyValuePair<GameState, GameStateProbabilities>> MakeSerializable(this OptimumStrategyData optimumStrategyData) {
        return optimumStrategyData.GameStateProbabilities.ToList();
    }
}
