namespace YahtzeePro.models;

[Serializable]
public record GameConfiguration(
    int WinningValue,
    int TotalDice
);