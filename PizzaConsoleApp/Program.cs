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
            var inputFile = Path.GetFullPath(@"..\..\input\03-big.in");

            var outputFile = Path.ChangeExtension(inputFile, ".out");

            var context = PizzaParser.Parse(File.OpenText(inputFile));
            var slicer = new Slicer(context);
            var stopwatch = Stopwatch.StartNew();

            var bestWayToCut = slicer.FindBestWayToCut(context.Pizza);
            stopwatch.Stop();

            File.WriteAllText(outputFile, bestWayToCut.ToOutputString());

            var elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Slicing took : {elapsed.Hours}H {elapsed.Minutes}M {elapsed.Seconds}S {elapsed.Milliseconds}MS");
            Console.WriteLine($"{bestWayToCut.PointEarned} / {context.Pizza.Size} ({bestWayToCut.PointEarned / context.Pizza.Size:P})");

            Console.ReadKey();
        }
    }
}
