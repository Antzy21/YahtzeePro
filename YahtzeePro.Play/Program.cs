using System;
using Microsoft.Extensions.Logging;
using YahtzeePro;
using YahtzeePro.Optimum_strategy;
using YahtzeePro.Play;
using YahtzeePro.Play.Players;

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

        IPlayer player1 = new RollToWin();

        // Create an ILoggerFactory
        ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        // Create an ILogger for OptimumStrategyFileStorage
        var logger = factory.CreateLogger<IOptimumStrategyRepository>();

        IOptimumStrategyRepository optimumStrategyRepository = new OptimumStrategyFileStorage(logger);
        var optimumStrategyData = optimumStrategyRepository.Get(gameConfiguration);
        IPlayer player2 = new OptimumPlayer(optimumStrategyData);

        var setOfGames = new SetOfGames(player1, player2, gameConfiguration);

        setOfGames.PlaySetOfSets(
            totalGames: 100,
            totalSets: 100,
            logging: true);
    }
}