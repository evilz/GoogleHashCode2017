using System;
using System.Diagnostics;
using System.IO;
using Pizza.Models;

namespace PizzaConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = @"D:\Workspace\CSharp\GoogleHashCode2017\PizzaConsoleApp\input\small.in";

            var outputFile = Path.ChangeExtension(inputFile, ".out");

            var slice = PizzaParser.ParseFile(inputFile).ToSlice();

            var stopwatch = Stopwatch.StartNew();

            var bestWayToCut = slice.FindBestWayToCut();
            stopwatch.Stop();

            File.WriteAllText(outputFile, bestWayToCut.ToOutputString());

            var elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Slicing took : {elapsed.Hours}H {elapsed.Minutes}M {elapsed.Seconds}S {elapsed.Milliseconds}MS");
            Console.WriteLine($"{bestWayToCut.PointEarned} / {slice.Size} ({bestWayToCut.PointEarned / slice.Size:P})");

            Console.ReadKey();
        }
    }
}
