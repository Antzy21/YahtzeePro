using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands.OptimumCommands;

public class ListOptimumCommand : Command
{
    public ListOptimumCommand(ICommandService commandService)
        : base("list", "List all optimums that have been calculated")
    {
        this.SetHandler(commandService.ListOptimums);
    }
}
