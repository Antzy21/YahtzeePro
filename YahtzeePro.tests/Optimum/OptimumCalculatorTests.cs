using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using YahtzeePro.Core.Models;
using YahtzeePro.Optimum;

namespace YahtzeePro.tests.Optimum
{
    public class OptimumCalculatorTests
    {
        private readonly GameConfiguration _gameConfiguration = new(WinningValue: 50, TotalDice: 1);

        [Fact]
        public void SimpleCaseCheckSizeOfResults()
        {
            var calculator = new OptimumCalculator(NullLogger<OptimumCalculator>.Instance); 
            var result = calculator.Calculate(_gameConfiguration);
            Assert.Equal(2, result.Count);
        }
    }
}