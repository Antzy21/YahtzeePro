using System.CommandLine;
using YahtzeePro.models;
using YahtzeePro.Optimum;

namespace YahtzeePro.Cli.Commands.OptimumCommands;

public class CalculateOptimumCommand : Command
{
    public CalculateOptimumCommand(IOptimumCalculator optimumCalculator, IOptimumStrategyRepository optimumStrategyRepository)
        : base("calculate", "Calculate and save the optimum for a gameConfiguration")
    {
        var winningValueArg = new Argument<int>("Winning value");
        var totalDiceArg = new Argument<int>("Total dice");
        AddArgument(winningValueArg);
        AddArgument(totalDiceArg);

        this.SetHandler((winningValue, totalDice) =>
        {
            var gameConfiguration = new GameConfiguration(winningValue, totalDice);

            var result = optimumCalculator.Calculate(gameConfiguration);

            optimumStrategyRepository.Save(gameConfiguration, result);

            Console.WriteLine($"Calculated optimum for {gameConfiguration}");

        }, winningValueArg, totalDiceArg);
    }
}