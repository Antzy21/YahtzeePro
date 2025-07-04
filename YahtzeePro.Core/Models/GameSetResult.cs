namespace YahtzeePro.Core.Models;

[Serializable]
public record GameSetResult(IEnumerable<GameResult> GameResults, int PlayerOneWinCount, int PlayerTwoWinCount);