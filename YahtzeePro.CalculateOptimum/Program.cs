﻿// See https://aka.ms/new-console-template for more information
using YahtzeePro;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!\n");

        int winningValue = 5000;
        int totalDice = 5;
        int initialStackCounterToReturnKnownValue = 2;
        int calculationIterations = 3;
        bool logAll = false;

        if (args.Length > 0)
        {
            winningValue = int.Parse(args[0]);
        }
        if (args.Length > 1)
        {
            totalDice = int.Parse(args[1]);
        }
        if (args.Length > 2)
        {
            initialStackCounterToReturnKnownValue = int.Parse(args[2]);
        }
        if (args.Length > 3)
        {
            calculationIterations = int.Parse(args[3]);
        }
        if (args.Length > 4)
        {
            logAll = bool.Parse(args[4]);
        }

        OptimumCalculator ProbabilitiesCalculator = new(
            winningValue,
            totalDice,
            initialStackCounterToReturnKnownValue,
            calculationIterations,
            logAll);

        string fileName = "scores.txt";

        Console.WriteLine("\nRegenerate results? (y/n)");
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
    }
}