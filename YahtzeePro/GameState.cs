namespace YahtzeePro
{
    public struct GameState
    {
        public GameState(
            int playerScore,
            int opponentScore,
            int cachedScore,
            int diceToRoll,
            bool isStartOfTurn)
        {
            PlayerScore = playerScore;
            OpponentScore = opponentScore;
            CachedScore = cachedScore;
            IsStartOfTurn = isStartOfTurn;
            DiceToRoll = diceToRoll;
            TotalDice = diceToRoll;
        }

        // Allows private setting of total dice
        private GameState(
            int playerScore,
            int opponentScore,
            int cachedScore,
            int diceToRoll,
            bool isStartOfTurn,
            int totalDice)
        {
            PlayerScore = playerScore;
            OpponentScore = opponentScore;
            CachedScore = cachedScore;
            IsStartOfTurn = isStartOfTurn;
            DiceToRoll = diceToRoll;
            TotalDice = totalDice;
        }

        public readonly int PlayerScore { get; init; }
        public readonly int OpponentScore { get; init; }
        public readonly int CachedScore { get; init; }
        public readonly int DiceToRoll { get; init; }
        public readonly int TotalDice { get; init; }
        public readonly bool IsStartOfTurn { get; init; }

        /// <summary>
        /// Reset the cached score and make it no longer start of turn.
        /// </summary>
        /// <returns></returns>
        public readonly GameState ResetCache()
        {
            return new GameState(
                playerScore: OpponentScore,
                opponentScore: PlayerScore,
                cachedScore: 0,
                isStartOfTurn: true,
                diceToRoll: TotalDice,
                totalDice: TotalDice
            );
        }

        /// <summary>
        /// Add the cached score to the player's score, then switch to opponent.
        /// Keep the cached score and the dice to roll.
        /// </summary>
        /// <returns></returns>
        public readonly GameState Bank()
        {
            return new GameState(
                playerScore: OpponentScore,
                opponentScore: PlayerScore + CachedScore,
                cachedScore: 0,
                isStartOfTurn: true,
                diceToRoll: TotalDice,
                totalDice: TotalDice
            );
        }

        /// <summary>
        /// Switch to opponent, reset the cached score and the dice to roll.
        /// </summary>
        /// <returns></returns>
        public readonly GameState Fail()
        {
            return new GameState(
                playerScore: OpponentScore,
                opponentScore: PlayerScore,
                cachedScore: 0,
                isStartOfTurn: true,
                diceToRoll: TotalDice,
                totalDice: TotalDice
            );
        }

        /// <summary>
        /// Add to the cache and reset the dice to roll.
        /// </summary>
        /// <param name="rolledScore"></param>
        /// <returns></returns>
        public readonly GameState RollOver(int rolledScore)
        {
            return new GameState(
                playerScore: PlayerScore,
                opponentScore: OpponentScore,
                cachedScore: CachedScore + rolledScore,
                isStartOfTurn: false,
                diceToRoll: TotalDice,
                totalDice: TotalDice
            );
        }

        /// <summary>
        /// Add to the cache and reduce the dice to roll.
        /// </summary>
        /// <param name="rolledScore"></param>
        /// <param name="usedDice"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public readonly GameState AddRolledScore(int rolledScore, int usedDice)
        {
            if (usedDice >= DiceToRoll)
            {
                throw new ArgumentOutOfRangeException(
                    $"Only had {DiceToRoll} dice to roll, but told {usedDice} have been used"
                );
            }

            return new GameState(
                playerScore: PlayerScore,
                opponentScore: OpponentScore,
                cachedScore: CachedScore + rolledScore,
                isStartOfTurn: false,
                diceToRoll: DiceToRoll - usedDice,
                totalDice: TotalDice
            );
        }

        public override readonly string ToString()
        {
            return $"P:{PlayerScore} | O:{OpponentScore} | C:{CachedScore} | D:{DiceToRoll} | S:{IsStartOfTurn}";
        }
    }
}
