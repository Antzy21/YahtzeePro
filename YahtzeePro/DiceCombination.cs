using static YahtzeePro.RollPosibilities;

namespace YahtzeePro
{
    public record DiceCombination(int NumberOfOnes, int NumberOfFives, int NumberOfOthers)
    {
        public static DiceCombination Generate(int numberOfDice, Random random)
        {
            var ones = 0;
            var fives = 0;
            var others = 0;

            for (int i = 0; i < numberOfDice; i++)
            {
                var diceValue = random.Next(5) + 1;
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
    }
}
