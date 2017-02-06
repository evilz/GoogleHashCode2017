using System.Text;
using Pizza.Utils;

namespace Pizza.Models
{
    public class SlicingContext
    {
        internal SlicingContext(int maximumSliceSize, int minimumIngredientCount, Pizza pizza)
        {
            MaximumSliceSize = maximumSliceSize;
            MinimumIngredientCount = minimumIngredientCount;
            Pizza = pizza;
        }

        #region Properties
        public Pizza Pizza { get; }
        public int MaximumSliceSize { get; }
        public int MinimumIngredientCount { get; }
        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{Pizza.Line} {Pizza.Column} {MinimumIngredientCount} {MaximumSliceSize}");

            for (var row = 0; row < Pizza.Line; row++)
            {
                for (var col = 0; col < Pizza.Column; col++)
                {
                    sb.Append(Pizza.GetIngredientAt(col: col, row: row).ToSingleCharString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public Slice ToSlice()
        {
            return new Slice(this, 0, 0, Pizza.Column - 1, Pizza.Line - 1);
        }
    }
}