using YahtzeePro.Core.Models;

namespace YahtzeePro.Play;

internal interface IPlayer
{
    string Name { get; }
    MoveChoice GetMove(GameState gs, GameConfiguration gc);
}