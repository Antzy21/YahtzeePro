using System;
using YahtzeePro.Play;
using YahtzeePro.Play.Players;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Duel!");

        IPlayer player1 = new Strategy1();
        IPlayer player2 = new Strategy2();

        var setOfGames = new SetOfGames(player1, player2);

        for (int i = 0; i < 5; i++)
        {
            setOfGames.PlaySetOfSets(
                totalGames: 10000,
                totalSets: 1000,
                logging: true);
        }
    }
}