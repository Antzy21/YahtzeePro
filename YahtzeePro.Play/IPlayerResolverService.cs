using YahtzeePro.Play.Players.AutoPlayers;

namespace YahtzeePro.Play;

public interface IPlayerResolverService
{
    IAutoPlayer ResolveAutoPlayer(string playerString);
}