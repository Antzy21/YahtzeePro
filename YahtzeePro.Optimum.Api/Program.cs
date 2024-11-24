using YahtzeePro.models;
using YahtzeePro.Optimum;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddConsole();
        builder.Services.AddScoped<IOptimumStrategyRepository, OptimumStrategyFileStorage>();
        builder.Services.AddScoped<IOptimumCalculator, OptimumCalculator>();
        builder.Services.AddScoped<ICalculationManager, CalculationManager>();

        var app = builder.Build();

        var optimumStrategyRepository = app.Services.GetRequiredService<IOptimumStrategyRepository>();
        var calculationManager = app.Services.GetRequiredService<ICalculationManager>();

        app.MapGet("/", () => optimumStrategyRepository.Get());

        app.MapGet("/queued", () => calculationManager.Queue);

        app.MapPost("/calculate", (GameConfiguration gameConfiguration, bool forceRecalculation = false) => {
            
            calculationManager.QueueCalculation(gameConfiguration);

            return $"Calculating optimum queued for: Win{gameConfiguration.WinningValue} Dice{gameConfiguration.TotalDice}";
        });

        app.MapPost("/getStrategy", (GameConfiguration gameConfiguration) => {
            
            var optimumStrategy = optimumStrategyRepository.Get(gameConfiguration);

            if (optimumStrategy is null)
            {
                return Results.NotFound();
            }
            
            return Results.Json(optimumStrategy.ToList());
        });

        app.MapPost("/getMove", (GameState gameState) => {
            
            var optimumStrategy = optimumStrategyRepository.Get(gameState.GameConfiguration);

            if (optimumStrategy is null)
            {
                return Results.NotFound();
            }
            
            return Results.Json(optimumStrategy.ToList());
        });

        app.Run();
    }
}