using System;
using System.Text;

namespace YahtzeePro.Play
{
    internal class SetOfGames
    {

        private readonly IPlayer _player1;
        private readonly IPlayer _player2;

        public SetOfGames(IPlayer player1, IPlayer player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public void PlaySetOfSets(int totalGames, int totalSets, bool logging = false)
        {
            int player1SetWins = 0;
            int player2SetWins = 0;

            Console.WriteLine($"PLayer 1 \"{_player1.Name}\"");
            Console.WriteLine($"Player 2 \"{_player2.Name}\"");
            Console.WriteLine($"Games per set: {totalGames}\n");
            Console.WriteLine("Set |  P1  :  P2");

            for (int set = 1; set <= totalSets; set++)
            {
                var (player1WinCount, player2WinCount) = PlayGames(totalGames, logging);

                if (player1WinCount > player2WinCount)
                    player1SetWins++;
                else if (player2WinCount > player1WinCount)
                    player2SetWins++;

                if (logging)
                {
                    var sb = new StringBuilder();
                    Console.Write($"\r{set,4} | {player1SetWins,4} : {player2SetWins,4}");
                }
            }
            Console.WriteLine("\n");
        }

        public Tuple<int, int> PlayGames(int totalGames, bool logging = false)
        {
            int player1WinCount = 0;
            int player2WinCount = 0;

            // Player 1 first
            for (int i = 0; i < totalGames / 2; i++)
            {
                var game = new Game(winningValue: 5000, totalDice: 5, _player1, _player2);
                game.Play();
                if (game.firstPlayerWon)
                    player1WinCount++;
                else
                    player2WinCount++;
            }

            // Player 2 first
            for (int i = 0; i < totalGames / 2; i++)
            {
                var game = new Game(winningValue: 5000, totalDice: 5, _player2, _player1);
                game.Play();
                if (game.firstPlayerWon)
                    player2WinCount++;
                else
                    player1WinCount++;
            }

            return Tuple.Create(player1WinCount, player2WinCount);
        }
    }
}
