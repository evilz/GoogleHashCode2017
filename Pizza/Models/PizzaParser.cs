using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Pizza.Utils;

namespace Pizza.Models
{
    public class PizzaParser
    {
        private static readonly Regex MetadataRegex = new Regex(@"(?<line>\d+) (?<column>\d+) (?<ingredient>\d+) (?<slicesize>\d+)");

        private static SlicingContext Parse(TextReader textReader)
        {
            var metadata = textReader.ReadLine();

            var m = MetadataRegex.Match(metadata);

            var minIngredient = int.Parse(m.Groups["ingredient"].Value);
            var maxSliceSize = int.Parse(m.Groups["slicesize"].Value);
            var nbColumn = int.Parse(m.Groups["column"].Value);
            var nbLine = int.Parse(m.Groups["line"].Value);

            var ingredients = new Ingredient[nbColumn * nbLine];

            Enumerable
                .Range(0, nbLine)
                .ForEach(row =>
                {
                    var lineContent = textReader.ReadLine();
                    lineContent.ForEach((c, col) =>
                    {
                        ingredients[col + row * nbColumn] = c.ToIngredient();
                    });
                });

            if (ingredients.Any(i => i == Ingredient.Unset))
            {
                throw new InvalidOperationException();
            }

            var pizza = new Pizza(nbLine, nbColumn, ingredients);

            return new SlicingContext(maxSliceSize, minIngredient, pizza);
        }

        public static SlicingContext Parse(string content)
        {
            return Parse(new StringReader(content));
        }

        public static SlicingContext ParseFile(string filePath)
        {
            return Parse(new StreamReader(File.OpenRead(filePath)));
        }
    }
}