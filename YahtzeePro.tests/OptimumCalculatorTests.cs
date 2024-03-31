using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace YahtzeePro.tests
{
    public class OptimumCalculatorTests
    {
        private readonly GameConfiguration _gameConfiguration = new(WinningValue: 50, TotalDice: 1);

        [Fact]
        public void SimpleCaseCheckSizeOfResults()
        {
            var calculator = new OptimumCalculator(NullLogger<OptimumCalculator>.Instance);
            var result = calculator.Calculate(_gameConfiguration);
            Assert.Equal(2, result.GameStateProbabilities.Count);
        }
    }
}