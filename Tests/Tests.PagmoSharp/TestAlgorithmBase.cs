using System;
using pagmo;
using Tests.PagmoSharp.TestProblems;
using Xunit;

namespace Tests.PagmoSharp
{
    public abstract class TestAlgorithmBase
    {
        public abstract IDisposable CreateAlgorithm(TestProblemWrapper testProblem);
        public abstract population EvolveAlgorithm(IDisposable algorithm, population population);

        [Fact]
        public void TestOneDimensionalProblem()
        {
            using (var problem = new OneDimensionalSimpleProblem())
            using (var algorithm = CreateAlgorithm(problem))
            using (var pop = new population(problem, 64))
            {
                var finalpop = EvolveAlgorithm(algorithm, pop);
                Assert.Equal(problem.ExpectedOptimalFunctionValue, finalpop.champion_f()[0], 2);
                Assert.Equal(problem.ExpectedOptimalX[0], finalpop.champion_x()[0], 2);
            }
        }

        [Fact]
        public void TestTwoDimensionalProblem()
        {
            using (var problem = new TwoDimensionalSingleCostProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            using (var pop = new population(problem, 128))
            {
                var finalpop = EvolveAlgorithm(algorithm, pop);
                Assert.Equal(problem.ExpectedOptimalFunctionValue, finalpop.champion_f()[0], 2);
                Assert.Equal(problem.ExpectedOptimalX[0], finalpop.champion_x()[0], 1);
            }
        }
    }
}