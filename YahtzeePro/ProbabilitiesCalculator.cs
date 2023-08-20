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
        // To avoid infinite loops, once this counter reaches zero on the stack, return the known existing value.
        private readonly int _initialStackCounterToReturnKnownValue;
        private readonly int _calculationIterations;
        private GameState? _currentCalculatingGs;
        private readonly HashSet<GameState> _gameStateThatHaveBeenCalculated = new() { };

        public Dictionary<GameState, double> gameStateProbabilities = new() { };
        public Dictionary<GameState, bool> gameStateShouldRoll = new();

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
            foreach (var (gs, probability) in gameStateProbabilities)
            {
                stringBuilder.AppendLine(GsDataToString(gs));
            }
            return stringBuilder.ToString();
        }

        public void WriteDataToFile(string fileName)
        {
            Console.WriteLine($"Writing data to {fileName}");

            var file = File.CreateText(fileName);

            foreach (var (gs, probability) in gameStateProbabilities)
            {
                var gsSerialised = JsonSerializer.Serialize(gs);
                file.Write(gsSerialised);
                file.WriteLine($"---{probability}---{gameStateShouldRoll[gs]}");
            }

            file.Close();
        }

        public void ReadDataFromFile(string fileName)
        {
            Console.WriteLine($"Reading data from {fileName}");

            var gsDataLines = File.ReadAllLines(fileName);

            foreach (var gsData in gsDataLines)
            {
                var gsSerialised = gsData.Split("---")[0];
                var probability = gsData.Split("---")[1];
                var shouldRoll = gsData.Split("---")[2];

                var gs = JsonSerializer.Deserialize<GameState>(gsSerialised);

                gameStateProbabilities[gs] = double.Parse(probability);
                gameStateShouldRoll[gs] = bool.Parse(shouldRoll);
            }
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
                            // New gs to test
                            _currentCalculatingGs = new GameState(playerScore, opponentScore, cachedScore, diceCount);

                            // Assume bank to start with.
                            gameStateShouldRoll[_currentCalculatingGs] = false;

                            // Make more accurate estimate
                            // Iterate a few times to improve the estimate.
                            for (int i = 0; i < _calculationIterations; i++)
                            {
                                gameStateProbabilities[_currentCalculatingGs] = ProbabilityOfWinningFromGs(
                                    _currentCalculatingGs,
                                    _initialStackCounterToReturnKnownValue,
                                    diceCount);
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

            /// #########################################################
            /// 
            var rollScoreProbability = ProbabilityOfWinningIfRolling(gs, rollsThisTurn, stackCounterToReturnKnownValue);
            ///
            /// #########################################################

            // Can't bank if they haven't rolled yet. 
            if (rollsThisTurn == 0)
            {
                gameStateShouldRoll[gs] = true;
                return rollScoreProbability;
            }

            /// #########################################################
            ///
            var bankScoreProbability = ProbabilityOfWinningIfBanking(gs, stackCounterToReturnKnownValue);
            /// 
            /// #########################################################

            if (gs == _currentCalculatingGs)
            {
                gameStateShouldRoll[gs] = rollScoreProbability > bankScoreProbability;
            }

            return Math.Max(bankScoreProbability, rollScoreProbability);
        }

        private double ProbabilityOfWinningIfBanking(GameState gs, int stackCounterToReturnKnownValue)
        {
            var resetDiceGs = new GameState(
                PlayerScore: gs.OpponentScore,
                OpponentScore: gs.PlayerScore + gs.CachedScore,
                CachedScore: 0,
                DiceToRoll: _totalDice
                );

            var resetDiceProability = ProbabilityOfWinningFromGs(resetDiceGs, stackCounterToReturnKnownValue: stackCounterToReturnKnownValue - 1);

            var continueDiceGs = new GameState(
                PlayerScore: gs.OpponentScore,
                OpponentScore: gs.PlayerScore + gs.CachedScore,
                CachedScore: gs.CachedScore,
                DiceToRoll: gs.DiceToRoll
                );

            var continueDiceProability = ProbabilityOfWinningFromGs(continueDiceGs, stackCounterToReturnKnownValue: stackCounterToReturnKnownValue - 1);

            return 1 - Math.Max(resetDiceProability, continueDiceProability);
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
                    if (score.score == 0)
                    {
                        var newGs = new GameState(
                            PlayerScore: gs.OpponentScore,
                            OpponentScore: gs.PlayerScore,
                            CachedScore: 0,
                            DiceToRoll: _totalDice
                        );

                        // Goes to opponent.
                        TotalScore += 1 - ProbabilityOfWinningFromGs(newGs, stackCounterToReturnKnownValue: stackCounterToReturnKnownValue - 1, rollsThisTurn: 0) * probability;
                    }
                    else if (diceUsed.valueAddingDice == gs.DiceToRoll)
                    {
                        // Roll over!!!!
                        var newGs = new GameState(
                            PlayerScore: gs.PlayerScore,
                            OpponentScore: gs.OpponentScore,
                            CachedScore: gs.CachedScore + score.score,
                            DiceToRoll: _totalDice
                        );

                        TotalScore += ProbabilityOfWinningFromGs(newGs, stackCounterToReturnKnownValue: stackCounterToReturnKnownValue - 1, rollsThisTurn: rollsThisTurn + 1) * probability;
                    }
                    else
                    {
                        var newGs = new GameState(
                            PlayerScore: gs.PlayerScore,
                            OpponentScore: gs.OpponentScore,
                            CachedScore: gs.CachedScore + score.score,
                            DiceToRoll: _totalDice - diceUsed.valueAddingDice
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

        private string GsDataToString(GameState gs)
        {
            var bestMove = gameStateShouldRoll[gs] ? "roll" : "bank";
            return $" ({gs.DiceToRoll}Ds) {gs.PlayerScore,4} + {gs.CachedScore,4} - {gs.OpponentScore,4}" +
                $" => Prob: {gameStateProbabilities[gs],4:#.##}" +
                $" => {bestMove}";
        }
    }
}
