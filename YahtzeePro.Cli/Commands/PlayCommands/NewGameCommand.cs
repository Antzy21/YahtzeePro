using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands;

public class NewGameCommand : Command
{
    public NewGameCommand(ICommandService commandService)
        : base("new", "Create a new game")
    {
        var winningValueOption = new Option<int>("--target", description: "Winning value", getDefaultValue: () => 5000);
        var totalDiceOption = new Option<int>("--dice", description: "Total dice", getDefaultValue: () => 5);
        AddOption(winningValueOption);
        AddOption(totalDiceOption);

        this.SetHandler(commandService.NewGame, winningValueOption, totalDiceOption);
    }
}