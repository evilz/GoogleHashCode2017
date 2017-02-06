using System;
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
            var pizzaContext = PizzaParser.Parse(pizzaString);

            Check.That(pizzaContext.MaximumSliceSize).IsEqualTo(5);
            Check.That(pizzaContext.MinimumIngredientCount).IsEqualTo(1);

            Check.That(pizzaContext.Pizza.Line).IsEqualTo(6);
            Check.That(pizzaContext.Pizza.Column).IsEqualTo(7);

            Check.That(pizzaContext.Pizza.GetIngredientAt(col: 0, row: 0)).IsEqualTo(Ingredient.Tomato);
            Check.That(pizzaContext.Pizza.GetIngredientAt(col: 1, row: 0)).IsEqualTo(Ingredient.Mushroom);
            Check.That(pizzaContext.Pizza.GetIngredientAt(col: 2, row: 0)).IsEqualTo(Ingredient.Mushroom);

            Check.That(pizzaContext.ToString()).IsEqualTo(pizzaString);
        }

        [Test]
        public void PizzaParser_Should_Throw_Exception_When_Parsing_Invalid_Pizza()
        {
            var pizzaString = "3 4 1 4,TMM,MMM,MMT".Replace(",", Environment.NewLine);
            Check.ThatCode(() => PizzaParser.Parse(pizzaString)).Throws<InvalidOperationException>();
        }
    }
}
