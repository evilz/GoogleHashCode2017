using System;
using System.IO;
using Pizza.Models;

static internal class TestHelpers
{
    public static SlicingChallengeResponse SlicingPizza( string pizzaString)
    {   
        var context = ParseContext(pizzaString);
        var slicer = new Slicer(context);
        var cutStrat = slicer.FindBestWayToCut(context.Pizza);
        return cutStrat;
    }

    public static SlicingContext ParseContext(string pizzaString)
    {
        var context = PizzaParser.Parse(new StringReader(pizzaString.Replace(",", Environment.NewLine)));
        return context;
    }
}