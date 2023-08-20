namespace YahtzeePro
{
    public class ProbabilitiesCalculator
    {
        // For different quantities of dice to roll, there will be different scores and probabilities.
        // The RollProbabilities class generates the scores and probabilities for each.
        private readonly Dictionary<int, RollPosibilities> _rollPosibilitiesDictionary = new();

        private readonly int _winningValue;

        private readonly int _totalDice;

        // To avoid infinite loops, once this counter reaches zero on the stack, return the known existing value.
        private readonly int _initialStackCounterToReturnKnownValue;

        private readonly int _calculationIterations;

        public ProbabilitiesCalculator(
            int winningValue,
            int totalDice,
            int initialStackCounterToReturnKnownValue = 3,
            int calculationIterations = 10)
        {
            _winningValue = winningValue;
            _totalDice = totalDice;
            _initialStackCounterToReturnKnownValue = initialStackCounterToReturnKnownValue;
            _calculationIterations = calculationIterations;

            Console.WriteLine("New Probabilities Calculator created.");
            Console.WriteLine($"Win Value: {_winningValue}");
            Console.WriteLine($"Total Dice: {_totalDice}");

            for (int i = 1; i <= _totalDice; i++)
            {
                _rollPosibilitiesDictionary.Add(i, new RollPosibilities(i));
            }
            _calculationIterations = calculationIterations;
        }

        public Dictionary<GameState, double> gameStateProbabilities = new() { };

        private HashSet<GameState> gameStateThatHaveBeenCalculated = new() { };

        // If the value doesnt exist in the dictionary, mock it at 0.5 to avoid infinite loops
        private double GetGameStateProbability(GameState gs)
        {
            if (gameStateProbabilities.TryGetValue(gs, out var probability))
            {
                return probability;
            }
            return 0.5;
        }

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
                            var gs = new GameState(playerScore, opponentScore, cachedScore, diceCount);

                            // Make more accurate estimate
                            // Iterate a few times to improve the estimate.
                            for (int i = 0; i < _calculationIterations; i++)
                            {
                                gameStateProbabilities[gs] = ProbabilityOfWinningFromGs(gs, _initialStackCounterToReturnKnownValue, 0);
                            }

                            gameStateThatHaveBeenCalculated.Add(gs);

                            if (timer.Elapsed > NextLoggingTime)
                            {
                                NextLoggingTime = timer.Elapsed + LoggingInterval;
                                Console.WriteLine($" ({diceCount}Ds) {playerScore,4} + {cachedScore,4} - {opponentScore,4} => Prob: {gameStateProbabilities[gs].ToString("#.##")}");
                            }
                        }
                    }
                }
            }

            Console.WriteLine("\nFinished populating\n");
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

        private double ProbabilityOfWinningFromGs(GameState gs, int stackCounterToReturnKnownValue, int rollsThisTurn = 0)
        {
            // stackCounterToReturnKnownValue makes sure that the existing preset value (which is needed to avoid stack overflow),
            // is not immediately returned. Once it is zero, it will return.
            if (stackCounterToReturnKnownValue == 0 || gameStateThatHaveBeenCalculated.Contains(gs))
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
                return rollScoreProbability;
            }

            /// #########################################################
            ///
            var bankScoreProbability = ProbabilityOfWinningIfBanking(gs, stackCounterToReturnKnownValue);
            /// 
            /// #########################################################

            return Math.Max(bankScoreProbability, rollScoreProbability);
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
    }
}
