// See https://aka.ms/new-console-template for more information
using YahtzeePro;

Console.WriteLine("Hello, World!");

var winningValue = 2000;
var totalDice = 3;
var initialStackCounterToReturnKnownValue = 4;
var calculationIterations = 5;

var ProbabilitiesCalculator = new ProbabilitiesCalculator(
    winningValue,
    totalDice,
    initialStackCounterToReturnKnownValue,
    calculationIterations,
    logAll: false);

ProbabilitiesCalculator.PopulateGameStateProbabilities();

var fileName = "../../../../Win5000Dice5/stack4Iter5.txt";

ProbabilitiesCalculator.WriteDataToFile(fileName);

var newProbCalc = new ProbabilitiesCalculator(
    winningValue,
    totalDice);

newProbCalc.ReadDataFromFile(fileName);

Console.WriteLine(newProbCalc);