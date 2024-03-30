using YahtzeePro;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddConsole();

        var app = builder.Build();

        IOptimumStrategyRepository _optimumStrategyRepository = new OptimumStrategyFileStorage();

        app.MapGet("/", () => _optimumStrategyRepository.Get());

        app.MapGet("/calculate", (int winningValue = 1000, int diceCount = 5) => CalculateOptimum(winningValue, diceCount));

        app.Run();
    }

    private static string CalculateOptimum(int winningValue, int diceCount, bool forceRecalculation = false)
    {
        Console.WriteLine("Calculate Optimum YatzeePro Strategy...\n");

        int initialStackCounterToReturnKnownValue = 2;
        int calculationIterations = 3;
        bool logAll = false;
        
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
            OptimumCalculator ProbabilitiesCalculator = new(
                winningValue,
                diceCount,
                initialStackCounterToReturnKnownValue,
                calculationIterations,
                logAll);

            var gameStateProbabilities = ProbabilitiesCalculator.Calculate();
            
            optimumStrategyRepository.Save(winningValue, diceCount, gameStateProbabilities);

            return $"Saved //Win{winningValue}//Dice{diceCount}";
        }
    }
}