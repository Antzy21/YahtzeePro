namespace YahtzeePro
{
    public class RollPosibilities
    {
        private readonly int _maxDiceCount;

        private record DiceCombination(int NumberOfOnes, int NumberOfFives, int NumberOfOthers);

        public record ScoreAndValuableDiceCount(int score, int valueAddingDice);

        private Dictionary<DiceCombination, double> _diceComboToProbabilities = new();

        private readonly Dictionary<ScoreAndValuableDiceCount, double> _scoresToProbabilities = new();

        public Dictionary<ScoreAndValuableDiceCount, double> ProbabilitiesOfScores => _scoresToProbabilities;

        public RollPosibilities(int maxDiceCount)
        {
            _maxDiceCount = maxDiceCount;

            GenerateValues(_maxDiceCount, new DiceCombination(0, 0, 0));

            _scoresToProbabilities = _diceComboToProbabilities.ToDictionary(
                    kvp => new ScoreAndValuableDiceCount(
                        ScoreDiceCombination(kvp.Key),
                        kvp.Key.NumberOfOnes + kvp.Key.NumberOfFives
                    ),
                    kvp => kvp.Value
                );
        }

        private void GenerateValues(int diceCount, DiceCombination currentCombination, double probability = 1)
        {
            if (diceCount == 0)
            {
                _diceComboToProbabilities.Add(currentCombination, probability);
            }

            // Add a 1
            var comboWithExtraOne = new DiceCombination(
                currentCombination.NumberOfOnes + 1,
                currentCombination.NumberOfFives,
                currentCombination.NumberOfOthers);
            GenerateValues(diceCount - 1, comboWithExtraOne, probability / 6);

            // Add a 5
            var comboWithExtraFive = new DiceCombination(
                currentCombination.NumberOfOnes,
                currentCombination.NumberOfFives + 1,
                currentCombination.NumberOfOthers);
            GenerateValues(diceCount - 1, comboWithExtraFive, probability / 6);

            // Add 2,3,4 or 6
            var comboWithExtraOther = new DiceCombination(
                currentCombination.NumberOfOnes,
                currentCombination.NumberOfFives + 1,
                currentCombination.NumberOfOthers);
            GenerateValues(diceCount - 1, comboWithExtraOther, probability * 4 / 6);
        }

        private int ScoreDiceCombination(DiceCombination diceCombination)
        {
            return diceCombination.NumberOfOnes * 100 +
                diceCombination.NumberOfFives * 50;
        }
    }
}
