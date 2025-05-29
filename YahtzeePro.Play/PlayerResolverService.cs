using YahtzeePro.Play.Players.AutoPlayers;

namespace YahtzeePro.Play;

public class PlayerResolverService : IPlayerResolverService
{
    public IAutoPlayer ResolveAutoPlayer(string playerString)
    {
        return playerString.ToLower() switch
        {
            "optimum" => GetOptimumPlayer(),
            "rolltowin" => new RollToWin(),
            _ => ResolveSimpleStrategy(playerString),
        };
    }

    private IAutoPlayer ResolveSimpleStrategy(string playerString)
    {
        throw new NotImplementedException();
    }

    private IAutoPlayer GetOptimumPlayer()
    {
        throw new NotImplementedException();
    }
}