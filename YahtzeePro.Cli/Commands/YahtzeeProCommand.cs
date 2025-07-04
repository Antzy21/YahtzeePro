using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands;

internal class YahtzeeProCommand : RootCommand
{
    public YahtzeeProCommand(ICommandService commandService)
    {
        Add(new OptimumCommand(commandService));
        Add(new PlayCommand(commandService));
        Add(new StatusCommand(commandService));
        Add(new SimulateGameCommand(commandService));
        
        this.SetHandler(() =>
        {
            Console.WriteLine("Yahtzee Optimum CLI");
        });
    }
}