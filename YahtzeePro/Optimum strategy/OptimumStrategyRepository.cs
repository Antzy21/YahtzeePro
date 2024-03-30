using System.Text.Json;

namespace YahtzeePro;

public class OptimumStrategyFileStorage : IOptimumStrategyRepository
{
    private readonly string _optimumStrategyDirectory;

    public OptimumStrategyFileStorage()
    {
        _optimumStrategyDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Antzy21", "YahtzeePro", "Data");
        
        Directory.CreateDirectory(_optimumStrategyDirectory);
    }

    public List<string> Get()
    {
        List<string> listOfOptimumStrats = [];

        foreach (var winDir in Directory.GetDirectories(_optimumStrategyDirectory))
        {
            foreach (var diceDir in Directory.GetDirectories(winDir))
            {
                listOfOptimumStrats.Add(diceDir.Replace(_optimumStrategyDirectory, ""));
            }
        }

        return listOfOptimumStrats;
    }

    public OptimumStrategyData? Get(int winningValue, int totalDice)
    {
        var dir = Path.Combine(_optimumStrategyDirectory, $"Win{winningValue}", $"Dice{totalDice}");
    
        var fileName = Path.Combine(dir, "scores.txt");

        string[] gsDataLines;

        try
        {
            gsDataLines = File.ReadAllLines(fileName);
            Console.WriteLine($"Reading data from {fileName}. {gsDataLines.Length} lines.");
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
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

        return new OptimumStrategyData(optimumStrategyData);
    }

    public void Save(int winningValue, int totalDice, OptimumStrategyData optimumStrategyData)
    {
        var dir = Path.Combine(_optimumStrategyDirectory, $"Win{winningValue}", $"Dice{totalDice}");
    
        var fileName = Path.Combine(dir, "scores.txt");

        Console.WriteLine($"Writing data to {fileName}");

        Directory.CreateDirectory(dir);
        StreamWriter file = File.CreateText(fileName);

        foreach ((GameState gs, GameStateProbabilities probabilities) in optimumStrategyData.GameStateProbabilities)
        {
            string gsSerialised = JsonSerializer.Serialize(gs);
            file.Write(gsSerialised);
            file.WriteLine($"---{probabilities.RiskyPlay}---{probabilities.RiskyPlayProbability}---{probabilities.SafePlayProbability}");
        }

        file.Close();
    }
}
