using System.CommandLine;
using YahtzeePro.Optimum;

namespace YahtzeePro.Cli.Commands;

public class ListCommand : Command
{
    public ListCommand(IOptimumStrategyRepository optimumRepository)
        : base("list", "List all optimums that have been calculated")
    {
        this.SetHandler(() =>
        {
            var optimums = optimumRepository.Get();

            foreach (var optimum in optimums)
            {
                Console.WriteLine(optimum);
            }

        });
    }
}
