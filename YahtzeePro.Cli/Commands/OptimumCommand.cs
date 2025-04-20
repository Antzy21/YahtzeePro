using System.CommandLine;
using YahtzeePro.Cli.Commands.OptimumCommands;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands;

public class OptimumCommand : Command
{
    public OptimumCommand(ICommandService commandService)
        : base("optimum", "View and create calculated Optimum stragies")
    {
        Add(new CalculateOptimumCommand(commandService));
        Add(new GetOptimumCommand(commandService));
        Add(new ListOptimumCommand(commandService));
    }
}