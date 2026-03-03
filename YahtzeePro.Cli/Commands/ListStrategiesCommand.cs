using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands;

internal class ListStrategiesCommand : Command
{
    public ListStrategiesCommand(ICommandService commandService)
        : base("liststrategies", "List available strategies")
    {
        this.SetHandler(commandService.ListStrategies);
    }
}