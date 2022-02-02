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
            using var bfeSample = new pagmo.bfe();
            using var problem = new TwoDimensionalSingleCostProblemWrapper();
            using var pop = new population(problem, 4);
            var batchX = new DoubleVector(new[] { 1.2, 1.3, 1.1, 1.4 });
            var bfeResult = bfeSample.Operator(problem, batchX);
            Assert.Equal(2, bfeResult.Count);
            Assert.Equal(problem.fitness(1.2, 1.3)[0], bfeResult[0]);
            Assert.Equal(problem.fitness(1.1, 1.4)[0], bfeResult[1]);
            Assert.Equal("Default batch fitness evaluator", bfeSample.get_name());
            Assert.Equal("", bfeSample.get_extra_info());
            Assert.Equal(thread_safety.basic, bfeSample.get_thread_safety());
            Assert.True(bfeSample.is_valid());
        }

        //[Fact]
        //public void TestThreadBfe()
        //{
        //    using (var bfeSample = new pagmo.thread_bfe())
        //    using (var problem = new TwoDimensionalSingleCostProblemWrapper())
        //    using (var pop = new population(problem, 4))
        //    {
        //        var batchX = new DoubleVector(new[] { 1.2, 1.3, 1.1, 1.4 });
        //        var bfeResult = bfeSample.Operator(problem, batchX);
        //        Assert.Equal(2, bfeResult.Count);
        //        Assert.Equal(problem.fitness(1.2, 1.3)[0], bfeResult[0]);
        //        Assert.Equal(problem.fitness(1.1, 1.4)[0], bfeResult[1]);
        //        Assert.Equal("Default batch fitness evaluator", bfeSample.get_name());
        //        Assert.Equal("", bfeSample.get_extra_info());
        //        Assert.Equal(thread_safety.basic, bfeSample.get_thread_safety());
        //        Assert.True(bfeSample.is_valid());
        //    }
        //}

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
        //        Assert.Equal("", bfeSample.get_extra_info());
        //        Assert.Equal(thread_safety.basic, bfeSample.get_thread_safety());
        //        Assert.True(bfeSample.is_valid());
        //    }
        //}
    }
}