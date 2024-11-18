internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<GameManager>();

        var app = builder.Build();
        var gameManager = app.Services.GetRequiredService<GameManager>();
        
        app.MapGet("/", () => "Hello World!");

        app.MapPost("/newgame", (int winningValue = 5000, int diceCount = 5) => gameManager.CreateNewGame(winningValue, diceCount));

        app.MapGet("/games", () => gameManager.GetGameIds());

        app.MapGet("/games/{guid}", (Guid guid) =>
        {
            var gameState = gameManager.GetGame(guid);
            if (gameState is not null)
            {
                return Results.Ok(gameState);
            }
            return Results.NotFound();
        });

        app.Run();
    }
}
