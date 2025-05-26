namespace YahtzeePro.Core.Models;

public record GameSetResult(IEnumerable<GameResult> GameResults, int PlayerOneWinCount, int PlayerTwoWinCount);