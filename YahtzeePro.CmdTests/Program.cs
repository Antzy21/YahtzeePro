// See https://aka.ms/new-console-template for more information
using YahtzeePro;

Console.WriteLine("Hello, World!");

var winningValue = 2000;
var totalDice = 5;
var initialStackCounterToReturnKnownValue = 3;
var calculationIterations = 5;

var ProbabilitiesCalculator = new ProbabilitiesCalculator(
    winningValue,
    totalDice,
    initialStackCounterToReturnKnownValue,
    calculationIterations,
    logAll: true);

ProbabilitiesCalculator.PopulateGameStateProbabilities();