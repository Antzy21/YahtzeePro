using YahtzeePro.models;

namespace YahtzeePro.Optimum;

public interface ICalculationManager
{
    public void QueueCalculation(GameConfiguration gc);
}