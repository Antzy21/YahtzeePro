using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands.ConfigCommands;

internal class ListConfigCommand : Command
{
    public ListConfigCommand(ICommandService commandService)
        : base("list", "List all config for the CLI")
    {
        this.SetHandler(commandService.ListConfig);
    }
}
