﻿namespace YahtzeePro
{
    public class ProbabilitiesCalculator
    {
        // For different quantities of dice to roll, there will be different scores and probabilities.
        // The RollProbabilities class generates the scores and probabilities for each.
        private readonly Dictionary<int, RollPosibilities> _rollPosibilitiesDictionary = new();

        public ProbabilitiesCalculator()
        {
            for (int i = 1; i <= totalDice; i++)
            {
                _rollPosibilitiesDictionary.Add(i, new RollPosibilities(i));
            }
        }

        private static int winningValue = 5000;

        private static int totalDice = 5;

        public Dictionary<GameState, double> gameStateProbabilities = new() { };

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
            var LoggingInterval = new TimeSpan(1000000);
            var NextLoggingTime = timer.Elapsed;
            Console.WriteLine("\nBegin populating...\n");

            for (int playerScore = winningValue - 50; playerScore >= 0; playerScore -= 50)
            {
                for (int opponentScore = winningValue - 50; opponentScore >= 0; opponentScore -= 50)
                {
                    for (int cachedScore = winningValue - playerScore; cachedScore >= 0; cachedScore -= 50)
                    {
                        for (int diceCount = totalDice; diceCount > 0; diceCount--)
                        {
                            // New gs to test
                            var gs = new GameState(playerScore, opponentScore, cachedScore, diceCount);

                            // Make more accurate estimate
                            // Iterate a few times to improve the estimate.
                            for (int i = 0; i < 20; i++)
                            {
                                gameStateProbabilities[gs] = ProbabilityOfWinningFromGs(gs, skipEarlyReturn: true);
                            }

                            if (timer.Elapsed > NextLoggingTime)
                            {
                                NextLoggingTime = timer.Elapsed + LoggingInterval;
                                Console.WriteLine($"Score: {playerScore,4}+{cachedScore,4} - {opponentScore,4} => Prob: {gameStateProbabilities[gs].ToString("#.##")}");
                            }
                        }
                    }
                }
            }

            Console.WriteLine("\nFinished populating\n");
        }

        private double ProbabilityOfWinningIfBanking(GameState gs)
        {
            var resetDiceGs = new GameState(
                PlayerScore: gs.OpponentScore,
                OpponentScore: gs.PlayerScore + gs.CachedScore,
                CachedScore: 0,
                DiceToRoll: totalDice
                );

            var resetDiceProability = ProbabilityOfWinningFromGs(resetDiceGs);

            var continueDiceGs = new GameState(
                PlayerScore: gs.OpponentScore,
                OpponentScore: gs.PlayerScore + gs.CachedScore,
                CachedScore: gs.CachedScore,
                DiceToRoll: gs.DiceToRoll
                );

            var continueDiceProability = ProbabilityOfWinningFromGs(continueDiceGs);

            return 1 - Math.Max(resetDiceProability, continueDiceProability);
        }

        // Skip early return makes sure that the existing preset value (which is needed to avoid stack overflow),
        // is not immediately returned.
        private double ProbabilityOfWinningFromGs(GameState gs, int rollsThisTurn = 0, bool skipEarlyReturn = false)
        {
            // Check to see if value has previously been calculated
            if (!skipEarlyReturn)
            {
                return GetGameStateProbability(gs);
            }

            // Default cases
            if (gs.PlayerScore + gs.CachedScore >= winningValue)
            {
                if (gs.OpponentScore >= winningValue)
                {
                    throw new Exception("Both players are in a winning state.");
                }
                return 1;
            }
            else if (gs.OpponentScore >= winningValue)
            {
                return 0;
            }

            /// #########################################################
            /// 
            var rollScoreProbability = ProbabilityOfWinningIfRolling(gs, rollsThisTurn);
            ///
            /// #########################################################

            // Can't bank if they haven't rolled yet. 
            if (rollsThisTurn == 0)
            {
                return rollScoreProbability;
            }

            /// #########################################################
            ///
            var bankScoreProbability = ProbabilityOfWinningIfBanking(gs);
            /// 
            /// #########################################################

            return Math.Max(bankScoreProbability, rollScoreProbability);
        }

        private double ProbabilityOfWinningIfRolling(GameState gs, int rollsThisTurn)
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
                            DiceToRoll: totalDice
                        );

                        // Goes to opponent.
                        TotalScore += 1 - ProbabilityOfWinningFromGs(newGs, 0) * probability;
                    }
                    else if (diceUsed.valueAddingDice == gs.DiceToRoll)
                    {
                        // Roll over!!!!
                        var newGs = new GameState(
                            PlayerScore: gs.PlayerScore,
                            OpponentScore: gs.OpponentScore,
                            CachedScore: gs.CachedScore + score.score,
                            DiceToRoll: totalDice
                        );

                        TotalScore += ProbabilityOfWinningFromGs(newGs, rollsThisTurn + 1) * probability;
                    }
                    else
                    {
                        var newGs = new GameState(
                            PlayerScore: gs.PlayerScore,
                            OpponentScore: gs.OpponentScore,
                            CachedScore: gs.CachedScore + score.score,
                            DiceToRoll: totalDice - diceUsed.valueAddingDice
                        );

                        TotalScore += ProbabilityOfWinningFromGs(newGs, rollsThisTurn + 1) * probability;
                    }
                }
            }

            return TotalScore;
        }
    }
}
