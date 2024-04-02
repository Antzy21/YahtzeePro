using System.Runtime.InteropServices.JavaScript;
using Moq;
using Xunit;
using YahtzeePro.models;
using YahtzeePro.Optimum;

namespace YahtzeePro.tests.Optimum
{
    public class CalculationManagerTests
    {
        [Fact]
        public void QueueCalculation_QueueingOne_CallsCalculateOnce()
        {
            // Arrange
            var mockOptimumCalculator = GetMockOptimumCalculator();
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();
            var calculationManager = new CalculationManager(mockOptimumCalculator.Object, mockOptimumRepo.Object);
            var gameConfiguration = new GameConfiguration(500, 5);
            
            // Act
            calculationManager.QueueCalculation(gameConfiguration);
            
            // Assert
            mockOptimumCalculator.Verify(
                m => m.Calculate(
                    It.IsAny<GameConfiguration>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()),
                Times.Once()
            );
        }
        
        [Fact]
        public void QueueCalculation_QueueingTwo_CallsCalculateOnceThenTwice()
        {
            // Arrange
            var mockOptimumCalculator = GetMockOptimumCalculator();
            
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();
            var calculationManager = new CalculationManager(mockOptimumCalculator.Object, mockOptimumRepo.Object);
            var gameConfiguration1 = new GameConfiguration(500, 5);
            var gameConfiguration2 = new GameConfiguration(500, 3);
            
            // Act
            calculationManager.QueueCalculation(gameConfiguration1);
            calculationManager.QueueCalculation(gameConfiguration2);
            
            // Assert
            mockOptimumCalculator.Verify(
                m => m.Calculate(
                    It.IsAny<GameConfiguration>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()),
                Times.Once()
            );
            
            Thread.Sleep(150);
            
            mockOptimumCalculator.Verify(
                m => m.Calculate(
                    It.IsAny<GameConfiguration>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()),
                Times.Exactly(2)
            );
        }
        
        [Fact]
        public void Queue_After4UniqueItemsAreQueued_Shows3Items()
        {
            // Arrange
            var mockOptimumCalculator = GetMockOptimumCalculator();
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();
            var calculationManager = new CalculationManager(mockOptimumCalculator.Object, mockOptimumRepo.Object);
            
            // Act
            calculationManager.QueueCalculation(new GameConfiguration(500, 5));
            calculationManager.QueueCalculation(new GameConfiguration(500, 4));
            calculationManager.QueueCalculation(new GameConfiguration(500, 3));
            calculationManager.QueueCalculation(new GameConfiguration(500, 2));
            
            // Assert
            Assert.Equal(4, calculationManager.Queue.Count());
        }
        
        [Fact]
        public void QueueCalculation_WithItemInQueue_DoesntDuplicate()
        {
            // Arrange
            var mockOptimumCalculator = GetMockOptimumCalculator();
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();
            var calculationManager = new CalculationManager(mockOptimumCalculator.Object, mockOptimumRepo.Object);
            var gameConfiguration1 = new GameConfiguration(500, 5);
            var gameConfiguration2 = new GameConfiguration(500, 3);
            
            calculationManager.QueueCalculation(gameConfiguration1);
            
            Thread.Sleep(20);
            
            // Act
            calculationManager.QueueCalculation(gameConfiguration2);
            calculationManager.QueueCalculation(gameConfiguration2);
            
            // Assert
            Assert.Single(calculationManager.Queue);
        }

        private static Mock<IOptimumCalculator> GetMockOptimumCalculator()
        {
            var mockOptimumCalculator = new Mock<IOptimumCalculator>();
            mockOptimumCalculator.Setup(
                m => m.Calculate(
                    It.IsAny<GameConfiguration>(),
                    It.IsAny<int>(),
                    It.IsAny<int>())
            ).Callback(() => Thread.Sleep(100));
            return mockOptimumCalculator;
        }
    }
}