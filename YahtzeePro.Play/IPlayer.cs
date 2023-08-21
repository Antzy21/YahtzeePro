using YahtzeePro;
using YahtzeePro.Play;

internal interface IPlayer
{
    PlayChoice GetMove(GameState gameState);
}