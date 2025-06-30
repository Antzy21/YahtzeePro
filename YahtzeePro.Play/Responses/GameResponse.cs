using YahtzeePro.Core.Models;

namespace YahtzeePro.Play.Responses;

public record GameResponse(GameState GameState, string ActivePlayerName);