using System.Text.Json;
using YahtzeePro.Play.Players.AutoPlayers;
using YahtzeePro.Play.Players.AutoPlayers.SimpleStrategyConfigurations;

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

    private static SimpleStrategy ResolveSimpleStrategy(string playerString)
    {
        var simpleStrategyJson = File.ReadAllText($"Players/AutoPlayers/SimpleStrategyConfigurations/{playerString}.json");
        var simpleStrategyConfiguration = JsonSerializer.Deserialize<SimpleStrategyConfiguration>(simpleStrategyJson);

        if (simpleStrategyConfiguration == null)
        {
            throw new ArgumentException($"Invalid SimpleStrategy configuration: {playerString}");
        }
        var simpleStrategyPlayer = new SimpleStrategy(simpleStrategyConfiguration);
        return simpleStrategyPlayer;
    }

    private IAutoPlayer GetOptimumPlayer()
    {
        throw new NotImplementedException();
    }
}