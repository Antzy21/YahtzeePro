using YahtzeePro.Core.Models;

namespace YahtzeePro.Play.Api.Requests;

public record MoveRequest(Guid GameId, MoveChoice Move);