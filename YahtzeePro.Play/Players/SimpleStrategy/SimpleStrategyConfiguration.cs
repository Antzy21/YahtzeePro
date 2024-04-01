using System;
using System.Collections.Generic;

namespace YahtzeePro.Play.Players.SimpleStrategy;

[Serializable]
public class SimpleStrategyConfiguration
{
    public Dictionary<int, int> WhenToBankWithNumberOfDice {get; set;}
}
