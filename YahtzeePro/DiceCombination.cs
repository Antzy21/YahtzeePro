namespace YahtzeePro
{
    public record DiceCombination(int NumberOfOnes, int NumberOfFives, int NumberOfOthers)
    {
        public static DiceCombination Generate(int numberOfDice, Random random)
        {
            if (numberOfDice < 0)
            {
                throw new ArgumentOutOfRangeException($"number of dice give {numberOfDice} is negative.");
            }

            int ones = 0;
            int fives = 0;
            int others = 0;

            for (int i = 0; i < numberOfDice; i++)
            {
                int diceValue = random.Next(5) + 1;
                if (diceValue == 1)
                {
                    ones++;
                }
                else if (diceValue == 5)
                {
                    fives++;
                }
                else
                {
                    others++;
                }
            }
            return new DiceCombination(ones, fives, others);
        }

        public int Score => NumberOfOnes * 100 + NumberOfFives * 50;

        public int ScoringDice => NumberOfOnes + NumberOfFives;

        public bool UsesAllDice => NumberOfOthers == 0;

        public override string ToString()
        {
            return $"1s:{NumberOfOnes} | 5s:{NumberOfFives} | 2-6s:{NumberOfOthers}";
        }
    }
}
