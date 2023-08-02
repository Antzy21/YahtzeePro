namespace YahtzeePro
{
    public class ProbabilitiesCalculator
    {
        private readonly RollPosibilities _rollPossibilitiesCalculator;

        public ProbabilitiesCalculator()
        {
            _rollPossibilitiesCalculator = new RollPosibilities(5);
        }

        private static int winningValue = 5000;

        private static int totalDice = 5;

        public Dictionary<GameState, double> gameStateProbabilities = new() { };

        public void PopulateGameStateProbabilities()
        {
            for (int playerScore = winningValue - 50; playerScore >= 0; playerScore -= 50)
            {
                for (int opponentScore = winningValue - 50; opponentScore >= 0; opponentScore -= 50)
                {
                    for (int cachedScore = winningValue - playerScore; cachedScore >= 0; cachedScore -= 50)
                    {
                        for (int diceCount = 1; diceCount <= totalDice; diceCount++)
                        {
                            var gs = new GameState(playerScore, opponentScore, cachedScore, diceCount);
                            gameStateProbabilities.Add(gs, ProbabilityOfWinningFromGs(gs));
                        }
                    }
                }
            }
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

            return Math.Max(resetDiceProability, continueDiceProability);
        }

        private double ProbabilityOfWinningFromGs(GameState gs)
        {
            // Check to see if value has previously been calculated
            if (gameStateProbabilities.TryGetValue(gs, out var result))
            {
                return result;
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

            // Other cases
            var bankScoreProbability = ProbabilityOfWinningIfBanking(gs);

            var rollScoreProbability = ProbabilityOfWinningIfRolling(gs);

            return Math.Max(bankScoreProbability, rollScoreProbability);
        }

        private double ProbabilityOfWinningIfRolling(GameState gs)
        {
            foreach (var (extraScore, diceUsed, probability) in PossibleScoring(gs.DiceToRoll))
            {

            }
        }

    }
}
