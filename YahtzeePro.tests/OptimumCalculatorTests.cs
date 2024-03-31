using Microsoft.Extensions.Logging.Abstractions;
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
            var calculator = new OptimumCalculator(NullLogger<OptimumCalculator>.Instance);
            var result = calculator.Calculate(_winningValue, _totalDice);
            Assert.Equal(2, result.GameStateProbabilities.Count);
        }
    }
}