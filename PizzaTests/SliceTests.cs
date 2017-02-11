using System;
using System.Linq;
using NFluent;
using NUnit.Framework;
using Pizza.Models;
using static TestHelpers;
using Pizza = Pizza.Models.Pizza;

namespace PizzaTests
{
    public class SliceTests
    {


        [Test]
        public void ToStringTest()
        {
            const string expectedString =
@"TMM
MMM
MMT
";
            var context = ParseContext("3 3 1 4,TMM,MMM,MMT");
            
            Check.That(context.Pizza.ToString()).IsEqualTo(expectedString);
        }

        [TestCase("3 3 1 4,TMM,MMM,MMT", 7, 2, false)]
        [TestCase("3 3 3 9,TMM,MMM,MMT", 7, 2, false)]
        [TestCase("3 3 2 9,TMM,MMM,MMT", 7, 2, true)]
        [TestCase("3 3 3 9,MTT,TTT,TTM", 2, 7, false)]
        public void Test(string pizzaString, int expectedMushroom, int expectedTomato, bool shouldBeValid)
        {
            
            var context = ParseContext(pizzaString);
            
            Check.That(context.Pizza.Width)        .IsEqualTo(3);
            Check.That(context.Pizza.Height)       .IsEqualTo(3);
            Check.That(context.Pizza.Size)         .IsEqualTo(9);

            Check.That(context.Pizza.MushroomCount).IsEqualTo(expectedMushroom);
            Check.That(context.Pizza.TomatoCount)  .IsEqualTo(expectedTomato);

            Check.That(context.Pizza.MushroomCount + context.Pizza.TomatoCount).IsEqualTo(context.Pizza.Size);

           // Check.That(context.Pizza.IsValid).IsEqualTo(shouldBeValid);
        }

        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 1)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 2)]
        public void Valid_Horizontal_Cut_Test(string pizzaString, int firstSliceSize)
        {

            var context = ParseContext(pizzaString);
            var slicer = new Slicer(context);
            var tmp = slicer.Cut(context.Pizza, Direction.Horizontal, firstSliceSize);

            var topSlice = tmp.Item1;
            Check.That(topSlice.Width)
                .IsEqualTo(context.Pizza.Width);
            Check.That(topSlice.Height)
                .IsEqualTo(firstSliceSize);

            var bottomSlice = tmp.Item2;
            Check.That(bottomSlice.Width)
                .IsEqualTo(context.Pizza.Width);
            Check.That(bottomSlice.Height)
                .IsEqualTo(context.Pizza.Height - firstSliceSize);

            Check.That(topSlice.ToString() + bottomSlice.ToString())
                .IsEqualTo(context.Pizza.ToString());
        }

        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 1)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 2)]
        public void Valid_Vertical_Cut_Test(string pizzaString, int firstSliceSize)
        {
            var context = ParseContext(pizzaString);
            var slicer = new Slicer(context);
            var tmp = slicer.Cut(context.Pizza, Direction.Vertical, firstSliceSize);
            
            var leftSlice = tmp.Item1;
            Check.That(leftSlice.Height).IsEqualTo(context.Pizza.Height);
            Check.That(leftSlice.Width).IsEqualTo(firstSliceSize);

            var rightSlice = tmp.Item2;
            Check.That(rightSlice.Height).IsEqualTo(context.Pizza.Height);
            Check.That(rightSlice.Width).IsEqualTo(context.Pizza.Width - firstSliceSize);

            var leftSliceString = leftSlice.ToString().Split(new [] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            var rightSliceString = rightSlice.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var mergedString = leftSliceString.Zip(rightSliceString, (left, right) => left + right + Environment.NewLine)
                    .Aggregate(string.Empty, (accu, str) => accu + str);
            Check.That(mergedString).IsEqualTo(context.Pizza.ToString());
        }

        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Vertical,0)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Vertical,4)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Horizontal,0)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Horizontal,3)]
        public void Invalid_Cut_Should_Throw(string pizzaString, Direction direction, int firstSliceSize)
        {
            var context = ParseContext(pizzaString);
            var slicer = new Slicer(context);
            
            Check.ThatCode(() =>
                slicer.Cut(context.Pizza, direction, firstSliceSize))
            .Throws<ArgumentOutOfRangeException>();
        }


        [TestCase("3 3 1 4,TMM,MMM,MMM", 4)]
        [TestCase("3 3 1 4,TMM,MMM,MMT", 8)]
        [TestCase("3 3 1 4,TMM,MTM,MMT", 9)]
        public void Test_MaxPossiblePoints(string pizzaString, int expectedMaxPossiblePoints)
        {
            var context = ParseContext(pizzaString);
            var slicer = new Slicer(context);

            Check.That(slicer.MaxPossiblePoints(context.Pizza)).IsEqualTo(expectedMaxPossiblePoints);
        }
    }
}