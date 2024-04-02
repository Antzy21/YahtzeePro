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

        app.MapGet("/calculate", (int winningValue = 1000, int diceCount = 5, bool forceRecalculation = false) => {
            
            var gameConfiguration = new GameConfiguration(winningValue, diceCount);

            calculationManager.QueueCalculation(gameConfiguration);

            return $"Calculating optimum queued for: Win{winningValue} Dice{diceCount}";
        });

        app.MapGet("/getOptimumStrategy", (int winningValue = 1000, int diceCount = 5) => {
            
            var gameConfiguration = new GameConfiguration(winningValue, diceCount);

            var optimumStrategy = optimumStrategyRepository.Get(gameConfiguration);

            if (optimumStrategy is null)
            {
                return Results.NotFound();
            }
            
            return Results.Json(optimumStrategy.ToList());
        });

        app.Run();
    }
}