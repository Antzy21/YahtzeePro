using Xunit;
using YahtzeePro.Core;

namespace YahtzeePro.tests
{
    public class DiceCombinationTests
    {
        [Theory]
        [InlineData(150, 1, 2, 3, 4, 5, 6)]
        [InlineData(200, 2, 2, 2, 4)]
        [InlineData(600, 6, 6, 6)]
        [InlineData(4100, 4, 4, 4, 4, 1)]
        [InlineData(1000, 1, 1, 1, 1)]
        public void Score_IsCalculatedCorrectly(int expectedScore, params int[] dice)
        {
            var diceCombo = DiceCombinationGenerator.FromDieList(dice);
            Assert.Equal(expectedScore, diceCombo.Score);
        }
        
        [Theory]
        [InlineData("1,2,3,4,5,6,", 1, 2, 3, 4, 5, 6)]
        [InlineData("2,2,2,4,", 2, 2, 2, 4)]
        [InlineData("6,6,6,", 6, 6, 6)]
        [InlineData("1,4,4,4,4,", 4, 4, 4, 4, 1)]
        [InlineData("1,1,1,1,", 1, 1, 1, 1)]
        public void ToString_IsCorrect(string expectedString, params int[] dice)
        {
            var diceCombo = DiceCombinationGenerator.FromDieList(dice);
            Assert.Equal(expectedString, diceCombo.ToString());
        }

        [Theory]
        [InlineData(2, 1, 2, 3, 4, 5, 6)]
        [InlineData(3, 2, 2, 2, 4)]
        [InlineData(3, 6, 6, 6)]
        [InlineData(5, 4, 4, 4, 4, 1)]
        [InlineData(4, 1, 1, 1, 1)]
        public void NumberOfScoringDice_IsCalculatedCorrectly(int expectedNumberOfScoringDice, params int[] dice)
        {
            var diceCombo = DiceCombinationGenerator.FromDieList(dice);
            Assert.Equal(expectedNumberOfScoringDice, diceCombo.NumberOfScoringDice);
        }

        [Theory]
        [InlineData(false, 1, 2, 3, 4, 5, 6)]
        [InlineData(false, 2, 2, 2, 4)]
        [InlineData(true, 6, 6, 6)]
        [InlineData(true, 4, 4, 4, 4, 1)]
        [InlineData(true, 1, 1, 1, 1)]
        public void AllDiceAreScoring_IsCorrect(bool expectedBool, params int[] dice)
        {
            var diceCombo = DiceCombinationGenerator.FromDieList(dice);
            Assert.Equal(expectedBool, diceCombo.AllDiceAreScoring);
        }
    }
}
