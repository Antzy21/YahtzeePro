using System.Text;

namespace YahtzeePro
{
    public readonly record struct DiceCombination
    {
        private readonly Dictionary<int, int> diceCount = new() {
            {1, 0},
            {2, 0},
            {3, 0},
            {4, 0},
            {5, 0},
            {6, 0},
        };

        private readonly int[] _dice;

        public int Score { get; init; }
        public int NumberOfScoringDice { get; init; }
        public bool AllDiceAreScoring { get; init; }

        public DiceCombination(params int[] dice)
        {
            _dice = dice;
            foreach (int die in dice)
            {
                switch (die)
                {
                    case 1:
                        diceCount[1]++;
                        break;
                    case 2:
                        diceCount[2]++;
                        break;
                    case 3:
                        diceCount[3]++;
                        break;
                    case 4:
                        diceCount[4]++;
                        break;
                    case 5:
                        diceCount[5]++;
                        break;
                    case 6:
                        diceCount[6]++;
                        break;
                    default:
                        throw new ArgumentException($"Unable to accept dice with value {die}");
                }
            }
            Score = CalculateScore();
            NumberOfScoringDice = CalculateNumberOfScoringDice();
            AllDiceAreScoring = NumberOfScoringDice == diceCount[1] + diceCount[2] + diceCount[3] + diceCount[4] + diceCount[5] + diceCount[6];
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
            return new DiceCombination(dice.ToArray());
        }

        public DiceCombination AddDie(int dieValue)
        {
            return new DiceCombination([.. _dice, dieValue]);
        }

        private int CalculateScore()
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

        private int CalculateNumberOfScoringDice()
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

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < diceCount[1]; i++)
                sb.Append("1,");
            for (int i = 0; i < diceCount[2]; i++)
                sb.Append("2,");
            for (int i = 0; i < diceCount[3]; i++)
                sb.Append("3,");
            for (int i = 0; i < diceCount[4]; i++)
                sb.Append("4,");
            for (int i = 0; i < diceCount[5]; i++)
                sb.Append("5,");
            for (int i = 0; i < diceCount[6]; i++)
                sb.Append("6,");

            return sb.ToString();
        }
    }
}
