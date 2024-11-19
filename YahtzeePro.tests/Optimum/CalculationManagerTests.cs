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
            var mockOptimumCalculator = GetMockOptimumCalculator(out var taskCompletionSource);
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();
            var calculationManager = new CalculationManager(mockOptimumCalculator.Object, mockOptimumRepo.Object);
            var gameConfiguration = new GameConfiguration(500, 5);

            // Act
            calculationManager.QueueCalculation(gameConfiguration);

            taskCompletionSource.SetResult();
            Thread.Sleep(50);

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
        public void Queue_After4UniqueItemsAreQueued_Shows3Items()
        {
            // Arrange
            var mockOptimumCalculator = GetMockOptimumCalculator(out var _);
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();
            var calculationManager = new CalculationManager(mockOptimumCalculator.Object, mockOptimumRepo.Object);

            // Act
            calculationManager.QueueCalculation(new GameConfiguration(500, 5));
            calculationManager.QueueCalculation(new GameConfiguration(500, 4));
            calculationManager.QueueCalculation(new GameConfiguration(500, 3));
            calculationManager.QueueCalculation(new GameConfiguration(500, 2));
            Thread.Sleep(50);

            // Assert
            Assert.Equal(3, calculationManager.Queue.Count());
        }

        [Fact]
        public void QueueCalculation_WithItemInQueue_DoesntDuplicate()
        {
            // Arrange
            var mockOptimumCalculator = GetMockOptimumCalculator(out var _);
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();
            var calculationManager = new CalculationManager(mockOptimumCalculator.Object, mockOptimumRepo.Object);
            var gameConfiguration1 = new GameConfiguration(500, 5);
            var gameConfiguration2 = new GameConfiguration(500, 3);

            calculationManager.QueueCalculation(gameConfiguration1);

            // Act
            calculationManager.QueueCalculation(gameConfiguration2);
            calculationManager.QueueCalculation(gameConfiguration2);

            // Assert
            Assert.Single(calculationManager.Queue);
        }

        [Fact]
        public void QueueCalculation_AfterCalculation_SavesToRepo()
        {
            // Arrange
            var gameConfiguration1 = new GameConfiguration(500, 5);
            var mockOptimumCalculator = GetMockOptimumCalculator(out var taskCompletionSource);
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();
            mockOptimumRepo.Setup(r => r.Save(
                gameConfiguration1,
                It.IsAny<Dictionary<GameState, GameStateProbabilities>>()
            ));
            var calculationManager = new CalculationManager(mockOptimumCalculator.Object, mockOptimumRepo.Object);

            // Act
            calculationManager.QueueCalculation(gameConfiguration1);
            taskCompletionSource.SetResult();
            Thread.Sleep(50);

            // Assert
            mockOptimumRepo.Verify(
                r => r.Save(
                    gameConfiguration1,
                    It.IsAny<Dictionary<GameState, GameStateProbabilities>>()),
                Times.Once()
            );
        }

        private static Mock<IOptimumCalculator> GetMockOptimumCalculator(out TaskCompletionSource completionTask)
        {
            var mockOptimumCalculator = new Mock<IOptimumCalculator>();
            completionTask = new TaskCompletionSource();

            mockOptimumCalculator.Setup(
                m => m.Calculate(
                    It.IsAny<GameConfiguration>(),
                    It.IsAny<int>(),
                    It.IsAny<int>())
            ).Callback(completionTask.Task.Wait);
            
            return mockOptimumCalculator;
        }
    }
}