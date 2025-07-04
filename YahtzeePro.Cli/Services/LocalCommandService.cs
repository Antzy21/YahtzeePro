using YahtzeePro.Core.Models;
using YahtzeePro.Optimum;

namespace YahtzeePro.Cli.Services;

public class LocalCommandService(
    ICalculationManager calculationManager,
    IOptimumStrategyRepository optimumStrategyRepository,
    IOptimumCalculator optimumCalculator
    ) : ICommandService {

    private readonly ICalculationManager _calculationManager = calculationManager;
    private readonly IOptimumStrategyRepository _optimumRepository = optimumStrategyRepository;
    private readonly IOptimumCalculator _optimumCalculator = optimumCalculator;

    public void Status() {
        Console.WriteLine("Local command service ready.");
    }

    public void CalculateOptimum(int winningValue, int totalDice) {
        var gameConfiguration = new GameConfiguration(winningValue, totalDice);

        var result = _optimumCalculator.Calculate(gameConfiguration);

        _optimumRepository.Save(gameConfiguration, result);

        Console.WriteLine($"Calculated optimum for {gameConfiguration}");
    }
    
    public void GetOptimum(int winningValue, int totalDice) {
        var gameConfiguration = new GameConfiguration(winningValue, totalDice);

        var optimum = _optimumRepository.Get(gameConfiguration);

        if (optimum is null)
        {
            Console.WriteLine($"No optimum calculated for {gameConfiguration}");
            Console.WriteLine("To calculate for this configuration run:");
            Console.WriteLine($"calculate {winningValue} {totalDice}");
            return;
        }

        foreach (var item in optimum)
        {
            Console.WriteLine(item);
        }
    }

    public void ListOptimums() {
        var optimums = _optimumRepository.Get();

        foreach (var optimum in optimums)
        {
            Console.WriteLine(optimum);
        }
    }

    public void ListGames()
    {
        throw new NotImplementedException();
    }

    public void NewGame(string opponent, int winningValue, int totalDice)
    {
        throw new NotImplementedException();
    }

    public void Move(MoveChoice moveChoice, Guid gameId)
    {
        throw new NotImplementedException();
    }

    public void Simulate(string strategy1, string strategy2, int numberOfGames, int numberOfSets, int winningValue, int totalDice)
    {
        throw new NotImplementedException();
    }
}