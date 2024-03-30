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
            Console.WriteLine("Calculate Optimum YatzeePro Strategy...\n");

            int initialStackCounterToReturnKnownValue = 2;
            int calculationIterations = 3;
            
            IOptimumStrategyRepository optimumStrategyRepository = new OptimumStrategyFileStorage();

            var optimumStrategies = optimumStrategyRepository.Get();

            if (!forceRecalculation && optimumStrategies.Contains($"//Win{winningValue}//Dice{diceCount}"))
            {
                optimumStrategyRepository.Get(winningValue, diceCount);
                Console.WriteLine("Finished reading");
                return "Scores exist for this configuration.";
            }
            else
            {
                var gameStateProbabilities = optimumCalculator.Calculate(winningValue, diceCount, initialStackCounterToReturnKnownValue, calculationIterations);

                optimumStrategyRepository.Save(winningValue, diceCount, gameStateProbabilities);

                return $"Saved //Win{winningValue}//Dice{diceCount}";
            }
        });

        app.Run();
    }
}