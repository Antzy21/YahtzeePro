using YahtzeePro.Core.Models;

namespace YahtzeePro.Play.Requests;

public record NewGameRequest(
    GameConfiguration GameConfiguration,
    string OpponentName
);