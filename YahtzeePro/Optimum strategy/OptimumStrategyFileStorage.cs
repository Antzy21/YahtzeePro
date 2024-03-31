using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace YahtzeePro;

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

    public OptimumStrategyData? Get(int winningValue, int totalDice)
    {
        string fileName = GetFileName(winningValue, totalDice);

        string[] gsDataLines;

        try
        {
            gsDataLines = File.ReadAllLines(fileName);
            _logger.LogInformation("Reading data from {fileName}. {gsDataLinesLength} lines.", fileName, gsDataLines.Length);
        }
        catch (DirectoryNotFoundException ex)
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

        return new OptimumStrategyData(optimumStrategyData);
    }

    public void Save(int winningValue, int totalDice, OptimumStrategyData optimumStrategyData)
    {
        var fileName = GetFileName(winningValue, totalDice);

        _logger.LogInformation("Writing data to {fileName}", fileName);

        StreamWriter file = File.CreateText(fileName);

        foreach ((GameState gs, GameStateProbabilities probabilities) in optimumStrategyData.GameStateProbabilities)
        {
            string gsSerialised = JsonSerializer.Serialize(gs);
            file.Write(gsSerialised);
            file.WriteLine($"---{probabilities.RiskyPlay}---{probabilities.RiskyPlayProbability}---{probabilities.SafePlayProbability}");
        }

        file.Close();
    }

    private string GetFileName(int winningValue, int totalDice)
    {
        return Path.Combine(_optimumStrategyDirectory, $"win{winningValue}_dice{totalDice}_scores.txt");
    }
}
