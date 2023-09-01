using System.Text;

namespace YahtzeePro
{
    public readonly record struct DiceCombination
    {
        private readonly int _onesCount;
        private readonly int _twosCount;
        private readonly int _threesCount;
        private readonly int _foursCount;
        private readonly int _fivesCount;
        private readonly int _sixesCount;

        public int Score { get; init; }
        public int NumberOfScoringDice { get; init; }
        public bool AllDiceAreScoring { get; init; }

        public DiceCombination(params int[] dice)
        {
            foreach (var die in dice)
            {
                switch (die)
                {
                    case 1:
                        _onesCount++;
                        break;
                    case 2:
                        _twosCount++;
                        break;
                    case 3:
                        _threesCount++;
                        break;
                    case 4:
                        _foursCount++;
                        break;
                    case 5:
                        _fivesCount++;
                        break;
                    case 6:
                        _sixesCount++;
                        break;
                    default:
                        throw new ArgumentException($"Unable to accept dice with value {die}");
                }
            }
            Score = CalculateScore();
            NumberOfScoringDice = CalculateNumberOfScoringDice();
            AllDiceAreScoring = NumberOfScoringDice == _onesCount + _twosCount + _threesCount + _foursCount + _fivesCount + _sixesCount;
        }

        public DiceCombination Generate(int numberOfDice, Random random)
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

        private int CalculateScore()
        {
            int score = 0;
            if (_onesCount >= 3)
                score += (int)Math.Pow(10, _onesCount);
            else
                score += 100 * _onesCount;
            if (_twosCount >= 3)
                score += 2 * (int)Math.Pow(10, _twosCount);
            if (_threesCount >= 3)
                score += 3 * (int)Math.Pow(10, _threesCount);
            if (_foursCount >= 3)
                score += 4 * (int)Math.Pow(10, _foursCount);
            if (_fivesCount >= 3)
                score += 5 * (int)Math.Pow(10, _fivesCount);
            else
                score += 50 * _fivesCount;
            if (_sixesCount >= 3)
                score += 6 * (int)Math.Pow(10, _sixesCount);
            return score;
        }

        private int CalculateNumberOfScoringDice()
        {
            int scoringDiceCount = _onesCount + _fivesCount;
            if (_twosCount >= 3)
                scoringDiceCount += _twosCount;
            if (_threesCount >= 3)
                scoringDiceCount += _threesCount;
            if (_foursCount >= 3)
                scoringDiceCount += _foursCount;
            if (_sixesCount >= 3)
                scoringDiceCount += _sixesCount;
            return scoringDiceCount;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i <= _onesCount; i++)
                sb.Append("1,");
            for (int i = 0; i <= _twosCount; i++)
                sb.Append("2,");
            for (int i = 0; i <= _threesCount; i++)
                sb.Append("3,");
            for (int i = 0; i <= _foursCount; i++)
                sb.Append("4,");
            for (int i = 0; i <= _fivesCount; i++)
                sb.Append("5,");
            for (int i = 0; i <= _sixesCount; i++)
                sb.Append("6,");

            return sb.ToString();
        }
    }
}
