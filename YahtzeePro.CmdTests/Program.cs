// See https://aka.ms/new-console-template for more information
using YahtzeePro;

Console.WriteLine("Hello, World!");

int winningValue = 2000;
int totalDice = 3;
int initialStackCounterToReturnKnownValue = 2;
int calculationIterations = 3;

OptimumCalculator ProbabilitiesCalculator = new(
    winningValue,
    totalDice,
    initialStackCounterToReturnKnownValue,
    calculationIterations,
    logAll: true);

string fileName = "scores.txt";

Console.WriteLine("Regenerate results? (y/n)");
string regenerate = Console.ReadLine()!;

if (regenerate == "y")
{
    ProbabilitiesCalculator.PopulateGameStateProbabilities();
    ProbabilitiesCalculator.WriteDataToFile(fileName);
}
else
{
    ProbabilitiesCalculator.ReadDataFromFile(fileName);
}