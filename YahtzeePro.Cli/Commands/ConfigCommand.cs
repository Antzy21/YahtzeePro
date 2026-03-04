using System.CommandLine;
using YahtzeePro.Cli.Commands.ConfigCommands;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands;

internal class ConfigCommand : Command
{
    public ConfigCommand(ICommandService commandService)
        : base("config", "View and set config for the CLI")
    {
        Add(new SetConfigCommand(commandService));
        Add(new ListConfigCommand(commandService));
    }
}