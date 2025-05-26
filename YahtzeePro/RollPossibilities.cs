using System.Text;
using Probability = System.Double;
using Score = System.Int32;
using ValuableDiceCount = System.Int32;

namespace YahtzeePro.Core
{
    public class RollPossibilities
    {
        private readonly int _maxDiceCount;

        private readonly Dictionary<DiceCombination, Probability> _diceComboToProbabilities = new();

        private readonly Dictionary<ValuableDiceCount, Dictionary<Score, Probability>> _diceCountToScoresToProbabilities = new();
        public Dictionary<ValuableDiceCount, Dictionary<Score, Probability>> DiceCountToScoresToProbabilities => _diceCountToScoresToProbabilities;

        public RollPossibilities(int maxDiceCount)
        {
            _maxDiceCount = maxDiceCount;

            GenerateValues(_maxDiceCount, new DiceCombination(Array.Empty<int>()));

            foreach ((DiceCombination diceCombo, double probability) in _diceComboToProbabilities)
            {
                if (!_diceCountToScoresToProbabilities.ContainsKey(diceCombo.NumberOfScoringDice))
                {
                    // No Entry exists for number of scoring dice, so add one to be populated with scores and probabilities.
                    _diceCountToScoresToProbabilities[diceCombo.NumberOfScoringDice] = new();
                }
                if (!_diceCountToScoresToProbabilities[diceCombo.NumberOfScoringDice].ContainsKey(diceCombo.Score))
                {
                    // Entry exists for number of scoring dice, but this score hasn't been seen yet. Add it and a default probability of zero.
                    _diceCountToScoresToProbabilities[diceCombo.NumberOfScoringDice].Add(diceCombo.Score, 0);
                }
                // Entry exists for number of scoring dice and the score, so add the probability for this other dice combo.
                _diceCountToScoresToProbabilities[diceCombo.NumberOfScoringDice][diceCombo.Score] += probability;
            }
        }

        private void GenerateValues(int diceCount, DiceCombination diceCombo, double probability = 1)
        {
            if (diceCount == 0)
            {
                _diceComboToProbabilities[diceCombo] = probability;
                return;
            }

            for (int i = 1; i <= 6; i++)
            {
                var comboWithExtraOne = diceCombo.AddDie(dieValue: i);
                GenerateValues(diceCount - 1, comboWithExtraOne, probability / 6);
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach ((DiceCombination diceCombo, double probability) in _diceComboToProbabilities)
            {
                stringBuilder.AppendLine($"({diceCombo}) => Score:{diceCombo.Score} = Prob:{probability.ToString("#.###")}");
            }

            return stringBuilder.ToString();
        }
    }
}
