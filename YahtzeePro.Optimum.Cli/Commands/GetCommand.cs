using System.CommandLine;
using YahtzeePro.models;

namespace YahtzeePro.Optimum.Cli.Commands;

public class GetCommand : Command
{
    public GetCommand(IOptimumStrategyRepository optimumRepository)
        : base("get", "Get the optimum for a gameConfiguration if it has been calculated")
    {
        var winningValueArg = new Argument<int>("Winning value");
        var totalDiceArg = new Argument<int>("Total dice");
        AddArgument(winningValueArg);
        AddArgument(totalDiceArg);

        this.SetHandler((winningValue, totalDice) =>
        {
            var gameConfiguration = new GameConfiguration(winningValue, totalDice);

            var optimum = optimumRepository.Get(gameConfiguration);

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

        }, winningValueArg, totalDiceArg);
    }
}
