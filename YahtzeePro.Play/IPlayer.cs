using YahtzeePro;
using YahtzeePro.Play;

internal interface IPlayer
{
    string Name { get; }
    PlayChoice GetMove(GameState gs, GameConfiguration gc);
}