using System.CommandLine;
using YahtzeePro.Cli.Commands.OptimumCommands;
using YahtzeePro.Optimum;

namespace YahtzeePro.Cli.Commands;

public class OptimumCommand : Command
{
    public OptimumCommand(IOptimumCalculator optimumCalculator, IOptimumStrategyRepository optimumepository)
        : base("optimum", "View and create calculated Optimum stragies")
    {
        Add(new CalculateOptimumCommand(optimumCalculator, optimumepository));
        Add(new GetOptimumCommand(optimumepository));
        Add(new ListOptimumCommand(optimumepository));
    }
}