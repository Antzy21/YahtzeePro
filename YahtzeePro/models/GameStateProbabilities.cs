namespace YahtzeePro.models;

[Serializable]
public record GameStateProbabilities(
    bool RiskyPlay,
    double RiskyPlayProbability,
    double SafePlayProbability
);
