using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace YahtzeePro
{
    public class OptimumCalculator : IOptimumCalculator
    {
        // For different quantities of dice to roll, there will be different scores and probabilities.
        // The RollProbabilities class generates the scores and probabilities for each.
        private readonly Dictionary<int, RollPossibilities> _rollPosibilitiesDictionary = new();
        private int _winningValue;
        private int _totalDice;

        private readonly ILogger _logger;

        private readonly Dictionary<GameState, double> gameStateProbabilities = [];
        private readonly Dictionary<GameState, double> gameStateProbabilitiesRisky = [];
        private readonly Dictionary<GameState, double> gameStateProbabilitiesSafe = [];


        // To avoid infinite loops, once this counter reaches zero on the stack, return the known existing value.
        private int _initialStackCounterToReturnKnownValue;
        private int _calculationIterations;
        private GameState _currentCalculatingGs;
        private readonly HashSet<GameState> _gameStateThatHaveBeenCalculated = new();

        public OptimumCalculator(ILogger<OptimumCalculator> logger)
        {
            _logger = logger;
        }

        // The main function
        public Dictionary<GameState, GameStateProbabilities> Calculate(
            GameConfiguration gameConfiguration,
            int initialStackCounterToReturnKnownValue = 2,
            int calculationIterations = 3)
        {
            _logger.LogInformation("Calculate Optimum YatzeePro Strategy...");

            _winningValue = gameConfiguration.WinningValue;
            _totalDice = gameConfiguration.TotalDice;
            _initialStackCounterToReturnKnownValue = initialStackCounterToReturnKnownValue;
            _calculationIterations = calculationIterations;
            
            _currentCalculatingGs = new GameState(_winningValue, _winningValue, 0, _totalDice, true, _totalDice);
            
            _logger.LogInformation($"Win Value: {_winningValue}");
            _logger.LogInformation($"Total Dice: {_totalDice}");

            for (int i = 1; i <= _totalDice; i++)
            {
                _rollPosibilitiesDictionary[i] = new RollPossibilities(i);
            }

            Stopwatch timer = Stopwatch.StartNew();
            TimeSpan LoggingInterval = new(0, 0, seconds: 5);
            TimeSpan NextLoggingTime = timer.Elapsed;
            _logger.LogInformation("Begin populating");

            Dictionary<GameState, GameStateProbabilities> GameStateProbabilities = [];

            for (int playerScore = _winningValue - 50; playerScore >= 0; playerScore -= 50)
            {
                for (int opponentScore = _winningValue - 50; opponentScore >= 0; opponentScore -= 50)
                {
                    for (int cachedScore = _winningValue - playerScore; cachedScore >= 0; cachedScore -= 50)
                    {
                        for (int diceCount = _totalDice; diceCount > 0; diceCount--)
                        {
                            foreach (bool isStartOfTurn in new[] { true, false })
                            {
                                // Impossible to have no cache not at start of turn
                                if (cachedScore == 0 && !isStartOfTurn)
                                { continue; }

                                // Impossible to have cache at start of turn if all dice are available
                                if (cachedScore != 0 && isStartOfTurn && diceCount == _totalDice)
                                { continue; }

                                // Impossible to have a cache larger than opponent's score at the start of turn
                                if (cachedScore > opponentScore && isStartOfTurn)
                                { continue; }

                                // New gs to test
                                _currentCalculatingGs = new GameState(playerScore, opponentScore, cachedScore, diceCount, isStartOfTurn, _totalDice);

                                // Assume safe strategy at gs is better to start with.
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
                                GameStateProbabilities.Add(_currentCalculatingGs, new GameStateProbabilities(
                                    gameStateProbabilitiesRisky[_currentCalculatingGs] > gameStateProbabilitiesSafe[_currentCalculatingGs],
                                    gameStateProbabilitiesRisky[_currentCalculatingGs],
                                    gameStateProbabilitiesSafe[_currentCalculatingGs]
                                ));
                                
                                if (timer.Elapsed > NextLoggingTime)
                                {
                                    NextLoggingTime = timer.Elapsed + LoggingInterval;
                                    _logger.LogInformation(GsDataToString(_currentCalculatingGs));
                                }
                                else {
                                    _logger.LogDebug(GsDataToString(_currentCalculatingGs));
                                } 
                            }
                        }
                    }
                }
            }

            _logger.LogInformation("Finished populating");
            return GameStateProbabilities;
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
                    throw new Exception("Both players are in a winning state.");
                return 1;
            }
            else if (gs.OpponentScore >= _winningValue)
            {
                return 0;
            }

            /// This is the risky play
            /// ####################
            double rollScoreProbability = ProbabilityOfWinningIfRolling(gs, rollsThisTurn, stackCounterToReturnKnownValue);
            /// ####################

            // This will either be:
            // Choosing to reset the cache at start of turn
            // Choosing to bank at subsequent rolls 
            double safePlayProbability;

            // Can't bank if it's the start of their turn. 
            // Has to choose to reset cache or not!
            if (gs.IsStartOfTurn)
            {
                GameState resetCacheGs = gs.ResetCache();

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

            if (gs.Equals(_currentCalculatingGs))
            {
                if (safePlayProbability > 1 || safePlayProbability < 0)
                    throw new Exception($"Probability for safe play {safePlayProbability} is out of bounds for gs:{_currentCalculatingGs}");
                if (rollScoreProbability > 1 || rollScoreProbability < 0)
                    throw new Exception($"Probability for rolling {rollScoreProbability} is out of bounds for gs:{_currentCalculatingGs}");

                gameStateProbabilitiesSafe[gs] = safePlayProbability;
                gameStateProbabilitiesRisky[gs] = rollScoreProbability;
                gameStateProbabilities[gs] = Math.Max(safePlayProbability, rollScoreProbability);
            }

            return Math.Max(safePlayProbability, rollScoreProbability);
        }

        private double ProbabilityOfWinningIfBanking(GameState gs, int stackCounterToReturnKnownValue)
        {
            GameState newGs = gs.Bank();

            // Goes to opponent.
            return 1 - ProbabilityOfWinningFromGs(newGs, stackCounterToReturnKnownValue: stackCounterToReturnKnownValue - 1);
        }

        private double ProbabilityOfWinningIfRolling(GameState gs, int rollsThisTurn, int stackCounterToReturnKnownValue)
        {
            double TotalScore = 0;

            RollPossibilities rollCalculator = _rollPosibilitiesDictionary[gs.DiceToRoll];

            foreach ((int diceUsed, Dictionary<int, double> scoreToProbabilities) in rollCalculator.DiceCountToScoresToProbabilities)
            {
                foreach ((int score, double probability) in scoreToProbabilities)
                {
                    if (score == 0)
                    {
                        GameState newGs = gs.Fail();

                        // Goes to opponent.
                        TotalScore += (1 - ProbabilityOfWinningFromGs(newGs, stackCounterToReturnKnownValue - 1, rollsThisTurn: 0)) * probability;
                    }
                    else if (diceUsed == gs.DiceToRoll)
                    {
                        GameState newGs = gs.RollOver(score);

                        TotalScore += ProbabilityOfWinningFromGs(newGs, stackCounterToReturnKnownValue - 1, rollsThisTurn + 1) * probability;
                    }
                    else
                    {
                        GameState newGs = gs.AddRolledScore(score, diceUsed);

                        TotalScore += ProbabilityOfWinningFromGs(newGs, stackCounterToReturnKnownValue - 1, rollsThisTurn + 1) * probability;
                    }
                }
            }

            return TotalScore;
        }

        // If the value doesnt exist in the dictionary, mock it at 0.5 to avoid infinite loops
        private double GetGameStateProbability(GameState gs)
        {
            if (gameStateProbabilities.TryGetValue(gs, out double probability))
            {
                return probability;
            }
            return 0.5;
        }

        private bool ShouldRoll(GameState gs, out double probability)
        {
            if (gameStateProbabilitiesRisky.TryGetValue(gs, out double probabilityRisky))
            {
                if (gameStateProbabilitiesSafe.TryGetValue(gs, out double probabilitySafe))
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
                $" | R {gameStateProbabilitiesRisky.FirstOrDefault(kvp => kvp.Key.Equals(gs)).Value,6:#.####} | S {gameStateProbabilitiesSafe.FirstOrDefault(kvp => kvp.Key.Equals(gs)).Value,6:#.####}";
        }
    }
}
