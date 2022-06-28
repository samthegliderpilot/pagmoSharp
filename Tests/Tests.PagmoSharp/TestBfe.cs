using System.Linq;
using pagmo;
using Tests.PagmoSharp.TestProblems;
using Xunit;

namespace Tests.PagmoSharp
{
    public class TestBfe
    {
        [Fact]
        public void TestDefaultBfe()
        {
            using var bfeSample = new default_bfe();
            using var problem = new TwoDimensionalSingleCostProblemWrapper();
            using var pop = new population(problem, 4);
            var batchX = new DoubleVector(new[] { 1.2, 1.3, 1.1, 1.4 });
            var bfeResult = bfeSample.Operator(problem, batchX);
            Assert.Equal(2, bfeResult.Count);
            Assert.Equal(problem.fitness(1.2, 1.3)[0], bfeResult[0]);
            Assert.Equal(problem.fitness(1.1, 1.4)[0], bfeResult[1]);
            Assert.Equal("Default batch fitness evaluator", bfeSample.get_name());
        }

        [Fact]
        public void TestThreadBfe()
        {
            bool pass = false;
            int timesToTry = 16;
            int tryCount = 0;
            while (!pass && tryCount++ < timesToTry)
            {
                using var bfeSample = new pagmo.thread_bfe();
                using var problem = new TwoDimensionalSingleCostProblemWrapper();
                using (var pop = new population(problem, 64))
                {
                    var batchX = new DoubleVector(new[]
                    {
                        1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1,
                        1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4
                    });
                    var bfeResult = bfeSample.Operator(problem, batchX);
                    Assert.Equal(14, bfeResult.Count);
                    Assert.Equal(problem.fitness(1.2, 1.3)[0], bfeResult[0]);
                    Assert.Equal(problem.fitness(1.1, 1.4)[0], bfeResult[1]);
                    //Assert.Equal("Multi-threaded batch fitness evaluator", bfeSample.get_name());
                    Assert.NotEmpty(problem.ThreadsExecuted);
                    pass = problem.ThreadsExecuted.Distinct().Count() != 1;
                }
            }
            Assert.True(pass);
        }

        //[Fact]
        //public void TestMemberBfe()
        //{
        //    using (var bfeSample = new pagmo.member_bfe())
        //    using (var problem = new TwoDimensionalSingleCostProblemWrapper())
        //    using (var pop = new population(problem, 4))
        //    {
        //        var batchX = new DoubleVector(new[] { 1.2, 1.3, 1.1, 1.4 });
        //        var bfeResult = bfeSample.Operator(problem, batchX);
        //        Assert.Equal(2, bfeResult.Count);
        //        Assert.Equal(problem.fitness(1.2, 1.3)[0], bfeResult[0]);
        //        Assert.Equal(problem.fitness(1.1, 1.4)[0], bfeResult[1]);
        //        Assert.Equal("Default batch fitness evaluator", bfeSample.get_name());
        //        //Assert.Equal("", bfeSample.get_extra_info());
        //        //Assert.Equal(thread_safety.basic, bfeSample.get_thread_safety());
        //        //Assert.True(bfeSample.is_valid());
        //    }
        //}
    }
}