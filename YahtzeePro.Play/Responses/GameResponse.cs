using YahtzeePro.Core;
using YahtzeePro.Core.Models;

namespace YahtzeePro.Play.Responses;

public record GameResponse(GameState GameState, string ActivePlayerName, DiceCombination? LastDiceRoll);