using YahtzeePro;
using YahtzeePro.models;

internal interface ICalculationManager
{
    public void QueueCalculation(GameConfiguration gc);
}