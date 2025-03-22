using Microsoft.Extensions.DependencyInjection;
using YahtzeePro.Optimum;
using YahtzeePro.Cli;

internal class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddScoped<IOptimumStrategyRepository, OptimumStrategyFileStorage>();
        services.AddScoped<IOptimumCalculator, OptimumCalculator>();
        services.AddScoped<ICalculationManager, CalculationManager>();
        services.AddSingleton<CommandService, CommandService>();
        
        var serviceProvider = services.BuildServiceProvider();

        var commandService = serviceProvider.GetRequiredService<CommandService>();
        
        commandService.Invoke(args);
    }
}
