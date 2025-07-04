namespace YahtzeePro.Core.Models;

[Serializable]
public record GameResult(int WinnerScore, int LoserScore, string WinningPlayer);
