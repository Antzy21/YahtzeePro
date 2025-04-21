using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands.PlayCommands;

public class ListGamesCommand : Command
{
    public ListGamesCommand(ICommandService commandService)
        : base("list", "List all games")
    {
        this.SetHandler(commandService.ListGames);
    }
}