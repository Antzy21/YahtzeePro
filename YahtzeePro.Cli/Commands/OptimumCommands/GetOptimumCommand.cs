using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands.OptimumCommands;

public class GetOptimumCommand : Command
{
    public GetOptimumCommand(ICommandService commandService)
        : base("get", "Get the optimum for a gameConfiguration if it has been calculated")
    {
        var winningValueArg = new Argument<int>("Winning value");
        var totalDiceArg = new Argument<int>("Total dice");
        AddArgument(winningValueArg);
        AddArgument(totalDiceArg);

        this.SetHandler(commandService.GetOptimum, winningValueArg, totalDiceArg);
    }
}
