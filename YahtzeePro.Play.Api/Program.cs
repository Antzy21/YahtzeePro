using YahtzeePro.Play.Requests;

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
            var gameState = gameManagerService.GetGame(gameId);
            if (gameState is not null)
            {
                return Results.Ok(gameState);
            }
            return Results.NotFound();
        });

        app.MapPost("/move", (MoveRequest moveRequest) =>
        {
            var gameState = gameManagerService.GetGame(moveRequest.GameId);
            if (gameState is null)
                return Results.NotFound();

            gameManagerService.MakeMove(moveRequest.GameId, moveRequest.Move);
            return Results.Ok(gameManagerService.GetGame(moveRequest.GameId));
        });

        app.Run();
    }
}
