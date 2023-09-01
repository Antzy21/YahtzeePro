using Xunit;

namespace YahtzeePro.tests
{
    public class RollPosibilityTests
    {
        // For zero dice, there is a dice combination of nothing, with a 100% chance of this.
        [Fact]
        public void RollPosibilitiesForZeroDice()
        {
            var rp = new RollPosibilities(0);
            Assert.Single(rp.DiceCountToScoresToProbabilities);
        }

        // For one dice, there are 3 possible scores, from 3 dice combinations.

        // Two include scoring dice, 
        [Fact]
        public void RollPosibilitiesForOneDice()
        {
            var rp = new RollPosibilities(1);
            Assert.Equal(2, rp.DiceCountToScoresToProbabilities.Count);

            Dictionary<int, double> zeroScoringDiceProbability = rp.DiceCountToScoresToProbabilities[0];
            Assert.Equal(4d / 6d, zeroScoringDiceProbability[0]);
        }

        [Fact]
        public void NumberOfScoringDiceCombinationsEqualToDiceAvailable()
        {
            for (int i = 0; i < 5; i++)
            {
                var rp = new RollPosibilities(i);
                Assert.Equal(i + 1, rp.DiceCountToScoresToProbabilities.Count);
            }
        }
    }
}
