using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using YahtzeePro.models;

namespace YahtzeePro.Cli.Services;

public class ApiCommandService : ICommandService
{
    private readonly HttpClient _client;

    public ApiCommandService(
        IConfiguration configuration
    ) {
        var baseAddress = configuration["OptimumStrategyRepository"];
        if (string.IsNullOrEmpty(baseAddress))
        {
            throw new ArgumentException("OptimumStrategyRepository URL is not configured.");
        }

        _client = new HttpClient { BaseAddress = new Uri(baseAddress) };
    }
    
    public void Status()
    {
        Console.WriteLine("Not yet Implemented.");
    }

    public void CalculateOptimum(int winningValue, int totalDice)
    {
        Console.WriteLine("Not yet Implemented.");
    }

    public void GetOptimum(int winningValue, int totalDice)
    {
        var gameConfiguration = new GameConfiguration(winningValue, totalDice);
        var content = JsonContent.Create(gameConfiguration);
        var response = _client.PostAsync("/getStrategy", content);

        Console.WriteLine(response.Result.Content);

        var x = response.Result.Content.ReadAsStringAsync().Result;
        Console.WriteLine(x);
    }

    public void ListOptimums()
    {
        var response = _client.GetAsync("");
        var optimums = response.Result.Content.ReadFromJsonAsync<List<string>>().Result ?? [];

        foreach (var optimum in optimums)
        {
            Console.WriteLine(optimum);
        }
    }
}
