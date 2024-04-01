using Moq;
using Xunit;
using YahtzeePro.models;
using YahtzeePro.Optimum;

namespace YahtzeePro.tests.Optimum
{
    public class CalculationManagerTests
    {
        [Fact]
        public void QueueCalculation_WithItemInQueue_DoesntDuplicate()
        {
            // Arrange
            var mockOptimumCalculator = new Mock<IOptimumCalculator>();
            var calculationManager = new CalculationManager(mockOptimumCalculator.Object);
            var gameConfiguration = new GameConfiguration(500, 5);
            
            calculationManager.QueueCalculation(gameConfiguration);
            
            // Act
            calculationManager.QueueCalculation(gameConfiguration);
            
            // Assert
            //Assert.Equal(...calculationManager only has one in queue...);
        }
    }
}