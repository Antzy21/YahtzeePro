﻿using System;
using System.Collections.Generic;

namespace YahtzeePro.Play;

[Serializable]
public class SimpleStrategyConfiguration
{
    public readonly string Name;
    public readonly Dictionary<int, int> WhenToBankWithNumberOfDice;
}
