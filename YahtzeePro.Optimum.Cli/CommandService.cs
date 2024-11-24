using System.CommandLine;

public class CommandService()
{
    private readonly RootCommand _rootCommand = new();

    public void Invoke(string[] args)
    {
        _rootCommand.SetHandler(() =>
        {
            Console.WriteLine("Yahtzee Optimum CLI");
        });

        _rootCommand.Invoke(args);
    }
}