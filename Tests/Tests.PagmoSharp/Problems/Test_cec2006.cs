﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems
{
    [TestFixture]
    public class Test_cec2006 : TestProblemBase
    {
        public override IProblem CreateStandardProblem(uint problemIndex = 0)
        {
            if (problemIndex == 0)
                problemIndex = 1;
            return new cec2006(problemIndex);
        }

        public override IEnumerable<uint> MultiProblemIndices {
            get
            {
                for (uint i = 1; i <= 24; i++)
                {
                    yield return i;
                }
            }
        }

        [Test]
        public override void TestBoilerPlate()
        {
            using var problem = CreateStandardProblem(1);
            Assert.AreEqual("CEC2006 - g1", problem.get_name(), "name");
            Assert.AreEqual(0, problem.get_nec(), "equality constraint count");
            Assert.AreEqual(1, problem.get_nobj(), "objective count");
            Assert.AreEqual(0, problem.get_nix(), "integer count");
            var bounds = problem.get_bounds();
            Assert.AreEqual(0.0, bounds.first[0]);
            Assert.IsFalse(problem.has_batch_fitness(), "has batch fitness");
        }

        [Test]
        public override void TestOptimizing()
        {
            using var problemBase = CreateStandardProblem();
            Assert.AreEqual(0, problemBase.get_bounds().first[0]);
            var problemBase2 = new ProblemWrapper(problemBase);
            using var algorithm = new gaco(20);
            using (var pop = new population(problemBase2, 1024))
            {
                algorithm.set_seed(2); // for consistent results
                
                var finalpop = algorithm.evolve(pop);
                var champX = finalpop.champion_x().ToArray();
                var champF = finalpop.champion_f().ToArray();
                Assert.AreEqual(13, champX.Length, "2 in x");
                Assert.AreEqual(0.991d, champX[0], 1.0, "1.0 for first x value");
                Assert.AreEqual(0.99708444960275089, champX[1], 1.0, "1.0 for second x value");
                // this function is hard to optimize (that's the point), are we anywhere close?
                Assert.AreEqual(10, champF.Length, "1 in f(x)");
                Assert.AreEqual(-12.45437350584859d, champF[0], 2.0, "optimal function value");

            }
        }

        public static IEnumerable<ProblemTestData> RegressionData
        {
            get
            {
                return new List<ProblemTestData>()
                {
                    new ProblemTestData("cec2006", "simpleTest1",
                        new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
                        new double[] { -181, 17, 20, 23, 2, -5, -12, -3, -8, -13 }, 1),
                    new ProblemTestData("cec2006", "simpleTest2",
                        new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, },
                        new double[] { -0.03410429993861006, -2.43290200817664E+18, 60, }, 2),

                    //new ProblemTestData("cec2006", "simpleTest3",
                    //    new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, },
                    //    new double[] { -0.03410429993861006, -2.43290200817664E+18, 60, }, 3),
                };
            }
        }
    }
}
