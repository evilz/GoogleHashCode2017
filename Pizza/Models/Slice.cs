using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Pizza.Utils;
using static System.Math;

namespace Pizza.Models
{
    public class Slice : IEnumerable<IEnumerable<Ingredient>>
    {
        protected readonly Ingredient[][] _ingredients;
        
        
        public Point TopLeft { get; }
        public Point BottomRight { get; }

        public int Width => BottomRight.X - TopLeft.X + 1;
        public int Height => BottomRight.Y - TopLeft.Y + 1;

        public int Size => Width * Height;

        public int TomatoCount { get; private set; }
        public int MushroomCount { get; private set; }
        
        public Ingredient this[int y, int x] => _ingredients[y][x];

        public Slice([NotNull]Ingredient[][] ingredients, Point topLeft,  Point bottomRight)
        {
            _ingredients = ingredients;

            TopLeft = topLeft;
            BottomRight = bottomRight;

            this.SelectMany(i => i).ForEach(ingredient =>
            {
                switch (ingredient)
                {
                    case Ingredient.Tomato:
                        TomatoCount++;
                        break;
                    case Ingredient.Mushroom:
                        MushroomCount++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(ingredient), ingredient, null);
                }
            });
        
        }

      
        public override string ToString()
        {
            var sb = new StringBuilder();
            this.ForEach(line =>
            {
                sb.Append(line.Select(col => col.ToSingleCharString()).ToArray()).AppendLine();
            } );
            //sb.Remove(sb.Length - Environment.NewLine.Length, Environment.NewLine.Length);
            return sb.ToString();
        }
        public IEnumerator<IEnumerable<Ingredient>> GetEnumerator()
        {
            return Enumerable.Range(TopLeft.Y, Height)
                    .Select(y => _ingredients[y].Skip(TopLeft.X).Take(Width))
                    .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}