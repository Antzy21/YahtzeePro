namespace YahtzeePro.Play.Players
{
    internal abstract class SimpleStrategyBase : IPlayer
    {
        protected int WhenToBankWith1Dice { get; set; }
        protected int WhenToBankWith2Dice { get; set; }
        protected int WhenToBankWith3Dice { get; set; } = int.MaxValue;
        protected int WhenToBankWith4Dice { get; set; } = int.MaxValue;

        public string Name => $"1->{WhenToBankWith1Dice}, 2->{WhenToBankWith2Dice}";

        public PlayChoice GetMove(GameState gs)
        {
            if (gs.CachedScore >= WhenToBankWith1Dice|| gs.DiceToRoll == 1)
            {
                return PlayChoice.Safe;
            }
            if (gs.CachedScore >= WhenToBankWith2Dice || gs.DiceToRoll == 2)
            {
                return PlayChoice.Safe;
            }
            if (gs.CachedScore >= WhenToBankWith3Dice || gs.DiceToRoll == 3)
            {
                return PlayChoice.Safe;
            }
            if (gs.CachedScore >= WhenToBankWith4Dice || gs.DiceToRoll == 4)
            {
                return PlayChoice.Safe;
            }
            return PlayChoice.Risky;
        }
    }
}