using System;

namespace YahtzeePro.Play
{
    internal class SetOfGames
    {
        private readonly GameConfiguration _gameConfiguration;
        private readonly IPlayer _player1;
        private readonly IPlayer _player2;

        public SetOfGames(IPlayer player1, IPlayer player2, GameConfiguration gameConfiguration)
        {
            _player1 = player1;
            _player2 = player2;
            _gameConfiguration = gameConfiguration;
        }

        public void PlaySetOfSets(int totalGames, int totalSets, bool logging = false)
        {
            int player1SetWins = 0;
            int player2SetWins = 0;

            Console.WriteLine($"Player 1 \"{_player1.Name}\"");
            Console.WriteLine($"Player 2 \"{_player2.Name}\"");
            Console.WriteLine($"Games per set: {totalGames}\n");
            Console.WriteLine("Set | P1 : P2");

            for (int set = 1; set <= totalSets; set++)
            {
                (int player1WinCount, int player2WinCount) = PlayGames(totalGames);

                if (player1WinCount > player2WinCount)
                    player1SetWins++;
                else if (player2WinCount > player1WinCount)
                    player2SetWins++;

                if (logging)
                {
                    Console.Write($"\r{set,3} |{player1SetWins,3} :{player2SetWins,3}");
                }
            }
            Console.WriteLine("\n");
        }

        public Tuple<int, int> PlayGames(int totalGames)
        {
            int player1WinCount = 0;
            int player2WinCount = 0;

            // Player 1 first
            for (int i = 0; i < totalGames / 2; i++)
            {
                var game = new Game(_gameConfiguration, _player1, _player2);
                game.Play();
                if (game.firstPlayerWon)
                    player1WinCount++;
                else
                    player2WinCount++;
            }

            // Player 2 first
            for (int i = 0; i < totalGames / 2; i++)
            {
                var game = new Game(_gameConfiguration, _player2, _player1);
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
