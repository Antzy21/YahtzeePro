using YahtzeePro.Play.Requests;
using YahtzeePro.Play.Responses;
using YahtzeePro.Play.Players;

namespace YahtzeePro.Play.Api;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddConsole();
        builder.Services.AddScoped<IGameManagerService, GameManagerService>();
        builder.Services.AddScoped<IGameRepository, InMemoryGameRepository>();
        builder.Services.AddScoped<IPlayerResolverService, PlayerResolverService>();
        builder.Services.AddScoped<ISimulatorService, SimulatorService>();

        var app = builder.Build();
        var gameManagerService = app.Services.GetRequiredService<IGameManagerService>();
        var gameRepository = app.Services.GetRequiredService<IGameRepository>();
        var simulator = app.Services.GetRequiredService<ISimulatorService>();
        var playerResolverService = app.Services.GetRequiredService<IPlayerResolverService>();

        app.MapGet("/", () => "Yahtzee Pro Game Api");

        app.MapPost("/newgame", (NewGameRequest newGameRequest) =>
        {
            var opponent = playerResolverService.ResolveAutoPlayer(newGameRequest.OpponentName);

            var newGame = new Game(newGameRequest.GameConfiguration, new HumanPlayer(), opponent);
            var newGameId = gameRepository.AddGame(newGame);

            return Results.Created($"/games/{newGameId}", newGameId);
        });

        app.MapGet("/games", () => gameRepository.GetAllGameIds());

        app.MapGet("/games/{gameId}", (Guid gameId) =>
        {
            var game = gameRepository.GetGame(gameId);
            if (game is null)
                return Results.NotFound();
            return Results.Ok(game);
        });

        app.MapPost("/move", (MoveRequest moveRequest) =>
        {
            var game = gameRepository.GetGame(moveRequest.GameId);
            if (game is null)
                return Results.NotFound();

            gameManagerService.MakeMove(game, moveRequest.Move);

            var updatedGame = gameRepository.GetGame(moveRequest.GameId);
            return Results.Ok(updatedGame);
        });

        app.MapGet("/strategies", () =>
        {
            var strategies = playerResolverService.GetAvailableAutoPlayers();
            return Results.Ok(strategies);
        });

        app.MapPost("/simulate", (SimulateGamesRequest simulateGamesRequest) =>
        {
            var player1 = playerResolverService.ResolveAutoPlayer(simulateGamesRequest.Player1Name);
            var player2 = playerResolverService.ResolveAutoPlayer(simulateGamesRequest.Player2Name);
            var gameSetResult = simulator.SimulateSetsOfGames(player1, player2, simulateGamesRequest.TotalGames, simulateGamesRequest.TotalSets, simulateGamesRequest.GameConfiguration).ToList();
            return Results.Ok(gameSetResult);
        });

        app.Run();
    }
}
