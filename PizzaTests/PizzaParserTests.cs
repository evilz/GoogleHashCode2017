using System;
using System.IO;
using NFluent;
using NUnit.Framework;
using Pizza.Models;

namespace PizzaTests
{
    public class PizzaParserTests
    {
        [Test]
        public void PizzaParser_Should_Parse_Sample_And_Print_It_Back()
        {
            var pizzaString =
@"6 7 1 5
TMMMTTT
MMMMTMM
TTMTTMT
TMMTMMM
TTTTTTM
TTTTTTM
";
            var pizzaContext = PizzaParser.Parse(new StringReader(pizzaString));

            Check.That(pizzaContext.MaximumSliceSize).IsEqualTo(5);
            Check.That(pizzaContext.MinimumIngredientCount).IsEqualTo(1);

            Check.That(pizzaContext.Pizza.Height).IsEqualTo(6);
            Check.That(pizzaContext.Pizza.Width).IsEqualTo(7);

            Check.That(pizzaContext.Pizza[0,0]).IsEqualTo(Ingredient.Tomato);
            Check.That(pizzaContext.Pizza[0, 1]).IsEqualTo(Ingredient.Mushroom);
            Check.That(pizzaContext.Pizza[0, 2]).IsEqualTo(Ingredient.Mushroom);

            Check.That(pizzaContext.ToString()).IsEqualTo(pizzaString);
        }

        [Test]
        public void PizzaParser_Should_Throw_Exception_When_Parsing_Invalid_Pizza()
        {
            var pizzaString = "3 4 1 4,TMM,MMM,MMT".Replace(",", Environment.NewLine);
            Check.ThatCode(() => PizzaParser.Parse(new StringReader(pizzaString))).Throws<InvalidOperationException>();
        }
    }
}
