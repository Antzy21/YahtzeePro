using System.Text.Json;
namespace YahtzeePro
{
    public class OptimumStrategy
    {
        private protected readonly string _dir;
        private protected readonly string _fileName;

        public Dictionary<GameState, double> gameStateProbabilities = new();
        public Dictionary<GameState, double> gameStateProbabilitiesRisky = new();
        public Dictionary<GameState, double> gameStateProbabilitiesSafe = new();

        public OptimumStrategy(int winningValue, int totalDice)
        {
            var localappdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _dir = Path.Combine(localappdata, "Antzy21", "YahtzeePro", "Data", $"Win{winningValue}", $"Dice{totalDice}");
            _fileName = Path.Combine(_dir, "scores.txt");
        }

        public void ReadDataFromFile()
        {
            string[] gsDataLines;

            try
            {
                gsDataLines = File.ReadAllLines(_fileName);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine($"Reading data from {_fileName}. {gsDataLines.Length} lines.");

            foreach (string gsData in gsDataLines)
            {
                string gsSerialised = gsData.Split("---")[0];
                string probability = gsData.Split("---")[1];
                string rollProbability = gsData.Split("---")[2];
                string bankProbability = gsData.Split("---")[3];

                GameState gs = JsonSerializer.Deserialize<GameState>(gsSerialised)!;

                gameStateProbabilities[gs] = double.Parse(probability);
                gameStateProbabilitiesRisky[gs] = double.Parse(rollProbability);
                gameStateProbabilitiesSafe[gs] = double.Parse(bankProbability);
            }

            Console.WriteLine("Finished reading");
        }
    }
}