using System;
using YahtzeePro.Optimum_strategy;
using YahtzeePro.Play;
using YahtzeePro.Play.Players;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!\n");

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

        Console.WriteLine("Duel!");

        IPlayer player1 = new RollToWin();
        IPlayer player2 = new OptimumPlayer("scores.txt", winningValue, totalDice);

        var setOfGames = new SetOfGames(player1, player2, winningValue, totalDice);

        for (int i = 0; i < 5; i++)
        {
            setOfGames.PlaySetOfSets(
                totalGames: 10000,
                totalSets: 1000,
                logging: true);
        }
    }
}