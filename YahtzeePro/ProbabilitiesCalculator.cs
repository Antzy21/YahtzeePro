namespace YahtzeePro
{
    public static class ProbabilitiesCalculator
    {
        public static double FailProbability(int diceCount)
        {
            return Math.Pow(4d / 6d, (double)diceCount);
        }
    }
}
