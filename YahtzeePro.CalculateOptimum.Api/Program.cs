using YahtzeePro;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", ListOfOptimumStrats);

        app.MapGet("/calculate", (int winningValue = 0, int diceCount = 0) => CalculateOptimum(winningValue, diceCount));

        app.Run();
    }

    private static string CalculateOptimum(int winningValue = 0, int diceCount = 0)
    {
        return $"Test success: {winningValue}, {diceCount}";
    }

    private static List<string> ListOfOptimumStrats()
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
}