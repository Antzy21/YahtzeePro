﻿using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace YahtzeePro
{
    public class ProbabilitiesCalculator
    {
        // For different quantities of dice to roll, there will be different scores and probabilities.
        // The RollProbabilities class generates the scores and probabilities for each.
        private readonly Dictionary<int, RollPosibilities> _rollPosibilitiesDictionary = new();
        private readonly bool _logAll = false;
        private readonly int _winningValue;
        private readonly int _totalDice;
        private readonly string _dir;
        // To avoid infinite loops, once this counter reaches zero on the stack, return the known existing value.
        private readonly int _initialStackCounterToReturnKnownValue;
        private readonly int _calculationIterations;
        private GameState? _currentCalculatingGs;
        private readonly HashSet<GameState> _gameStateThatHaveBeenCalculated = new() { };

        public Dictionary<GameState, double> gameStateProbabilities = new() { };
        public Dictionary<GameState, double> gameStateProbabilitiesRisky = new();
        public Dictionary<GameState, double> gameStateProbabilitiesSafe = new();

        public ProbabilitiesCalculator(
            int winningValue,
            int totalDice,
            int initialStackCounterToReturnKnownValue = 3,
            int calculationIterations = 10,
            bool logAll = false)
        {
            _winningValue = winningValue;
            _totalDice = totalDice;
            _initialStackCounterToReturnKnownValue = initialStackCounterToReturnKnownValue;
            _calculationIterations = calculationIterations;
            _logAll = logAll;
            _dir = $"../../../../Win{_winningValue}/Dice{_totalDice}/";

            Console.WriteLine("New Probabilities Calculator created.");
            Console.WriteLine($"Win Value: {_winningValue}");
            Console.WriteLine($"Total Dice: {_totalDice}");

            for (int i = 1; i <= _totalDice; i++)
            {
                _rollPosibilitiesDictionary.Add(i, new RollPosibilities(i));
            }
            _calculationIterations = calculationIterations;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var (gs, _) in gameStateProbabilities)
            {
                stringBuilder.AppendLine(GsDataToString(gs));
            }
            return stringBuilder.ToString();
        }

        public void WriteDataToFile(string fileName)
        {
            Console.WriteLine($"Writing data to {fileName}");

            Directory.CreateDirectory(_dir);
            var file = File.CreateText(_dir+fileName);

            foreach (var (gs, probability) in gameStateProbabilities)
            {
                var gsSerialised = JsonSerializer.Serialize(gs);
                file.Write(gsSerialised);
                file.WriteLine($"---{probability}---{gameStateProbabilitiesRisky[gs]}---{gameStateProbabilitiesSafe[gs]}");
            }

            file.Close();
        }

        public void ReadDataFromFile(string fileName)
        {
            var gsDataLines = File.ReadAllLines(_dir+fileName);

            Console.WriteLine($"Reading data from {_dir + fileName}. {gsDataLines.Length} lines.");

            foreach (var gsData in gsDataLines)
            {
                var gsSerialised = gsData.Split("---")[0];
                var probability = gsData.Split("---")[1];
                var rollProbability = gsData.Split("---")[2];
                var bankProbability = gsData.Split("---")[3];

                var gs = JsonSerializer.Deserialize<GameState>(gsSerialised)!;

                gameStateProbabilities[gs] = double.Parse(probability);
                gameStateProbabilitiesRisky[gs] = double.Parse(rollProbability);
                gameStateProbabilitiesSafe[gs] = double.Parse(bankProbability);
            }

            Console.WriteLine("Finished reading");
        }

        // The main function
        public void PopulateGameStateProbabilities()
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var LoggingInterval = new TimeSpan(0, 0, 1);
            var NextLoggingTime = timer.Elapsed;
            Console.WriteLine("\nBegin populating...\n");

            for (int playerScore = _winningValue - 50; playerScore >= 0; playerScore -= 50)
            {
                for (int opponentScore = _winningValue - 50; opponentScore >= 0; opponentScore -= 50)
                {
                    for (int cachedScore = _winningValue - playerScore; cachedScore >= 0; cachedScore -= 50)
                    {
                        for (int diceCount = _totalDice; diceCount > 0; diceCount--)
                        {
                            foreach (bool isStartOfTurn in new[]{ true, false })
                            {
                                // Impossible to have no cache not at start of turn
                                if (cachedScore == 0 && !isStartOfTurn)
                                { continue; }

                                // New gs to test
                                _currentCalculatingGs = new GameState(playerScore, opponentScore, cachedScore, diceCount, isStartOfTurn);

                                // Assume bank to start with.
                                gameStateProbabilitiesRisky[_currentCalculatingGs] = 0;
                                gameStateProbabilitiesSafe[_currentCalculatingGs] = 1;

                                // Make more accurate estimate
                                // Iterate a few times to improve the estimate.
                                for (int i = 0; i < _calculationIterations; i++)
                                {
                                    gameStateProbabilities[_currentCalculatingGs] = ProbabilityOfWinningFromGs(
                                        _currentCalculatingGs,
                                        _initialStackCounterToReturnKnownValue);
                                }

                                _gameStateThatHaveBeenCalculated.Add(_currentCalculatingGs);

                                if (_logAll || timer.Elapsed > NextLoggingTime)
                                {
                                    NextLoggingTime = timer.Elapsed + LoggingInterval;
                                    Console.WriteLine(GsDataToString(_currentCalculatingGs));
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("\nFinished populating\n");
        }

        private double ProbabilityOfWinningFromGs(GameState gs, int stackCounterToReturnKnownValue, int rollsThisTurn = 0)
        {
            // stackCounterToReturnKnownValue makes sure that the existing preset value (which is needed to avoid stack overflow),
            // is not immediately returned. Once it is zero, it will return.
            if (stackCounterToReturnKnownValue == 0 || _gameStateThatHaveBeenCalculated.Contains(gs))
            {
                return GetGameStateProbability(gs);
            }

            // Default cases
            if (gs.PlayerScore + gs.CachedScore >= _winningValue)
            {
                if (gs.OpponentScore >= _winningValue)
                {
                    throw new Exception("Both players are in a winning state.");
                }
                return 1;
            }
            else if (gs.OpponentScore >= _winningValue)
            {
                return 0;
            }


            /// This is the risky play
            /// ####################
            var rollScoreProbability = ProbabilityOfWinningIfRolling(gs, rollsThisTurn, stackCounterToReturnKnownValue);
            /// ####################

            // This will either be:
            // Choosing to reset the cache at start of turn
            // Choosing to bank at subsequent rolls 
            double safePlayProbability;

            // Can't bank if it's the start of their turn. 
            // Has to choose to reset cache or not!
            if (gs.IsStartOfTurn)
            {
                var resetCacheGs = new GameState(
                    PlayerScore: gs.PlayerScore,
                    OpponentScore: gs.OpponentScore,
                    CachedScore: 0,
                    DiceToRoll: gs.DiceToRoll,
                    IsStartOfTurn: false
                );

                /// ###############
                safePlayProbability = ProbabilityOfWinningIfRolling(resetCacheGs, rollsThisTurn, stackCounterToReturnKnownValue);
                /// ###############
            }
            else
            {
                /// ###############
                safePlayProbability = ProbabilityOfWinningIfBanking(gs, stackCounterToReturnKnownValue);
                /// ###############
            }

            if (gs == _currentCalculatingGs)
            {
                gameStateProbabilitiesSafe[gs] = safePlayProbability;
                gameStateProbabilitiesRisky[gs] = rollScoreProbability;
                gameStateProbabilities[gs] = Math.Max(safePlayProbability, rollScoreProbability);
            }

            return Math.Max(safePlayProbability, rollScoreProbability);
        }

        private double ProbabilityOfWinningIfBanking(GameState gs, int stackCounterToReturnKnownValue)
        {
            var newGs = new GameState(
                PlayerScore: gs.OpponentScore,
                OpponentScore: gs.PlayerScore + gs.CachedScore,
                CachedScore: gs.CachedScore,
                DiceToRoll: gs.DiceToRoll,
                IsStartOfTurn: true
                );

            return 1 - ProbabilityOfWinningFromGs(newGs, stackCounterToReturnKnownValue: stackCounterToReturnKnownValue - 1);
        }

        private double ProbabilityOfWinningIfRolling(GameState gs, int rollsThisTurn, int stackCounterToReturnKnownValue)
        {
            double TotalScore = 0;

            var rollCalculator = _rollPosibilitiesDictionary[gs.DiceToRoll];

            foreach (var (diceUsed, scoreToProbabilities) in rollCalculator.ProbabilitiesOfScores)
            {
                foreach (var (score, probability) in scoreToProbabilities)
                {
                    // Fail!!
                    if (score.Value == 0)
                    {
                        var newGs = new GameState(
                            PlayerScore: gs.OpponentScore,
                            OpponentScore: gs.PlayerScore,
                            CachedScore: 0,
                            DiceToRoll: _totalDice,
                            IsStartOfTurn: true
                        );

                        // Goes to opponent.
                        TotalScore += 1 - ProbabilityOfWinningFromGs(newGs, stackCounterToReturnKnownValue - 1, rollsThisTurn: 0) * probability;
                    }
                    else if (diceUsed.Value == gs.DiceToRoll)
                    {
                        // Roll over!!!!
                        var newGs = new GameState(
                            PlayerScore: gs.PlayerScore,
                            OpponentScore: gs.OpponentScore,
                            CachedScore: gs.CachedScore + score.Value,
                            DiceToRoll: _totalDice,
                            IsStartOfTurn: false
                        );

                        TotalScore += ProbabilityOfWinningFromGs(newGs, stackCounterToReturnKnownValue - 1, rollsThisTurn + 1) * probability;
                    }
                    else
                    {
                        var newGs = new GameState(
                            PlayerScore: gs.PlayerScore,
                            OpponentScore: gs.OpponentScore,
                            CachedScore: gs.CachedScore + score.Value,
                            DiceToRoll: _totalDice - diceUsed.Value,
                            IsStartOfTurn: false
                        );

                        TotalScore += ProbabilityOfWinningFromGs(newGs, stackCounterToReturnKnownValue - 1, rollsThisTurn + 1) * probability;
                    }
                }
            }

            return TotalScore;
        }

        // If the value doesnt exist in the dictionary, mock it at 0.5 to avoid infinite loops
        private double GetGameStateProbability(GameState gs)
        {
            if (gameStateProbabilities.TryGetValue(gs, out var probability))
            {
                return probability;
            }
            return 0.5;
        }

        private bool ShouldRoll(GameState gs, out double probability)
        {
            if (gameStateProbabilitiesRisky.TryGetValue(gs, out var probabilityRisky)){
                if (gameStateProbabilitiesSafe.TryGetValue(gs, out var probabilitySafe))
                {
                    probability = Math.Max(probabilityRisky, probabilitySafe);
                    return probabilityRisky > probabilitySafe;
                }
            }
            throw new Exception("No values found");
        }

        private string GsDataToString(GameState gs)
        {
            return $" {gs.DiceToRoll} Dice, New turn: {gs.IsStartOfTurn,5}" +
                $" | {gs.PlayerScore,4} + {gs.CachedScore,4} : {gs.OpponentScore,4}" +
                $" | Best: {(ShouldRoll(gs, out _) ? 'R' : 'S')}" +
                $" | R {gameStateProbabilitiesRisky.FirstOrDefault(kvp => kvp.Key == gs).Value,6:#.####} | S {gameStateProbabilitiesSafe.FirstOrDefault(kvp => kvp.Key == gs).Value,6:#.####}";
        }
    }
}
