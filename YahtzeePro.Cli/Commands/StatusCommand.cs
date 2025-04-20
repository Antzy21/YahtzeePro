using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands;

internal class StatusCommand : Command
{
    public StatusCommand(ICommandService commandService)
        : base("status", "Status of configured Command Service")
    {
        this.SetHandler(commandService.Status);
    }
}