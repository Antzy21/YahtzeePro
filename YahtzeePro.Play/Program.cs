using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using YahtzeePro.models;
using YahtzeePro.Play.Players;
using YahtzeePro.Play.Players.SimpleStrategy;

namespace YahtzeePro.Play;

internal class Program
{
    private static async Task Main(string[] args)
    {
        int winningValue = 1000;
        int totalDice = 5;

        if (args.Length > 0)
        {
            winningValue = int.Parse(args[0]);
        }
        if (args.Length > 1)
        {
            totalDice = int.Parse(args[1]);
        }
        if (args.Length > 2)
        {
            Console.WriteLine("Too many arguements passed. Expecting 'winningValue' and 'totalDice'");
        }

        GameConfiguration gameConfiguration = new(winningValue, totalDice);

        Console.WriteLine("Duel!");

        IPlayer rollToWinPlayer = new RollToWin();

        IPlayer optimumPlayer = await GetOptimumStrategyPlayer(gameConfiguration);

        IPlayer strategy1Player = GetSimpleStrategyPlayer("Players/SimpleStrategy/Configurations/strategy1.json");

        IPlayer strategy2Player = GetSimpleStrategyPlayer("Players/SimpleStrategy/Configurations/strategy2.json");

        var setOfGames = new SetOfGames(strategy1Player, strategy2Player, gameConfiguration);

        setOfGames.PlaySetOfSets(
            totalGames: 100,
            totalSets: 100,
            logging: true);
    }

    private static SimpleStrategy GetSimpleStrategyPlayer(string file)
    {
        var strategy1Json = File.ReadAllText(file);
        var strategy1 = JsonSerializer.Deserialize<SimpleStrategyConfiguration>(strategy1Json);
        var strategy1Player = new SimpleStrategy(strategy1);
        return strategy1Player;
    }

    private static async Task<IPlayer> GetOptimumStrategyPlayer(GameConfiguration gc)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8080/")
        };
        var getOptimumStrategyResponse = await httpClient.GetAsync($"getOptimumStrategy?winningValue={gc.WinningValue}&diceCount={gc.TotalDice}");

        if (getOptimumStrategyResponse.StatusCode == System.Net.HttpStatusCode.NotFound) {
            Console.WriteLine($"No optimum has been calculated for {gc}. Calculating request sent");
            await httpClient.GetAsync($"calculate?winningValue={gc.WinningValue}&diceCount={gc.TotalDice}");
            throw new Exception($"No optimum calculation found for game configuration {gc}");
        }
        var optimumStrategyData = await getOptimumStrategyResponse.Content.ReadFromJsonAsync<List<KeyValuePair<GameState, GameStateProbabilities>>>();
        return new OptimumPlayer(optimumStrategyData.ToDictionary(x => x.Key, x => x.Value));
    }
}