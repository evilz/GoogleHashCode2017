using System;
using System.IO;
using NFluent;
using NUnit.Framework;
using Pizza.Models;
using static TestHelpers;

namespace PizzaTests
{
    public class SliceFindBestWayToCutTests
    {
        [Test]
        public void Find_Best_Way_To_Cut_A_Single_Ingredient_Pizza()
        {
            var cutStrat = TestHelpers.SlicingPizza("1 1 1 1,M");

            Check.That(cutStrat.PointEarned).IsEqualTo(0);
            Check.That(cutStrat.ValidSlices).HasSize(0);
        }

        [Test]
        public void Find_Best_Way_To_Cut_A_Tiny_Pizza()
        {
            var cutStrat = TestHelpers.SlicingPizza("2 2 1 4,TM,MM");
            
            Check.That(cutStrat.PointEarned).IsEqualTo(4);
            Check.That(cutStrat.ValidSlices).HasSize(1);
        }

        [Test]
        public void Find_Best_Way_To_Cut_A_3x3_Pizza()
        {
            var cutStrat = TestHelpers.SlicingPizza("3 3 1 4,TMM,MMM,MMT");
            
            Check.That(cutStrat.PointEarned).IsEqualTo(7);
            Check.That(cutStrat.ValidSlices).HasSize(2);
        }

        [Test]
        public void Find_Best_Way_To_Cut_Sample()
        {
            var cutStrat = TestHelpers.SlicingPizza("3 5 1 6,TTTTT,TMMMT,TTTTT");
            
            Check.That(cutStrat.PointEarned).IsEqualTo(15);
            Check.That(cutStrat.ValidSlices).HasSize(3);
        }

        [Test, Explicit]
        public void Find_Best_Way_To_Cut_Small()
        {
            var cutStrat = TestHelpers.SlicingPizza("6 7 1 5,TMMMTTT,MMMMTMM,TTMTTMT,TMMTMMM,TTTTTTM,TTTTTTM");
            
        }
    }
}