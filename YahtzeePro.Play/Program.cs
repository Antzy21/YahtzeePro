using System;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Duel!");

        IPlayer player1 = null;
        IPlayer player2 = null;

        var game = new Game(winningValue: 5000, totalDice: 5, player1, player2);
    }
}