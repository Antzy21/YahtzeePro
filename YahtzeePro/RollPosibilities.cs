namespace YahtzeePro
{
    public class RollPosibilities
    {
        private readonly int _maxDiceCount;

        public RollPosibilities(int maxDiceCount)
        {
            _maxDiceCount = maxDiceCount;
            GenerateValues(_maxDiceCount);
        }

        public Dictionary<List<int>, int> DiceCombinationsCounter = new()
        {
            { new List<int>() {1}, 1  },
            { new List<int>() {2}, 1  },
            { new List<int>() {3}, 1  },
            { new List<int>() {4}, 1  },
            { new List<int>() {5}, 1  },
            { new List<int>() {6}, 1  },
        };

        // Get the total number of roll possibilities for an amount of dice.
        // Should be equal to 6 ^ diceCount?
        public int CombinationCountForDiceCount(int diceCount)
        {
            return DiceCombinationsCounter
                .Where(kvp => kvp.Key.Count == diceCount)
                .Aggregate(0, (sum, kvp) => sum + kvp.Value);
        }

        // For a set of dice, 1s and 5s can be rolled
        // Use recursion to figure out the posibilities
        private void GenerateValues(int maxDiceCount)
        {
            for (int i = 1; i < maxDiceCount; i++)
            {
                GenerateDiceCombinationsForAdditionalDice();
            }
        }

        private void GenerateDiceCombinationsForAdditionalDice()
        {
            var additionalDiceCombination = new Dictionary<List<int>, int>();

            foreach (var (diceCombo, _) in DiceCombinationsCounter)
            {
                for (int additionalDiceValue = 1; additionalDiceValue <= 6; additionalDiceValue++)
                {
                    var newDiceCombination = new List<int>(diceCombo) { additionalDiceValue };
                    newDiceCombination.Order();

                    if (additionalDiceCombination.ContainsKey(newDiceCombination))
                    {
                        additionalDiceCombination[newDiceCombination]++;
                    }
                    else
                    {
                        additionalDiceCombination[newDiceCombination] = 1;
                    }
                }
            }

            foreach (var (key, value) in additionalDiceCombination)
            {
                DiceCombinationsCounter.Add(key, value);
            }
        }

        private int ScoreDice(int dice)
        {
            if (dice == 1) return 100;
            if (dice == 5) return 50;
            return 0;
        }

        private int ScoreDiceCombination(List<int> diceCombinations)
        {
            // Functional Yeahhhh
            return diceCombinations.Aggregate(0, (sum, dice) => sum + ScoreDice(dice));
        }

        public Dictionary<int, double> PossibleScoring(int diceCount)
        {
            var scoresAndProbabilities = new Dictionary<int, double>();

            // Count the number of times a score is reached from a dice combination
            foreach (var (diceCombo, frequency) in DiceCombinationsCounter)
            {
                var score = ScoreDiceCombination(diceCombo);

                if (scoresAndProbabilities.ContainsKey(score))
                {
                    scoresAndProbabilities[score]++;
                }
                else
                {
                    scoresAndProbabilities[score] = 1;
                }
            }

            // Map the value in the dictionary from the count of the score to the probability of the score
            return scoresAndProbabilities.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value / DiceCombinationsCounter.Count
            );
        }
    }
}
