using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using YahtzeePro.Core.Models;
using YahtzeePro.Play.Requests;

namespace YahtzeePro.Cli.Services;

public class ApiCommandService : ICommandService
{
    private readonly HttpClient _optimumClient;
    private readonly HttpClient _playClient;

    public ApiCommandService(
        IConfiguration configuration
    ) {
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
                Console.WriteLine(newGameResponse.Result.Content.ReadFromJsonAsync<GameState>().Result);
            }
            else
            {
                Console.WriteLine("New game created, but no location URI was returned.");
            }
        }
        else
        {
            Console.WriteLine("Failed to create a new game.");
            Console.WriteLine(response.Result.Content.ReadAsStringAsync().Result);
        }
    }

    public void Move(MoveChoice moveChoice, Guid gameId)
    {
        var moveRequest = new MoveRequest(gameId, moveChoice);
        var response = _playClient.PostAsJsonAsync("/move", moveRequest);
        Console.WriteLine(response.Result.Content.ReadAsStringAsync().Result);
    }
}
