using System.CommandLine;
using YahtzeePro.Cli.Services;

namespace YahtzeePro.Cli.Commands.ConfigCommands;

internal class SetConfigCommand : Command
{
    public SetConfigCommand(ICommandService commandService)
        : base("set", "Set a configuration variable for the CLI")
    {
        var configVariableTypeArg = new Argument<ConfigVariable>("The configuration variable to set");
        Add(configVariableTypeArg);
        
        var configVariableValueArg = new Argument<string>("The value to be set");
        Add(configVariableValueArg);        

        this.SetHandler(commandService.SetConfig, configVariableTypeArg, configVariableValueArg);       
    }
}