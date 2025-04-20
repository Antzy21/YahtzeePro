using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands.OptimumCommands;

public class CalculateOptimumCommand : Command
{
    public CalculateOptimumCommand(ICommandService commandService)
        : base("calculate", "Calculate and save the optimum for a gameConfiguration")
    {
        var winningValueArg = new Argument<int>("Winning value");
        var totalDiceArg = new Argument<int>("Total dice");
        AddArgument(winningValueArg);
        AddArgument(totalDiceArg);

        this.SetHandler(commandService.CalculateOptimum, winningValueArg, totalDiceArg);
    }
}