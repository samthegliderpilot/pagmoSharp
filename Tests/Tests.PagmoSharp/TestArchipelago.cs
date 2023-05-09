using NUnit.Framework;
using pagmo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class TestArchipelago
    {
        [Test]
        public void TestSomething()
        {
            using var bfeSample = new default_bfe();
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var pop = new population(problem, 4);

            archipelago archi = new archipelago();
            archi.evolve(5);
        }
    }
}
