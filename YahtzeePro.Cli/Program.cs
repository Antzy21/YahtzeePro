using Microsoft.Extensions.DependencyInjection;
using YahtzeePro.Optimum;
using YahtzeePro.Cli.Services;
using System.CommandLine;
using YahtzeePro.Cli.Commands;

internal class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddLogging();

        // The service for command handlers in commands
        services.AddScoped<ICommandService, LocalCommandService>();

        // The services required for the local command service to run
        services.AddScoped<IOptimumStrategyRepository, OptimumStrategyFileStorage>();
        services.AddScoped<IOptimumCalculator, OptimumCalculator>();
        services.AddScoped<ICalculationManager, CalculationManager>();

        var serviceProvider = services.BuildServiceProvider();

        var commandService = serviceProvider.GetRequiredService<ICommandService>();
        RootCommand rootCommand = new YahtzeeProCommand(commandService);

        rootCommand.Invoke(args);
    }
}
