using System.CommandLine;
using YahtzeePro.Optimum.Cli.Commands;

namespace YahtzeePro.Optimum.Cli;

public class CommandService(IOptimumCalculator optimumCalculator, IOptimumStrategyRepository optimumRepository)
{
    private readonly RootCommand _rootCommand = new();
    private readonly IOptimumCalculator _optimumCalculator = optimumCalculator;
    private readonly IOptimumStrategyRepository _optimumRepository = optimumRepository;

    public void Invoke(string[] args)
    {
        _rootCommand.SetHandler(() =>
        {
            Console.WriteLine("Yahtzee Optimum CLI");
        });

        _rootCommand.Add(new CalculateCommand(_optimumCalculator));
        _rootCommand.Add(new GetCommand(_optimumRepository));

        _rootCommand.Invoke(args);
    }
}