using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands;

internal class PlayCommand : Command
{
    public PlayCommand(ICommandService commandService)
        : base("play", "Play a game of Yahtzee Pro")
    {
        
    }
}