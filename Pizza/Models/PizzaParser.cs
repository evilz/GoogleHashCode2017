using System;
using System.IO;
using System.Linq;
using Pizza.Utils;

namespace Pizza.Models
{
    public static class PizzaParser
    {
        public static SlicingContext Parse(this TextReader textReader)
        {
            var metadata = textReader.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var rowCount = metadata[0];
            var colCount = metadata[1];
            var minIngredient = metadata[2];
            var maxSliceSize = metadata[3];

            var ingredients = Enumerable.Range(0, rowCount)
                .Select(row => ReadIngredientsFromRow(textReader.ReadLine()))
                .ToArray();
            
            CheckSize(ingredients, rowCount, colCount);

            return new SlicingContext(maxSliceSize, minIngredient, ingredients);
        }

        private static void CheckSize(Ingredient[][] ingredients, int rowCount, int colCount)
        {
            if (ingredients.Length != rowCount)
            {
                throw new InvalidOperationException("Missing rows ??");
            }

            if (ingredients[0].Length != colCount)
            {
                throw new InvalidOperationException("Missing column ??");
            }
        }

        private static Ingredient[] ReadIngredientsFromRow(string rowContent)
        {
            return rowContent.Select(c => c.ToIngredient()).ToArray();
        }
    }
}