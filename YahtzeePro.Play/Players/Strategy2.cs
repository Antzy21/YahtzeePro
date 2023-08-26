namespace YahtzeePro.Play.Players
{
    internal class Strategy2 : SimpleStrategyBase, IPlayer
    {
        public Strategy2()
        {
            WhenToBankWith1Dice = 500;
            WhenToBankWith2Dice = 800;
        }
    }
}
