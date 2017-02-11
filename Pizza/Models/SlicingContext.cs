using System.Text;

namespace Pizza.Models
{
    public class SlicingContext
    {
        internal SlicingContext(int maximumSliceSize, int minimumIngredientCount, Ingredient[][] ingredients)
        {
            MaximumSliceSize = maximumSliceSize;
            MinimumIngredientCount = minimumIngredientCount;
            Ingredients = ingredients;
            Pizza = new Pizza(ingredients);
        }

        #region Properties
        public Pizza Pizza { get; }
        public int MaximumSliceSize { get; }
        public int MinimumIngredientCount { get; }
        public Ingredient[][] Ingredients { get; }

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{Pizza.Height} {Pizza.Width} {MinimumIngredientCount} {MaximumSliceSize}");
            sb.Append(Pizza.ToString());
            return sb.ToString();
        }

        //public Slice ToSlice()
        //{
        //    return new Slice(this, 0, 0, Pizza.Column - 1, Pizza.Line - 1);
        //}
    }
}