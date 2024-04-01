using YahtzeePro.models;

namespace YahtzeePro.Play;

internal interface IPlayer
{
    string Name { get; }
    PlayChoice GetMove(GameState gs, GameConfiguration gc);
}