using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Moq;
using Xunit;
using YahtzeePro.Core.Models;
using YahtzeePro.Optimum;

namespace YahtzeePro.tests.Optimum
{
    public class CalculationManagerTests
    {
        [Fact]
        public async Task QueueCalculation_QueueingOne_CallsCalculateOnce()
        {
            // Arrange
            var mockOptimumCalculator = new Mock<IOptimumCalculator>();
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();

            var calculationTask = new TaskCompletionSource();
            mockOptimumCalculator
                .Setup(m =>
                    m.Calculate(It.IsAny<GameConfiguration>(), It.IsAny<int>(), It.IsAny<int>())
                )
                .Callback(calculationTask.Task.Wait);
                
            var calculationManager = new CalculationManager(mockOptimumCalculator.Object, mockOptimumRepo.Object);
            var gameConfiguration = new GameConfiguration(500, 5);

            // Act
            calculationManager.QueueCalculation(gameConfiguration);

            calculationTask.SetResult();
            await calculationTask.Task;

            // Assert
            mockOptimumCalculator.Verify(
                m => m.Calculate(gameConfiguration, It.IsAny<int>(), It.IsAny<int>()),
                Times.Once()
            );
        }

        [Fact]
        public async Task Queue_After4UniqueItemsAreQueued_Contains3Items()
        {
            // Arrange
            var mockOptimumCalculator = new Mock<IOptimumCalculator>();
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();

            var calculateStartTask = new TaskCompletionSource();
            var calculateFinishTask = new TaskCompletionSource();
            mockOptimumCalculator
                .Setup(r =>
                    r.Calculate(It.IsAny<GameConfiguration>(), It.IsAny<int>(), It.IsAny<int>())
                )
                .Callback(() =>
                {
                    calculateStartTask.SetResult();
                    calculateFinishTask.Task.Wait();
                });

            var calculationManager = new CalculationManager(mockOptimumCalculator.Object, mockOptimumRepo.Object);

            // Act
            calculationManager.QueueCalculation(new GameConfiguration(500, 5));
            calculationManager.QueueCalculation(new GameConfiguration(500, 4));
            calculationManager.QueueCalculation(new GameConfiguration(500, 3));
            calculationManager.QueueCalculation(new GameConfiguration(500, 2));

            await calculateStartTask.Task;

            // Assert
            Assert.Equal(3, calculationManager.Queue.Count());
        }

        [Fact]
        public void QueueCalculation_WhenQueingItemAlreadyInQueue_DoesntDuplicate()
        {
            // Arrange
            var mockOptimumCalculator = new Mock<IOptimumCalculator>();
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();

            var saveTask = new TaskCompletionSource();
            mockOptimumRepo
                .Setup(r =>
                    r.Save(It.IsAny<GameConfiguration>(), It.IsAny<Dictionary<GameState, GameStateProbabilities>>())
                )
                .Callback(saveTask.Task.Wait);

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
        public async Task QueueCalculation_AfterCalculation_SavesToRepo()
        {
            // Arrange
            var mockOptimumCalculator = new Mock<IOptimumCalculator>();
            var mockOptimumRepo = new Mock<IOptimumStrategyRepository>();

            var saveTask = new TaskCompletionSource();
            mockOptimumRepo
                .Setup(r =>
                    r.Save(It.IsAny<GameConfiguration>(), It.IsAny<Dictionary<GameState, GameStateProbabilities>>())
                )
                .Callback(saveTask.SetResult);

            var calculationManager = new CalculationManager(mockOptimumCalculator.Object, mockOptimumRepo.Object);

            // Act
            calculationManager.QueueCalculation(new GameConfiguration(500, 5));
            await saveTask.Task;

            // Assert
            mockOptimumRepo.Verify(
                r => r.Save(It.IsAny<GameConfiguration>(), It.IsAny<Dictionary<GameState, GameStateProbabilities>>()),
                Times.Once()
            );
        }
    }
}