using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using YahtzeePro.Play.Players;
using YahtzeePro.Play.Players.SimpleStrategy;

namespace YahtzeePro.Play;

internal class Program
{
    private static void Main(string[] args)
    {
        int winningValue = 1000;
        int totalDice = 5;

        if (args.Length > 0)
        {
            winningValue = int.Parse(args[0]);
        }
        if (args.Length > 1)
        {
            totalDice = int.Parse(args[1]);
        }
        if (args.Length > 2)
        {
            Console.WriteLine("Too many arguements passed. Expecting 'winningValue' and 'totalDice'");
        }

        GameConfiguration gameConfiguration = new (winningValue, totalDice);

        Console.WriteLine("Duel!");

        IPlayer rollToWinPlayer = new RollToWin();

        // Create an ILoggerFactory
        ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        // Create an ILogger for OptimumStrategyFileStorage
        var logger = factory.CreateLogger<IOptimumStrategyRepository>();

        IOptimumStrategyRepository optimumStrategyRepository = new OptimumStrategyFileStorage(logger);
        var optimumStrategyData = optimumStrategyRepository.Get(gameConfiguration);
        IPlayer optimumPlayer = new OptimumPlayer(optimumStrategyData);

        var strategy1Json = System.IO.File.ReadAllText("Players/SimpleStrategy/Configurations/strategy1.json");
        var strategy1 = JsonSerializer.Deserialize<SimpleStrategyConfiguration>(strategy1Json);
        Console.WriteLine(strategy1Json);
        Console.WriteLine(strategy1.WhenToBankWithNumberOfDice);
        var strategy1Player = new SimpleStrategy(strategy1);

        var strategy2Json = System.IO.File.ReadAllText("Players/SimpleStrategy/Configurations/strategy2.json");
        var strategy2 = JsonSerializer.Deserialize<SimpleStrategyConfiguration>(strategy2Json);
        
        var strategy2Player = new SimpleStrategy(strategy2);

        var setOfGames = new SetOfGames(strategy1Player, strategy2Player, gameConfiguration);

        setOfGames.PlaySetOfSets(
            totalGames: 100,
            totalSets: 100,
            logging: true);
    }
}