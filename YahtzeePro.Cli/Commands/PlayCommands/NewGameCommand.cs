using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands.PlayCommands;

public class NewGameCommand : Command
{
    public NewGameCommand(ICommandService commandService)
        : base("new", "Create a new game")
    {
        var opponentArg = new Argument<string>("opponent", description: "Opponent name", getDefaultValue: () => "Computer");
        AddArgument(opponentArg);

        var winningValueOption = new Option<int>("--target", description: "Winning value", getDefaultValue: () => 5000);
        var totalDiceOption = new Option<int>("--dice", description: "Total dice", getDefaultValue: () => 5);
        AddOption(winningValueOption);
        AddOption(totalDiceOption);

        this.SetHandler(commandService.NewGame, opponentArg, winningValueOption, totalDiceOption);
    }
}