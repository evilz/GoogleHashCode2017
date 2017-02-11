using System;
using System.Drawing;
using System.Linq;
using static System.Math;

namespace Pizza.Models
{
    public class Slicer
    {
        private readonly SlicingContext _context;

        public Slicer(SlicingContext context)
        {
            _context = context;
        }

        public bool IsValid(Slice slice)
        {
            return IsSmallEnough(slice) && HaveRequestedIngredient(slice);
        }

        public bool IsSmallEnough(Slice slice)
        {
            return slice.Size <= _context.MaximumSliceSize;
        }

        public bool HaveRequestedIngredient(Slice slice)
        {
            return slice.MushroomCount >= _context.MinimumIngredientCount
                   && slice.TomatoCount >= _context.MinimumIngredientCount;
        }

        public int MaxPossiblePoints(Slice slice)
        {
            var maxNumberOfSlice = Min(slice.MushroomCount, slice.TomatoCount) / _context.MinimumIngredientCount;
            return Min(slice.Size, maxNumberOfSlice * _context.MaximumSliceSize);
        }
        
        public SlicingChallengeResponse FindBestWayToCut(Slice slice)
        {
            if (IsValid(slice)) { return new SlicingChallengeResponse(slice); }

            if (!HaveRequestedIngredient(slice)) { return SlicingChallengeResponse.Empty; }
            
            var bestWayToCut = SlicingChallengeResponse.Empty;

            var horizontalWayToCut = Enumerable.Range(1, slice.Height - 1)
                .Select(i => new WayToCut {Direction = Direction.Horizontal, SliceSize = i});
            var verticalWayToCut = Enumerable.Range(1, slice.Width - 1)
                .Select(i => new WayToCut {Direction = Direction.Vertical, SliceSize = i});

            var allPossibleWayToCut = horizontalWayToCut.Union(verticalWayToCut).ToList();

            allPossibleWayToCut.ForEach( cut =>
            {
                var tmp = Cut(slice, cut.Direction, cut.SliceSize);

                var slice1 = tmp.Item1;
                var slice2 = tmp.Item2;

                var bestWayToCutTop = FindBestWayToCut(slice1);
                var bestWayToCutBot = FindBestWayToCut(slice2);

                if (bestWayToCutTop.PointEarned + bestWayToCutBot.PointEarned > bestWayToCut.PointEarned)
                {
                    bestWayToCut = new SlicingChallengeResponse(bestWayToCutTop.ValidSlices.Union(bestWayToCutBot.ValidSlices));
                }
            });

            return bestWayToCut;
        }


        public Tuple<Slice, Slice> Cut(Slice slice, Direction direction, int firstSliceSize)
        {
            Slice slice1, slice2;

            if (direction == Direction.Horizontal)
            {
                if (firstSliceSize <= 0 || firstSliceSize >= slice.Height)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(firstSliceSize),
                        firstSliceSize,
                        $"0 < {nameof(firstSliceSize)} < {slice.Height} (Height)");
                }

                slice1 = new Slice(_context.Ingredients, slice.TopLeft, new Point(slice.BottomRight.X, slice.TopLeft.Y + firstSliceSize - 1));
                slice2 = new Slice(_context.Ingredients, new Point(slice.TopLeft.X, slice.TopLeft.Y + firstSliceSize), slice.BottomRight);
            }
            else
            {
                if (firstSliceSize <= 0 || firstSliceSize >= slice.Width)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(firstSliceSize),
                        firstSliceSize,
                        $"0 < {nameof(firstSliceSize)} < {slice.Width} (Width)");
                }

                slice1 = new Slice(_context.Ingredients, slice.TopLeft, new Point(slice.TopLeft.X + firstSliceSize - 1, slice.BottomRight.Y));
                slice2 = new Slice(_context.Ingredients, new Point(slice.TopLeft.X + firstSliceSize, slice.TopLeft.Y), slice.BottomRight);
            }

            return Tuple.Create(slice1, slice2);
        }

    }
}
    

