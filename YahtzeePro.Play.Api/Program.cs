using YahtzeePro.Core.Models;
using YahtzeePro.Play.Api;
using YahtzeePro.Play.Api.Requests;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddConsole();
        builder.Services.AddSingleton<GameManager>();

        var app = builder.Build();
        var gameManager = app.Services.GetRequiredService<GameManager>();

        app.MapGet("/", () => "Yahtzee Pro Game Api");

        app.MapPost("/newgame", (GameConfiguration gameConfiguration) =>
        {
            var newGameGuid = gameManager.CreateNewGame(gameConfiguration.WinningValue, gameConfiguration.TotalDice);
            return Results.Created($"/games/{newGameGuid}", newGameGuid);
        });

        app.MapGet("/games", () => gameManager.GetGameIds());

        app.MapGet("/games/{gameId}", (Guid gameId) =>
        {
            var gameState = gameManager.GetGame(gameId);
            if (gameState is not null)
            {
                return Results.Ok(gameState);
            }
            return Results.NotFound();
        });

        app.MapPost("/move", (MoveRequest moveRequest) =>
        {
            var gameState = gameManager.GetGame(moveRequest.GameId);
            if (gameState is null)
                return Results.NotFound();

            var newGameState = gameManager.MakeMove(moveRequest.GameId, moveRequest.Move);
            return Results.Ok(newGameState);
        });

        app.Run();
    }
}
