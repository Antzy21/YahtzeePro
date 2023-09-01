using Xunit;

namespace YahtzeePro.tests
{
    public class DiceCombinationTests
    {
        [Fact]
        public void ComboCase1()
        {
            var diceCombo = new DiceCombination(1, 2, 3, 4, 5, 6);
            Assert.Equal(150, diceCombo.Score);
            Assert.Equal(2, diceCombo.NumberOfScoringDice);
            Assert.Equal("1,2,3,4,5,6,", diceCombo.ToString());
            Assert.False(diceCombo.AllDiceAreScoring);
        }

        [Fact]
        public void ComboCase2()
        {
            var diceCombo = new DiceCombination(2, 2, 2, 4);
            Assert.Equal(200, diceCombo.Score);
            Assert.Equal(3, diceCombo.NumberOfScoringDice);
            Assert.Equal("2,2,2,4,", diceCombo.ToString());
            Assert.False(diceCombo.AllDiceAreScoring);
        }

        [Fact]
        public void ComboCase3()
        {
            var diceCombo = new DiceCombination(6, 6, 6);
            Assert.Equal(600, diceCombo.Score);
            Assert.Equal(3, diceCombo.NumberOfScoringDice);
            Assert.Equal("6,6,6,", diceCombo.ToString());
            Assert.True(diceCombo.AllDiceAreScoring);
        }

        [Fact]
        public void ComboCase4()
        {
            var diceCombo = new DiceCombination(4, 4, 4, 4, 1);
            Assert.Equal(4100, diceCombo.Score);
            Assert.Equal(5, diceCombo.NumberOfScoringDice);
            Assert.Equal("1,4,4,4,4,", diceCombo.ToString());
            Assert.True(diceCombo.AllDiceAreScoring);
        }
    }
}
