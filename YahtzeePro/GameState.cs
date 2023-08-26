namespace YahtzeePro
{
    public class GameState
    {
        private readonly int _totalDice;

        private int _playerScore;
        private int _opponentScore;
        private int _cachedScore;
        private int _diceToRoll;
        private bool _isStartOfTurn;

        public GameState(
            int playerScore,
            int opponentScore,
            int cachedScore,
            int diceToRoll,
            bool isStartOfTurn)
        {
            _playerScore = playerScore;
            _opponentScore = opponentScore;
            _cachedScore = cachedScore;
            _isStartOfTurn = isStartOfTurn;
            _diceToRoll = diceToRoll;
            _totalDice = diceToRoll;
        }

        public int PlayerScore => _playerScore;
        public int OpponentScore => _opponentScore;
        public int CachedScore => _cachedScore;
        public int DiceToRoll => _diceToRoll;
        public bool IsStartOfTurn => _isStartOfTurn;

        /// <summary>
        /// Reset the cached score and make it no longer start of turn.
        /// </summary>
        /// <returns></returns>
        public GameState ResetCache()
        {
            _cachedScore = 0;
            _isStartOfTurn = false;
            return this;
        }

        /// <summary>
        /// Add the cached score to the player's score, then switch to opponent.
        /// Keep the cached score and the dice to roll.
        /// </summary>
        /// <returns></returns>
        public GameState Bank()
        {
            (_opponentScore, _playerScore) = (_playerScore + _cachedScore, _opponentScore);
            _isStartOfTurn = true;
            return this;
        }

        /// <summary>
        /// Switch to opponent, reset the cached score and the dice to roll.
        /// </summary>
        /// <returns></returns>
        public GameState Fail()
        {
            (_opponentScore, _playerScore) = (_playerScore, _opponentScore);
            _isStartOfTurn = true;
            _diceToRoll = _totalDice;
            _cachedScore = 0;
            return this;
        }

        /// <summary>
        /// Add to the cache and reset the dice to roll.
        /// </summary>
        /// <param name="rolledScore"></param>
        /// <returns></returns>
        public GameState RollOver(int rolledScore)
        {
            _isStartOfTurn = false;
            _diceToRoll = _totalDice;
            _cachedScore += rolledScore;
            return this;
        }

        /// <summary>
        /// Add to the cache and reduce the dice to roll.
        /// </summary>
        /// <param name="rolledScore"></param>
        /// <param name="usedDice"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public GameState AddRolledScore(int rolledScore, int usedDice)
        {
            if (usedDice >= _diceToRoll)
                throw new ArgumentOutOfRangeException(
                    $"Only had {_diceToRoll} dice to roll, but told {usedDice} have been used"
                );

            _isStartOfTurn = false;
            _diceToRoll -= usedDice;
            _cachedScore += rolledScore;
            return this;
        }
    }
}
