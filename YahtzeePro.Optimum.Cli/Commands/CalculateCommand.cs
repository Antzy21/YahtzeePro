using System.CommandLine;
using YahtzeePro.models;

namespace YahtzeePro.Optimum.Cli.Commands;

public class CalculateCommand : Command
{
    public CalculateCommand(IOptimumCalculator optimumCalculator)
        : base("calculate", "Calculate the optimum for a gameConfiguration")
    {
        var winningValueArg = new Argument<int>("Winning value");
        var totalDiceArg = new Argument<int>("Total dice");
        AddArgument(winningValueArg);
        AddArgument(totalDiceArg);

        this.SetHandler((winningValue, totalDice) =>
        {
            var gameConfiguration = new GameConfiguration(winningValue, totalDice);

            optimumCalculator.Calculate(gameConfiguration);

            Console.WriteLine($"Calculated optimum for {gameConfiguration}");

        }, winningValueArg, totalDiceArg);
    }
}