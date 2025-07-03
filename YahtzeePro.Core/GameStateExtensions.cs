using YahtzeePro.Core.Models;

namespace YahtzeePro.Core
{
    public static class GameStateExtensions
    {
        /// <summary>
        /// Reset the cached score and make it no longer start of turn.
        /// </summary>
        /// <returns></returns>
        public static GameState ResetCache(this GameState gs)
        {
            return new GameState(
                PlayerScore: gs.OpponentScore,
                OpponentScore: gs.PlayerScore,
                CachedScore: 0,
                IsStartOfTurn: true,
                DiceToRoll: gs.GameConfiguration.TotalDice,
                GameConfiguration: gs.GameConfiguration
            );
        }

        /// <summary>
        /// Add the cached score to the player's score, then switch to opponent.
        /// Keep the cached score and the dice to roll.
        /// </summary>
        /// <returns></returns>
        public static GameState Bank(this GameState gs)
        {
            return new GameState(
                PlayerScore: gs.OpponentScore,
                OpponentScore: gs.PlayerScore + gs.CachedScore,
                CachedScore: 0,
                IsStartOfTurn: true,
                DiceToRoll: gs.GameConfiguration.TotalDice,
                GameConfiguration: gs.GameConfiguration
            );
        }

        /// <summary>
        /// Switch to opponent, reset the cached score and the dice to roll.
        /// </summary>
        /// <returns></returns>
        public static GameState Fail(this GameState gs)
        {
            return new GameState(
                PlayerScore: gs.OpponentScore,
                OpponentScore: gs.PlayerScore,
                CachedScore: 0,
                IsStartOfTurn: true,
                DiceToRoll: gs.GameConfiguration.TotalDice,
                GameConfiguration: gs.GameConfiguration
            );
        }

        /// <summary>
        /// Add to the cache and reset the dice to roll.
        /// </summary>
        /// <param name="rolledScore"></param>
        /// <returns></returns>
        public static GameState RollOver(this GameState gs, int rolledScore)
        {
            return new GameState(
                PlayerScore: gs.PlayerScore,
                OpponentScore: gs.OpponentScore,
                CachedScore: gs.CachedScore + rolledScore,
                IsStartOfTurn: false,
                DiceToRoll: gs.GameConfiguration.TotalDice,
                GameConfiguration: gs.GameConfiguration
            );
        }

        /// <summary>
        /// Add to the cache and reduce the dice to roll.
        /// </summary>
        /// <param name="rolledScore"></param>
        /// <param name="usedDice"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static GameState AddRolledScore(this GameState gs, int rolledScore, int usedDice)
        {
            if (usedDice >= gs.DiceToRoll)
            {
                throw new ArgumentOutOfRangeException(
                    $"Only had {gs.DiceToRoll} dice to roll, but told {usedDice} have been used"
                );
            }

            return new GameState(
                PlayerScore: gs.PlayerScore,
                OpponentScore: gs.OpponentScore,
                CachedScore: gs.CachedScore + rolledScore,
                IsStartOfTurn: false,
                DiceToRoll: gs.GameConfiguration.TotalDice - usedDice,
                GameConfiguration: gs.GameConfiguration
            );
        }
    }
}
