// See https://aka.ms/new-console-template for more information
using YahtzeePro;

Console.WriteLine("Hello, World!");

var winningValue = 5000;
var totalDice = 5;
var initialStackCounterToReturnKnownValue = 4;
var calculationIterations = 5;

var ProbabilitiesCalculator = new ProbabilitiesCalculator(
    winningValue,
    totalDice,
    initialStackCounterToReturnKnownValue,
    calculationIterations,
    logAll: false);

//ProbabilitiesCalculator.PopulateGameStateProbabilities();

var fileName = "scores.txt";

//ProbabilitiesCalculator.WriteDataToFile(fileName);

ProbabilitiesCalculator.ReadDataFromFile(fileName);