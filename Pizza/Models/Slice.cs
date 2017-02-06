using System;
using System.Linq;
using System.Text;
using Pizza.Utils;
using static System.Math;

namespace Pizza.Models
{
    public class Slice
    {
        private readonly SlicingContext _slicingContext;

        public Slice(SlicingContext context, int topLeftCol, int topLeftRow, int bottomRightCol, int bottomRightRow)
        {
            _slicingContext = context;

            TopLeftCol = topLeftCol;
            TopLeftRow = topLeftRow;

            BottomRightCol = bottomRightCol;
            BottomRightRow = bottomRightRow;

            for (var col = TopLeftCol; col <= BottomRightCol; col++)
            {
                for (var row = TopLeftRow; row <= BottomRightRow; row++)
                {
                    var ingredient = _slicingContext.Pizza.GetIngredientAt(col: col, row: row);

                    switch (ingredient)
                    {
                        case Ingredient.Tomato:
                            TomatoCount++;
                            continue;
                        case Ingredient.Mushroom:
                            MushroomCount++;
                            continue;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(ingredient), ingredient, null);
                    }
                }
            }
        }

        public Tuple<Slice, Slice> Cut(Direction direction, int firstSliceSize)
        {
            Slice slice1, slice2;

            if (direction == Direction.Horizontal)
            {
                if (firstSliceSize <= 0 || firstSliceSize >= Height)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(firstSliceSize),
                        firstSliceSize,
                        $"0 < {nameof(firstSliceSize)} < {Height} (Height)");
                }

                slice1 = new Slice(_slicingContext,
                    TopLeftCol, TopLeftRow,
                    BottomRightCol, TopLeftRow + firstSliceSize - 1);
                slice2 = new Slice(_slicingContext,
                    TopLeftCol, TopLeftRow + firstSliceSize,
                    BottomRightCol, BottomRightRow);
            }
            else
            {
                if (firstSliceSize <= 0 || firstSliceSize >= Width)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(firstSliceSize),
                        firstSliceSize,
                        $"0 < {nameof(firstSliceSize)} < {Width} (Width)");
                }


                slice1 = new Slice(_slicingContext,
                    TopLeftCol, TopLeftRow,
                    TopLeftCol + firstSliceSize - 1, BottomRightRow);
                slice2 = new Slice(_slicingContext,
                    TopLeftCol + firstSliceSize, TopLeftRow,
                    BottomRightCol, BottomRightRow);
            }

            return Tuple.Create(slice1, slice2);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var row = TopLeftRow; row <= BottomRightRow; row++)
            {
                for (var col = TopLeftCol; col <= BottomRightCol; col++)
                {
                    sb.Append(_slicingContext.Pizza.GetIngredientAt(col: col, row: row).ToSingleCharString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        #region Properties
        public int TopLeftCol { get; }
        public int TopLeftRow { get; }
        public int BottomRightCol { get; }
        public int BottomRightRow { get; }

        public int Width => BottomRightCol - TopLeftCol + 1;
        public int Height => BottomRightRow - TopLeftRow + 1;
        public int Size => Width * Height;

        public int TomatoCount { get; }
        public int MushroomCount { get; }

        public bool IsValid => IsSmallEnough && HaveRequestedIngredient;
        private bool IsSmallEnough => Size <= _slicingContext.MaximumSliceSize;
        private bool HaveRequestedIngredient => MushroomCount >= _slicingContext.MinimumIngredientCount
                   && TomatoCount >= _slicingContext.MinimumIngredientCount;

        public int MaxPossiblePoints
        {
            get
            {
                var maxNumberOfSlice = Min(MushroomCount, TomatoCount)/_slicingContext.MinimumIngredientCount;
                return Min(Size, maxNumberOfSlice*_slicingContext.MaximumSliceSize);
            }
        }

        #endregion

        public SlicingChallengeResponse FindBestWayToCut(bool allowParallelism = true)
        {
            if (IsValid)
            {
                return new SlicingChallengeResponse
                {
                    ValidSlices = { this }
                };
            }

            if (!HaveRequestedIngredient)
            {
                return SlicingChallengeResponse.Empty;
            }

            var parallelOrNot = ParallelismHelper.ForEach<WayToCut>(allowParallelism);
            
            var bestWayToCut = SlicingChallengeResponse.Empty;

            var horizontalWayToCut = Enumerable.Range(1, Height - 1)
                .Select(i => new WayToCut{Direction = Direction.Horizontal, SliceSize = i});
            var verticalWayToCut = Enumerable.Range(1, Width - 1)
                .Select(i => new WayToCut { Direction = Direction.Vertical, SliceSize = i });

            var allPossibleWayToCut = horizontalWayToCut.Union(verticalWayToCut).ToList();

            parallelOrNot(allPossibleWayToCut, (cut =>
            {
                var tmp = Cut(cut.Direction, cut.SliceSize);

                var slice1 = tmp.Item1;
                var slice2 = tmp.Item2;

                var bestWayToCutTop = slice1.FindBestWayToCut(false);
                var bestWayToCutBot = slice2.FindBestWayToCut(false);

                if (bestWayToCutTop.PointEarned + bestWayToCutBot.PointEarned > bestWayToCut.PointEarned)
                {
                    bestWayToCut = new SlicingChallengeResponse
                    {
                        ValidSlices = bestWayToCutTop.ValidSlices.Union(bestWayToCutBot.ValidSlices).ToList()
                    };
                }
            }));

            return bestWayToCut;
        }
    }
}