namespace YahtzeePro.Core.Models;

[Serializable]
public record GameConfiguration(
    int WinningValue,
    int TotalDice
);