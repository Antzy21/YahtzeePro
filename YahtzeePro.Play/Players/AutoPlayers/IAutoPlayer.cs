using YahtzeePro.Core.Models;

namespace YahtzeePro.Play.Players.AutoPlayers;

public interface IAutoPlayer : IPlayer
{
    MoveChoice GetMove(GameState gs, GameConfiguration gc);
}