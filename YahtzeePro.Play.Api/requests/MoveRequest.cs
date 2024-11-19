namespace YahtzeePro.Play.Api.Requests;

public record MoveRequest(Guid GameId, MoveType Move);