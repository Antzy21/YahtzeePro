using YahtzeePro.Core.Models;

namespace YahtzeePro.Play.Requests;

public record MoveRequest(Guid GameId, MoveChoice Move);