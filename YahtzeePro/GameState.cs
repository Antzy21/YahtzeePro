namespace YahtzeePro
{
    public readonly record struct GameState(int PlayerScore, int OpponentScore, int CachedScore, int DiceToRoll, bool IsStartOfTurn, int TotalDice)
    {
        /// <summary>
        /// Reset the cached score and make it no longer start of turn.
        /// </summary>
        /// <returns></returns>
        public readonly GameState ResetCache()
        {
            return new GameState(
                PlayerScore: OpponentScore,
                OpponentScore: PlayerScore,
                CachedScore: 0,
                IsStartOfTurn: true,
                DiceToRoll: TotalDice,
                TotalDice: TotalDice
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
                PlayerScore: OpponentScore,
                OpponentScore: PlayerScore + CachedScore,
                CachedScore: 0,
                IsStartOfTurn: true,
                DiceToRoll: TotalDice,
                TotalDice: TotalDice
            );
        }

        /// <summary>
        /// Switch to opponent, reset the cached score and the dice to roll.
        /// </summary>
        /// <returns></returns>
        public readonly GameState Fail()
        {
            return new GameState(
                PlayerScore: OpponentScore,
                OpponentScore: PlayerScore,
                CachedScore: 0,
                IsStartOfTurn: true,
                DiceToRoll: TotalDice,
                TotalDice: TotalDice
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
                PlayerScore: PlayerScore,
                OpponentScore: OpponentScore,
                CachedScore: CachedScore + rolledScore,
                IsStartOfTurn: false,
                DiceToRoll: TotalDice,
                TotalDice: TotalDice
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
                PlayerScore: PlayerScore,
                OpponentScore: OpponentScore,
                CachedScore: CachedScore + rolledScore,
                IsStartOfTurn: false,
                DiceToRoll: DiceToRoll - usedDice,
                TotalDice: TotalDice
            );
        }

        public override readonly string ToString()
        {
            return $"P:{PlayerScore} | O:{OpponentScore} | C:{CachedScore} | D:{DiceToRoll} | S:{IsStartOfTurn}";
        }
    }
}
