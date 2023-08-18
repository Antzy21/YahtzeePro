namespace YahtzeePro
{
    public class RollPosibilities
    {
        private readonly int _maxDiceCount;

        private record DiceCombination(int NumberOfOnes, int NumberOfFives, int NumberOfOthers);

        public record Score(int score);
        public record ValuableDiceCount(int valueAddingDice);

        private Dictionary<DiceCombination, double> _diceComboToProbabilities = new();

        private readonly Dictionary<ValuableDiceCount, Dictionary<Score, double>> _diceCountToScoresToProbabilities = new();

        public Dictionary<ValuableDiceCount, Dictionary<Score, double>> ProbabilitiesOfScores => _diceCountToScoresToProbabilities;

        private ValuableDiceCount GetNumberOfValuableDice(DiceCombination diceCombination)
        {
            return new(diceCombination.NumberOfFives + diceCombination.NumberOfOnes);
        }

        public RollPosibilities(int maxDiceCount)
        {
            _maxDiceCount = maxDiceCount;

            GenerateValues(_maxDiceCount, new DiceCombination(0, 0, 0));

            foreach (var (diceCombo, probability) in _diceComboToProbabilities)
            {
                var valueableDiceCount = GetNumberOfValuableDice(diceCombo);
                if (!_diceCountToScoresToProbabilities.ContainsKey(valueableDiceCount))
                {
                    _diceCountToScoresToProbabilities[valueableDiceCount] = new();
                }
                _diceCountToScoresToProbabilities[valueableDiceCount].Add(ScoreDiceCombination(diceCombo), probability);
            }
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

        private Score ScoreDiceCombination(DiceCombination diceCombination)
        {
            return new(
                diceCombination.NumberOfOnes * 100 +
                diceCombination.NumberOfFives * 50);
        }
    }
}
