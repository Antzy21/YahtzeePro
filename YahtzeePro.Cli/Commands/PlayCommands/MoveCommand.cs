using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands.PlayCommands;

public class MoveCommand : Command
{
    public MoveCommand(ICommandService commandService)
        : base("move", "Move in a game")
    {
        var moveOption = new Option<Guid>("--gameId", "The game ID") { IsRequired = true };
        Add(moveOption);

        this.SetHandler(commandService.Move, moveOption);
    }
}