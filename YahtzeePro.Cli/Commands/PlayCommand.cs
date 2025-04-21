using System.CommandLine;
using YahtzeePro.Cli.Commands.PlayCommands;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands;

internal class PlayCommand : Command
{
    public PlayCommand(ICommandService commandService)
        : base("play", "Play a game of Yahtzee Pro")
    {
        Add(new ListGamesCommand(commandService));
        Add(new NewGameCommand(commandService));
        Add(new MoveCommand(commandService));
    }
}
