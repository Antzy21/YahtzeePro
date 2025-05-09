using System.Text.Json;
using Microsoft.Extensions.Logging;
using YahtzeePro.Core.Models;

namespace YahtzeePro.Optimum;

public class OptimumStrategyFileStorage : IOptimumStrategyRepository
{
    private readonly string _optimumStrategyDirectory;
    private readonly ILogger _logger;

    public OptimumStrategyFileStorage(ILogger<IOptimumStrategyRepository> logger)
    {
        _logger = logger;

        _optimumStrategyDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Antzy21", "YahtzeePro", "Data");
        
        Directory.CreateDirectory(_optimumStrategyDirectory);
    }

    public List<string> Get()
    {
        List<string> listOfOptimumStrats = [];

        foreach (var strategyFile in Directory.GetFiles(_optimumStrategyDirectory))
        {
            listOfOptimumStrats.Add(strategyFile.Replace(_optimumStrategyDirectory, ""));
        }

        return listOfOptimumStrats;
    }

    public Dictionary<GameState, GameStateProbabilities>? Get(GameConfiguration gameConfiguration)
    {
        string fileName = GetFileName(gameConfiguration);

        string[] gsDataLines;

        try
        {
            gsDataLines = File.ReadAllLines(fileName);
            _logger.LogInformation("Reading data from {fileName}. {gsDataLinesLength} lines.", fileName, gsDataLines.Length);
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning("Optimum Strategy File not found: {message}", ex.Message);
            return null;
        }

        var optimumStrategyData = new Dictionary<GameState, GameStateProbabilities>();

        foreach (string gsData in gsDataLines)
        {
            string gsSerialised = gsData.Split("---")[0];
            string probability = gsData.Split("---")[1];
            string rollProbability = gsData.Split("---")[2];
            string bankProbability = gsData.Split("---")[3];

            GameState gs = JsonSerializer.Deserialize<GameState>(gsSerialised)!;

            var gameStateProbabilities = new GameStateProbabilities(
                bool.Parse(probability),
                double.Parse(rollProbability),
                double.Parse(bankProbability)
            );

            optimumStrategyData.Add(gs, gameStateProbabilities);
        }

        _logger.LogInformation("Finished reading");

        return optimumStrategyData;
    }

    public void Save(GameConfiguration gameConfiguration, Dictionary<GameState, GameStateProbabilities> optimumStrategyData)
    {
        var fileName = GetFileName(gameConfiguration);

        _logger.LogInformation("Writing data to {fileName}", fileName);

        StreamWriter file = File.CreateText(fileName);

        foreach ((GameState gs, GameStateProbabilities probabilities) in optimumStrategyData)
        {
            string gsSerialised = JsonSerializer.Serialize(gs);
            file.Write(gsSerialised);
            file.WriteLine($"---{probabilities.RiskyPlay}---{probabilities.RiskyPlayProbability}---{probabilities.SafePlayProbability}");
        }

        file.Close();
    }

    private string GetFileName(GameConfiguration gameConfiguration)
    {
        return Path.Combine(_optimumStrategyDirectory, $"win{gameConfiguration.WinningValue}_dice{gameConfiguration.TotalDice}_scores.txt");
    }
}
