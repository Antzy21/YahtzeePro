using YahtzeePro.Play.Requests;
using YahtzeePro.Play.Responses;

namespace YahtzeePro.Play.Api;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddConsole();
        builder.Services.AddScoped<IGameManagerService, GameManagerService>();
        builder.Services.AddScoped<IPlayerResolverService, PlayerResolverService>();

        var app = builder.Build();
        var gameManagerService = app.Services.GetRequiredService<IGameManagerService>();
        var playerResolverService = app.Services.GetRequiredService<IPlayerResolverService>();

        app.MapGet("/", () => "Yahtzee Pro Game Api");

        app.MapPost("/newgame", (NewGameRequest newGameRequest) =>
        {
            var opponent = playerResolverService.ResolveAutoPlayer(newGameRequest.OpponentName);
            var newGameGuid = gameManagerService.CreateNewGame(newGameRequest.GameConfiguration.WinningValue, newGameRequest.GameConfiguration.TotalDice, opponent);
            return Results.Created($"/games/{newGameGuid}", newGameGuid);
        });

        app.MapGet("/games", () => gameManagerService.GetGameIds());

        app.MapGet("/games/{gameId}", (Guid gameId) =>
        {
            var game = gameManagerService.GetGame(gameId);
            if (game is null)
                return Results.NotFound();

            var gameResponse = new GameResponse(game.GameState, game.GetCurrentPlayer().Name, game.LastDiceRoll);
            return Results.Ok(gameResponse);
        });

        app.MapPost("/move", (MoveRequest moveRequest) =>
        {
            var game = gameManagerService.GetGame(moveRequest.GameId);
            if (game is null)
                return Results.NotFound();

            gameManagerService.MakeMove(moveRequest.GameId, moveRequest.Move);

            game = gameManagerService.GetGame(moveRequest.GameId);
            var gameResponse = new GameResponse(game!.GameState, game!.GetCurrentPlayer().Name, game!.LastDiceRoll);
            return Results.Ok(gameResponse);
        });

        app.Run();
    }
}
