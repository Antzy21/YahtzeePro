using System.Text.Json;
using YahtzeePro;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddConsole();
        builder.Services.AddScoped<IOptimumStrategyRepository, OptimumStrategyFileStorage>();
        builder.Services.AddScoped<IOptimumCalculator, OptimumCalculator>();

        var app = builder.Build();

        var optimumStrategyRepository = app.Services.GetRequiredService<IOptimumStrategyRepository>();
        var optimumCalculator = app.Services.GetRequiredService<IOptimumCalculator>();

        app.MapGet("/", () => optimumStrategyRepository.Get());

        app.MapGet("/calculate", (int winningValue = 1000, int diceCount = 5, bool forceRecalculation = false) => {
            
            var gameConfiguration = new GameConfiguration(winningValue, diceCount);
            int initialStackCounterToReturnKnownValue = 2;
            int calculationIterations = 3;

            var gameStateProbabilities = optimumCalculator.Calculate(gameConfiguration, initialStackCounterToReturnKnownValue, calculationIterations);

            optimumStrategyRepository.Save(gameConfiguration, gameStateProbabilities);

            return $"Calculated optimum for: Win{winningValue} Dice{diceCount}";
        });

        app.MapGet("/getOptimumStrategy", (int winningValue = 1000, int diceCount = 5) => {
            
            var gameConfiguration = new GameConfiguration(winningValue, diceCount);

            var optimumStrategy = optimumStrategyRepository.Get(gameConfiguration);

            if (optimumStrategy is null)
            {
                return Results.NotFound();
            }
            
            return Results.Json(JsonSerializer.Serialize(optimumStrategy.GameStateProbabilities.AsEnumerable()));
        });

        app.Run();
    }
}