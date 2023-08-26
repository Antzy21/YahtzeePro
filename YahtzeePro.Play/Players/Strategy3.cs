using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahtzeePro.Play.Players
{
    internal class Strategy3 : SimpleStrategyBase, IPlayer
    {
        public Strategy3()
        {
            WhenToBankWith1Dice = 200;
            WhenToBankWith2Dice = 400;
            WhenToBankWith3Dice = 600;
        }
    }
}
