using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using YahtzeePro.Core.Models;
using YahtzeePro.Play.Requests;
using YahtzeePro.Play.Responses;

namespace YahtzeePro.Cli.Services;

public class ApiCommandService : ICommandService
{
    private readonly HttpClient _optimumClient;
    private readonly HttpClient _playClient;

    public ApiCommandService(
        IConfiguration configuration
    )
    {
        var baseOptimumAddress = configuration["OptimumApiUrl"];
        if (string.IsNullOrEmpty(baseOptimumAddress))
        {
            throw new ArgumentException("Optimum API URL is not configured.");
        }

        var basePlayAddress = configuration["PlayApiUrl"];
        if (string.IsNullOrEmpty(basePlayAddress))
        {
            throw new ArgumentException("Play API URL is not configured.");
        }

        _optimumClient = new HttpClient { BaseAddress = new Uri(baseOptimumAddress) };
        _playClient = new HttpClient { BaseAddress = new Uri(basePlayAddress) };
    }

    public void Status()
    {
        var optimumApiResponse = _optimumClient.GetAsync("/status");
        if (optimumApiResponse.Result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("Connected to YatzeePro Optimum API server!");
        }
        else
        {
            Console.WriteLine("Unable to reach YatzeePro Optimum API server");
        }

        var playApiResponse = _playClient.GetAsync("/");
        if (playApiResponse.Result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("Connected to YatzeePro Play API server!");
        }
        else
        {
            Console.WriteLine("Unable to reach YatzeePro Play API server");
        }
    }

    public void CalculateOptimum(int winningValue, int totalDice)
    {
        var gameConfiguration = new GameConfiguration(winningValue, totalDice);
        var content = JsonContent.Create(gameConfiguration);
        var response = _optimumClient.PostAsync("/calculate", content);

        Console.WriteLine(response.Result.Content.ReadAsStringAsync().Result);
    }

    public void GetOptimum(int winningValue, int totalDice)
    {
        var gameConfiguration = new GameConfiguration(winningValue, totalDice);
        var content = JsonContent.Create(gameConfiguration);
        var response = _optimumClient.PostAsync("/getStrategy", content);

        Console.WriteLine(response.Result.Content);

        var x = response.Result.Content.ReadAsStringAsync().Result;
        Console.WriteLine(x);
    }

    public void ListOptimums()
    {
        var response = _optimumClient.GetAsync("");
        var optimums = response.Result.Content.ReadFromJsonAsync<List<string>>().Result ?? [];

        foreach (var optimum in optimums)
        {
            Console.WriteLine(optimum);
        }
    }

    public void ListGames()
    {
        var response = _playClient.GetAsync("/games");

        var games = response.Result.Content.ReadFromJsonAsync<List<Guid>>().Result ?? [];

        foreach (var game in games)
        {
            Console.WriteLine(game);
        }
    }

    public void NewGame(string opponent, int winningValue, int totalDice)
    {
        var gameConfiguration = new GameConfiguration(winningValue, totalDice);
        var newGameRequest = new NewGameRequest(gameConfiguration, opponent);
        var content = JsonContent.Create(newGameRequest);
        var response = _playClient.PostAsync("/newgame", content);

        if (response.Result.StatusCode == System.Net.HttpStatusCode.Created)
        {
            var locationUri = response.Result.Headers.Location;
            if (locationUri != null)
            {
                Console.WriteLine(locationUri);
                var newGameResponse = _playClient.GetAsync(locationUri);
                var gameResponse = newGameResponse.Result.Content.ReadFromJsonAsync<GameResponse>().Result!;
                Console.WriteLine(gameResponse.GameState);
            }
            else
            {
                Console.WriteLine("New game created, but no location URI was returned.");
            }
        }
        else
        {
            Console.WriteLine("Failed to create a new game.");
            Console.WriteLine(response.Exception?.Message);
        }
    }

    public void Move(MoveChoice moveChoice, Guid gameId)
    {
        var moveRequest = new MoveRequest(gameId, moveChoice);
        var response = _playClient.PostAsJsonAsync("/move", moveRequest);
        if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var gameResponse = response.Result.Content.ReadFromJsonAsync<GameResponse>().Result!;
            if (gameResponse.LastDiceRoll != null)
            {
                PrettyPrintDie(gameResponse.LastDiceRoll);
            }
            Console.WriteLine($"Cached: {gameResponse.GameState.CachedScore}");
            Console.WriteLine($"Score: {gameResponse.GameState.PlayerScore} - {gameResponse.GameState.OpponentScore}");
        }
        else if (response.Result.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"No game found with ID {gameId}.");
        }
        else
        {
            Console.WriteLine("Failed to make move.");
            Console.WriteLine(response.Exception?.Message);
        }
    }

    private static void PrettyPrintDie(DiceCombination die)
    {
        var totalDie = 0;
        for (int i = 1; i <= 6; i++)
        {
            totalDie += die.DiceCount[i];
        }
        Console.WriteLine(string.Concat(Enumerable.Repeat(" ---  ", totalDie)));

        for (int i = 1; i <= 6; i++)
        {
            Console.Write(string.Concat(Enumerable.Repeat($"| {i} | ", die.DiceCount[i])));
        }
        Console.WriteLine();

        Console.WriteLine(string.Concat(Enumerable.Repeat(" ---  ", totalDie)));

    }

    public void Simulate(string strategy1, string strategy2, int numberOfGames, int numberOfSets, int winningValue, int totalDice)
    {
        var gameConfiguration = new GameConfiguration(winningValue, totalDice);
        var simulateGameRequest = new SimulateGamesRequest(strategy1, strategy2, numberOfGames, numberOfSets, gameConfiguration);
        var content = JsonContent.Create(simulateGameRequest);
        var response = _playClient.PostAsync("/simulate", content);

        var simulatedResults = response.Result.Content.ReadFromJsonAsync<List<GameSetResult>>().Result ?? [];

        // For Sets of Games
        if (simulatedResults.Count > 1)
        {
            foreach (var result in simulatedResults)
            {
                Console.WriteLine($"{strategy1}: {result.PlayerOneWinCount}");
                Console.WriteLine($"{strategy2}: {result.PlayerTwoWinCount}");
                Console.WriteLine();
            }
        }
        // For Single Set Simulation
        else if (simulatedResults.Count == 1)
        {
            var gameSetResult = simulatedResults.Single();
            foreach (var gameResult in gameSetResult.GameResults)
            {
                Console.WriteLine($"Game Result: {gameResult.WinningPlayer}");
                Console.WriteLine($"Score: {gameResult.WinnerScore} - {gameResult.LoserScore}");
                Console.WriteLine();
            }
        }
    }
}
