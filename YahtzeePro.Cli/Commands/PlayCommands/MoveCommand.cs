using System.CommandLine;
using YahtzeePro.Cli.Services;
using YahtzeePro.Core.Models;

namespace YahtzeePro.Cli.Commands.PlayCommands;

public class MoveCommand : Command
{
    public MoveCommand(ICommandService commandService)
        : base("move", "Move in a game")
    {
        var moveArg = new Argument<MoveChoice>("The move to make in the game. Risky or Safe");
        Add(moveArg);
        
        var gameOption = new Option<Guid>("--gameId", "The game ID") { IsRequired = true };
        Add(gameOption);

        this.SetHandler(commandService.Move, moveArg, gameOption);
    }
}