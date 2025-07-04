using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands;

public class SimulateGameCommand : Command
{
    public SimulateGameCommand(ICommandService commandService)
        : base("simulate", "Simulate a game between two strategies")
    {
        var player1Arg = new Argument<string>("player1", description: "Strategy 1");
        AddArgument(player1Arg);
        var player2Arg = new Argument<string>("player2", description: "Strategy 2");
        AddArgument(player2Arg);

        var totalGamesOption = new Option<int>("--gameCount", description: "Total games to simulate", getDefaultValue: () => 1);
        AddOption(totalGamesOption);
        var totalSetsOption = new Option<int>("--setCount", description: "Total sets to simulate", getDefaultValue: () => 1);
        AddOption(totalSetsOption);

        var winningValueOption = new Option<int>("--target", description: "Winning value", getDefaultValue: () => 5000);
        var totalDiceOption = new Option<int>("--dice", description: "Total dice", getDefaultValue: () => 5);
        AddOption(winningValueOption);
        AddOption(totalDiceOption);

        this.SetHandler(commandService.Simulate, player1Arg, player2Arg, totalGamesOption, totalSetsOption, winningValueOption, totalDiceOption);
    }
}