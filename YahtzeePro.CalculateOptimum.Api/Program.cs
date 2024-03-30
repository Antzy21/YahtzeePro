using YahtzeePro;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        
        IOptimumStrategyRepository _optimumStrategyRepository = new OptimumStrategyFileStorage();

        app.MapGet("/", () => _optimumStrategyRepository.Get());

        app.MapGet("/calculate", (int winningValue = 0, int diceCount = 0) => CalculateOptimum(winningValue, diceCount));

        app.Run();
    }

    private static string CalculateOptimum(int winningValue = 0, int diceCount = 0)
    {
        return $"Test success: {winningValue}, {diceCount}";
    }
}