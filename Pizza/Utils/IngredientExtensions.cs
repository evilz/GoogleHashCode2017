using System;
using Pizza.Models;

namespace Pizza.Utils
{
    public static class IngredientExtensions
    {
        public static char ToSingleCharString(this Ingredient ingredient)
        {
            switch (ingredient)
            {
                case Ingredient.Mushroom:
                    return 'M';
                case Ingredient.Tomato:
                    return 'T';
                default:
                    throw new ArgumentOutOfRangeException(nameof(ingredient), ingredient, null);
            }
        }

        public static Ingredient ToIngredient(this char @char)
        {
            switch (@char)
            {
                case 'M':
                    return Ingredient.Mushroom;
                case 'T':
                    return Ingredient.Tomato;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@char), @char, null);
            }
        }
    }
}