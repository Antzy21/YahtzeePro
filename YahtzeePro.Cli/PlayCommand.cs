using System.CommandLine;

namespace YahtzeePro.Cli;

internal class PlayCommand : Command
{
    public PlayCommand()
        : base("play", "Play a game of Yahtzee Pro")
    {
        
    }
}