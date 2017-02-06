using System;
using System.Linq;
using NFluent;
using NUnit.Framework;
using Pizza.Models;

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
            var pizzaString = "3 3 1 4,TMM,MMM,MMT".Replace(",", Environment.NewLine);
            var slice = PizzaParser.Parse(pizzaString).ToSlice();

            Check.That(slice.ToString()).IsEqualTo(expectedString);
        }

        [TestCase("3 3 1 4,TMM,MMM,MMT", 7, 2, false)]
        [TestCase("3 3 3 9,TMM,MMM,MMT", 7, 2, false)]
        [TestCase("3 3 2 9,TMM,MMM,MMT", 7, 2, true)]
        [TestCase("3 3 3 9,MTT,TTT,TTM", 2, 7, false)]
        public void Test(string pizzaString, int expectedMushroom, int expectedTomato, bool shouldBeValid)
        {
            pizzaString = pizzaString.Replace(",", Environment.NewLine);

            var slice = PizzaParser.Parse(pizzaString).ToSlice();

            Check.That(slice.Width)        .IsEqualTo(3);
            Check.That(slice.Height)       .IsEqualTo(3);
            Check.That(slice.Size)         .IsEqualTo(9);

            Check.That(slice.MushroomCount).IsEqualTo(expectedMushroom);
            Check.That(slice.TomatoCount)  .IsEqualTo(expectedTomato);

            Check.That(slice.MushroomCount + slice.TomatoCount).IsEqualTo(slice.Size);

            Check.That(slice.IsValid).IsEqualTo(shouldBeValid);
        }

        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 1)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 2)]
        public void Valid_Horizontal_Cut_Test(string pizzaString, int firstSliceSize)
        {
            pizzaString = pizzaString.Replace(",", Environment.NewLine);

            var originalSlice = PizzaParser.Parse(pizzaString).ToSlice();
            
            var tmp = originalSlice.Cut(Direction.Horizontal, firstSliceSize);

            var topSlice = tmp.Item1;
            Check.That(topSlice.Width)
                .IsEqualTo(originalSlice.Width);
            Check.That(topSlice.Height)
                .IsEqualTo(firstSliceSize);

            var bottomSlice = tmp.Item2;
            Check.That(bottomSlice.Width)
                .IsEqualTo(originalSlice.Width);
            Check.That(bottomSlice.Height)
                .IsEqualTo(originalSlice.Height - firstSliceSize);

            Check.That(topSlice.ToString() + bottomSlice.ToString())
                .IsEqualTo(originalSlice.ToString());
        }

        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 1)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", 2)]
        public void Valid_Vertical_Cut_Test(string pizzaString, int firstSliceSize)
        {
            pizzaString = pizzaString.Replace(",", Environment.NewLine);

            var originalSlice = PizzaParser.Parse(pizzaString).ToSlice();

            var tmp = originalSlice.Cut(Direction.Vertical, firstSliceSize);

            var leftSlice = tmp.Item1;
            Check.That(leftSlice.Height).IsEqualTo(originalSlice.Height);
            Check.That(leftSlice.Width).IsEqualTo(firstSliceSize);

            var rightSlice = tmp.Item2;
            Check.That(rightSlice.Height).IsEqualTo(originalSlice.Height);
            Check.That(rightSlice.Width).IsEqualTo(originalSlice.Width - firstSliceSize);

            var leftSliceString = leftSlice.ToString().Split(new [] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            var rightSliceString = rightSlice.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var mergedString = leftSliceString.Zip(rightSliceString, (left, right) => left + right + Environment.NewLine)
                    .Aggregate(string.Empty, (accu, str) => accu + str);
            Check.That(mergedString).IsEqualTo(originalSlice.ToString());
        }

        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Vertical,0)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Vertical,4)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Horizontal,0)]
        [TestCase("3 4 1 4,TMMM,MMMM,MMTM", Direction.Horizontal,3)]
        public void Invalid_Cut_Should_Throw(string pizzaString, Direction direction, int firstSliceSize)
        {
            pizzaString = pizzaString.Replace(",", Environment.NewLine);

            var originalSlice = PizzaParser.Parse(pizzaString).ToSlice();

            Check.ThatCode(() =>
                originalSlice.Cut(direction, firstSliceSize))
            .Throws<ArgumentOutOfRangeException>();
        }


        [TestCase("3 3 1 4,TMM,MMM,MMM", 4)]
        [TestCase("3 3 1 4,TMM,MMM,MMT", 8)]
        [TestCase("3 3 1 4,TMM,MTM,MMT", 9)]
        public void Test_MaxPossiblePoints(string pizzaString, int expectedMaxPossiblePoints)
        {
            pizzaString = pizzaString.Replace(",", Environment.NewLine);
            var slice = PizzaParser.Parse(pizzaString).ToSlice();

            Check.That(slice.MaxPossiblePoints).IsEqualTo(expectedMaxPossiblePoints);
        }
    }
}