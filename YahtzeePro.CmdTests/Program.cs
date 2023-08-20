// See https://aka.ms/new-console-template for more information
using YahtzeePro;

Console.WriteLine("Hello, World!");

var winningValue = 2000;
var totalDice = 5;
var maxIterationCounter = 3;

var ProbabilitiesCalculator = new ProbabilitiesCalculator(
    winningValue,
    totalDice,
    maxIterationCounter);

ProbabilitiesCalculator.PopulateGameStateProbabilities();