using System.Text;
using Probability = System.Double;
using Score = System.Int32;
using ValuableDiceCount = System.Int32;

namespace YahtzeePro
{
    public class RollPosibilities
    {
        private readonly int _maxDiceCount;

        private readonly Dictionary<DiceCombination, Probability> _diceComboToProbabilities = new();

        private readonly Dictionary<ValuableDiceCount, Dictionary<Score, Probability>> _diceCountToScoresToProbabilities = new();

        public Dictionary<ValuableDiceCount, Dictionary<Score, Probability>> ProbabilitiesOfScores => _diceCountToScoresToProbabilities;

        public RollPosibilities(int maxDiceCount)
        {
            _maxDiceCount = maxDiceCount;

            GenerateValues(_maxDiceCount, new DiceCombination(0, 0, 0));

            foreach ((DiceCombination diceCombo, double probability) in _diceComboToProbabilities)
            {
                ValuableDiceCount valueableDiceCount = GetNumberOfValuableDice(diceCombo);
                if (!_diceCountToScoresToProbabilities.ContainsKey(valueableDiceCount))
                {
                    _diceCountToScoresToProbabilities[valueableDiceCount] = new();
                }
                _diceCountToScoresToProbabilities[valueableDiceCount].Add(diceCombo.Score, probability);
            }
        }

        private static ValuableDiceCount GetNumberOfValuableDice(DiceCombination diceCombination)
        {
            return diceCombination.NumberOfFives + diceCombination.NumberOfOnes;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach ((DiceCombination diceCombo, double probability) in _diceComboToProbabilities)
            {
                stringBuilder.AppendLine($"{diceCombo} - {diceCombo.Score} = {probability.ToString("#.###")}");
            }

            return stringBuilder.ToString();
        }

        private void GenerateValues(int diceCount, DiceCombination currentCombination, double probability = 1)
        {
            if (diceCount == 0)
            {
                _diceComboToProbabilities[currentCombination] = probability;
                return;
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
                currentCombination.NumberOfFives,
                currentCombination.NumberOfOthers + 1);
            GenerateValues(diceCount - 1, comboWithExtraOther, probability * 4 / 6);
        }
    }
}
