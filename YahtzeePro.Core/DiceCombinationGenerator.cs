using YahtzeePro.Core.Models;

namespace YahtzeePro.Core
{
    public static class DiceCombinationGenerator
    {
        public static DiceCombination FromDieList(IEnumerable<int> dice)
        {
            Dictionary<int, int> diceCountByValue = GroupDieByValue(dice);

            var score = CalculateScore(diceCountByValue);
            var numberOfScoringDice = CalculateNumberOfScoringDice(diceCountByValue);
            var allDiceAreScoring = numberOfScoringDice == diceCountByValue[1] + diceCountByValue[2] + diceCountByValue[3] + diceCountByValue[4] + diceCountByValue[5] + diceCountByValue[6];

            return new DiceCombination(diceCountByValue, score, numberOfScoringDice, allDiceAreScoring);
        }

        public static DiceCombination Generate(int numberOfDice, Random random)
        {
            if (numberOfDice < 0)
                throw new ArgumentOutOfRangeException($"number of dice give {numberOfDice} is negative.");

            var dice = new List<int>();
            for (int i = 0; i < numberOfDice; i++)
            {
                int diceValue = random.Next(5) + 1;
                dice.Add(diceValue);
            }

            return FromDieList(dice);
        }

        public static DiceCombination Empty()
        {
            return Generate(0, new Random());
        }

        private static int CalculateScore(Dictionary<int, int> diceCount)
        {
            int score = 0;

            if (diceCount[1] <= 3)
                score += 100 * diceCount[1];

            if (diceCount[5] <= 2)
                score += 50 * diceCount[5];

            if (diceCount[1] >= 3)
                score += 1 * (int)Math.Pow(10, diceCount[1] - 1);
            if (diceCount[2] >= 3)
                score += 2 * (int)Math.Pow(10, diceCount[2] - 1);
            if (diceCount[3] >= 3)
                score += 3 * (int)Math.Pow(10, diceCount[3] - 1);
            if (diceCount[4] >= 3)
                score += 4 * (int)Math.Pow(10, diceCount[4] - 1);
            if (diceCount[5] >= 3)
                score += 5 * (int)Math.Pow(10, diceCount[5] - 1);
            if (diceCount[6] >= 3)
                score += 6 * (int)Math.Pow(10, diceCount[6] - 1);
            return score;
        }

        private static int CalculateNumberOfScoringDice(Dictionary<int, int> diceCount)
        {
            int scoringDiceCount = diceCount[1] + diceCount[5];
            if (diceCount[2] >= 3)
                scoringDiceCount += diceCount[2];
            if (diceCount[3] >= 3)
                scoringDiceCount += diceCount[3];
            if (diceCount[4] >= 3)
                scoringDiceCount += diceCount[4];
            if (diceCount[6] >= 3)
                scoringDiceCount += diceCount[6];
            return scoringDiceCount;
        }

        private static Dictionary<int, int> GroupDieByValue(IEnumerable<int> dice)
        {
            Dictionary<int, int> diceCount = new()
            {
                { 1, 0 },
                { 2, 0 },
                { 3, 0 },
                { 4, 0 },
                { 5, 0 },
                { 6, 0 }
            };
            foreach (int die in dice)
            {
                if (die > 6)
                    throw new ArgumentException($"Unable to accept dice with value {die}");

                diceCount[die]++;
            }

            return diceCount;
        }
    }
}
