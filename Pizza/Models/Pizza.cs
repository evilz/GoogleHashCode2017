namespace Pizza.Models
{
    public class Pizza
    {
        public int Line { get; }
        public int Column { get; }

        private readonly Ingredient[] _ingredients;

        public Pizza(int line, int column, Ingredient[] ingredients)
        {
            Line = line;
            Column = column;
            _ingredients = ingredients;

        }

        public Ingredient GetIngredientAt(int col, int row)
        {
            return _ingredients[col + row * Column];
        }

        public Ingredient this[int col, int row] => GetIngredientAt(col, row);
    }
}