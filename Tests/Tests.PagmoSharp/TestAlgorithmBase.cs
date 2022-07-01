using System;
using pagmo;
using Tests.PagmoSharp.TestProblems;
using Xunit;

namespace Tests.PagmoSharp
{
    public abstract class TestAlgorithmBase
    {
        public abstract IAlgorithm CreateAlgorithm(TestProblemWrapper testProblem);
        
        public abstract string Name { get; }
        
        public population EvolveAlgorithm(IAlgorithm algorithm, population population)
        {
            return algorithm.evolve(population);
        }

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

        [Fact]
        public void TestVerbosity()
        {
            using (var problem = new TwoDimensionalSingleCostProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.Equal<uint>(0, algorithm.get_verbosity());
                ((dynamic)algorithm).set_verbosity(2);
                Assert.Equal<uint>(2, algorithm.get_verbosity());
            }
        }
    }
}