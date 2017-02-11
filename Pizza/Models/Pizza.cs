using System.Drawing;
using JetBrains.Annotations;

namespace Pizza.Models
{
    public class Pizza :  Slice
    {   public Pizza([NotNull]Ingredient[][] ingredients) : base(ingredients, new Point(0,0), new Point(ingredients[0].Length - 1, ingredients.Length - 1))
        {
        }
    }
}