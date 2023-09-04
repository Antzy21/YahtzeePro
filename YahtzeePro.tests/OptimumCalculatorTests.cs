using Xunit;

namespace YahtzeePro.tests
{
    public class OptimumCalculatorTests
    {
        private readonly int _winningValue = 50;
        private readonly int _totalDice = 1;

        [Fact]
        public void SimpleCaseCheckSizeOfResults()
        {
            var calculator = new OptimumCalculator(_winningValue, _totalDice);
            calculator.PopulateGameStateProbabilities();
            var result = calculator.gameStateProbabilitiesRisky;
            Assert.Equal(2, result.Count);
        }
    }
}