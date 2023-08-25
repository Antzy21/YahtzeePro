using System;
using System.Threading;
using YahtzeePro.Play.Players;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Duel!");

        int player1WinCount = 0;
        int player2WinCount = 0;

        int totalGames = 1000;

        IPlayer player1 = new Strategy1();
        IPlayer player2 = new Strategy2();

        // Player 1 first
        for (int i = 0; i < totalGames/2; i++)
        {
            var game = new Game(winningValue: 5000, totalDice: 5, player1, player2);
            if (game.player1Won)
                player1WinCount++;
            else
                player2WinCount++;
        }

        // Player 2 first
        for (int i = 0; i < totalGames / 2; i++)
        {
            var game = new Game(winningValue: 5000, totalDice: 5, player2, player1);
            game.Play();
            if (game.player1Won)
                player2WinCount++;
            else
                player1WinCount++;
        }

        Console.WriteLine($"{player1.Name}:\n\t{player1WinCount}");
        Console.WriteLine($"{player2.Name}:\n\t{player2WinCount}");
    }
}