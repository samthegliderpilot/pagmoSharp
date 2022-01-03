using System;
using pagmo;
using Tests.PagmoSharp.TestProblems;
using Xunit;

namespace Tests.PagmoSharp
{
    public abstract class TestAlgorithmBase
    {
        public abstract IDisposable CreateAlgorithm();
        public abstract population EvolveAlgorithm(IDisposable algorithm, population population);

        [Fact]
        public void TestOneDimensionalProblem()
        {
            OneDimensionalSimpleProblem function = new OneDimensionalSimpleProblem();
            using (var problem = new pagmo.problem())
            {
                problem.RegisterFitnessCallback(function.EvaluateFitness);
                problem.SetBounds(new DoubleVector(function.LowerBounds), new DoubleVector(function.UpperBounds));
                using (var algorithm = CreateAlgorithm())
                using (var pop = new population(problem, 64))
                {
                    var finalpop = EvolveAlgorithm(algorithm, pop);
                    Assert.Equal(function.ExpectedOptimalFunctionValue, finalpop.champion_f()[0], 2);
                    Assert.Equal(function.ExpectedOptimalX[0], finalpop.champion_x()[0], 2);
                }
            }
        }
    }
}