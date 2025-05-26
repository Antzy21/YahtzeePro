using YahtzeePro.Core.Models;

namespace YahtzeePro.Play.Players;

public interface IPlayer
{
    string Name { get; }
    MoveChoice GetMove(GameState gs, GameConfiguration gc);
}