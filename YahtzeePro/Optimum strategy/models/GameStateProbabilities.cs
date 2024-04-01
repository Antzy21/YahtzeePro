namespace YahtzeePro;

[Serializable]
public record GameStateProbabilities(
    bool RiskyPlay,
    double RiskyPlayProbability,
    double SafePlayProbability
);
