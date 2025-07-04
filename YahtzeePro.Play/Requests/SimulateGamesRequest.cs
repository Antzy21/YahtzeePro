using YahtzeePro.Core.Models;

namespace YahtzeePro.Play.Requests;

public record SimulateGamesRequest(
    string Player1Name,
    string Player2Name,
    int TotalGames,
    int TotalSets,
    GameConfiguration GameConfiguration
);