namespace YahtzeePro;

public class OptimumStrategyFileStorage : IOptimumStrategyRepository
{
    public List<string> Get()
    {
        var localappdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dataDir = Path.Combine(localappdata, "Antzy21", "YahtzeePro", "Data");

        List<string> listOfOptimumStrats = [];

        foreach (var winDir in Directory.GetDirectories(dataDir))
        {
            foreach (var diceDir in Directory.GetDirectories(winDir))
            {
                listOfOptimumStrats.Add(diceDir.Replace(dataDir, ""));
            }
        }

        return listOfOptimumStrats;
    }

    public OptimumStrategyData Get(int winningValue, int totalDice)
    {
        throw new NotImplementedException();
    }
}
