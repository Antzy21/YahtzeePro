using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahtzeePro.Play.Players
{
    internal class Strategy1 : IPlayer
    {
        public string Name => "1000:1,600:2";

        public PlayChoice GetMove(GameState gs)
        {
            if (gs.CachedScore >= 1000 || gs.DiceToRoll <= 1)
            {
                return PlayChoice.Safe;
            }
            if (gs.CachedScore >= 600 || gs.DiceToRoll <= 2)
            {
                return PlayChoice.Safe;
            }
            return PlayChoice.Risky;
        }
    }
}
