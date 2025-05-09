using YahtzeePro.Core.Models;

namespace YahtzeePro.Optimum;

public interface ICalculationManager
{
    public IEnumerable<GameConfiguration> Queue { get; }
    public void QueueCalculation(GameConfiguration gc);
}