using System;
using System.Threading;
using YahtzeePro.Play;
using YahtzeePro.Play.Players;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Duel!");

        IPlayer player1 = new Strategy1();
        IPlayer player2 = new Strategy3();

        var setOfGames = new SetOfGames(player1, player2);

        setOfGames.PlaySetOfSets(
            totalGames: 1000,
            totalSets: 1000, 
            logging: true);
    }
}