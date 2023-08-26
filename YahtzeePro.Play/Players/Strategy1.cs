using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahtzeePro.Play.Players
{
    internal class Strategy1 : SimpleStrategyBase, IPlayer
    {
        public Strategy1()
        {
            WhenToBankWith1Dice = 600;
            WhenToBankWith2Dice = 1000;
        }
    }
}
