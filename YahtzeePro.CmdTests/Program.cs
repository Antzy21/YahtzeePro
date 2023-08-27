// See https://aka.ms/new-console-template for more information
using YahtzeePro;

Console.WriteLine("Hello, World!");

int winningValue = 5000;
int totalDice = 5;
int initialStackCounterToReturnKnownValue = 4;
int calculationIterations = 5;

ProbabilitiesCalculator ProbabilitiesCalculator = new(
    winningValue,
    totalDice,
    initialStackCounterToReturnKnownValue,
    calculationIterations,
    logAll: false);

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