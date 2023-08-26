using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahtzeePro.Play.Players
{
    internal class Strategy2 : SimpleStrategyBase, IPlayer
    {
        public Strategy2()
        {
            WhenToBankWith1Dice = 500;
            WhenToBankWith2Dice = 800;
        }
    }
}
