using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahtzeePro.Play.Players
{
    internal class Strategy2 : IPlayer
    {
        public string Name => "800:1,500:2";

        public PlayChoice GetMove(GameState gs)
        {
            if (gs.CachedScore >= 800 || gs.DiceToRoll < 2)
            {
                return PlayChoice.Safe;
            }
            if (gs.CachedScore >= 500 || gs.DiceToRoll < 3)
            {
                return PlayChoice.Safe;
            }
            return PlayChoice.Risky;
        }
    }
}
