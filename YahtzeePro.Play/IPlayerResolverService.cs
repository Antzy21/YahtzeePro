using YahtzeePro.Play.Players;

namespace YahtzeePro.Play;

public interface IPlayerResolverService
{
    IPlayer ResolvePlayer(string playerString);
}