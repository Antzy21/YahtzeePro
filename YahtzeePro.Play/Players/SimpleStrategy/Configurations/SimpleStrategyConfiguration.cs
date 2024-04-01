using System;
using System.Collections.Generic;

namespace YahtzeePro.Play;

[Serializable]
public class SimpleStrategyConfiguration
{
    public Dictionary<int, int> WhenToBankWithNumberOfDice {get; set;}
}
