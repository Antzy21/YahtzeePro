namespace YahtzeePro.Core.Models;

[Serializable]
public record GameStateProbabilities(
    bool RiskyPlay,
    double RiskyPlayProbability,
    double SafePlayProbability
);
