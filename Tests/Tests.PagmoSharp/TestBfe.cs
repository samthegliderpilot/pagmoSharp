using System.Linq;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class TestBfe
    {
        [Test]
        public void TestDefaultBfe()
        {
            using var bfeSample = new default_bfe();
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var pop = new population(problem, 4);
            var batchX = new DoubleVector(new[] { 1.2, 1.3, 1.1, 1.4 });
            var bfeResult = bfeSample.Operator(problem, batchX);
            Assert.AreEqual(2, bfeResult.Count);
            Assert.AreEqual(problem.fitness(1.2, 1.3)[0], bfeResult[0]);
            Assert.AreEqual(problem.fitness(1.1, 1.4)[0], bfeResult[1]);
            Assert.AreEqual("Default batch fitness evaluator", bfeSample.get_name());
        }

        [Test]
        public void TestThreadBfe()
        {
            bool pass = false;
            int timesToTry = 16;
            int tryCount = 0;
            while (!pass && tryCount++ < timesToTry) // can't guarantee threading actually runs on multiple threads, so run multiple times
            {
                using var bfeSample = new pagmo.thread_bfe();
                using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
                using (var pop = new population(problem, 64))
                {
                    var batchX = new DoubleVector(new[]
                    {
                        1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1,
                        1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4
                    });
                    var bfeResult = bfeSample.Operator(problem, batchX);
                    Assert.AreEqual(14, bfeResult.Count);
                    Assert.AreEqual(problem.fitness(1.2, 1.3)[0], bfeResult[0]);
                    Assert.AreEqual(problem.fitness(1.1, 1.4)[0], bfeResult[1]);
                    //Assert.AreEqual("Multi-threaded batch fitness evaluator", bfeSample.get_name());
                    Assert.IsNotEmpty(problem.ThreadsExecuted);
                    pass = problem.ThreadsExecuted.Distinct().Count() != 1;
                }
            }
            Assert.True(pass);
        }

        //[Test]
        //public void TestMemberBfe()
        //{
        //    using (var bfeSample = new pagmo.member_bfe())
        //    using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
        //    using (var pop = new population(problem, 4))
        //    {
        //        var batchX = new DoubleVector(new[] { 1.2, 1.3, 1.1, 1.4 });
        //        var bfeResult = bfeSample.Operator(problem, batchX);
        //        Assert.AreEqual(2, bfeResult.Count);
        //        Assert.AreEqual(problem.fitness(1.2, 1.3)[0], bfeResult[0]);
        //        Assert.AreEqual(problem.fitness(1.1, 1.4)[0], bfeResult[1]);
        //        Assert.AreEqual("Member batch fitness evaluator", bfeSample.get_name());
        //        //Assert.AreEqual("", bfeSample.get_extra_info());
        //        //Assert.AreEqual(thread_safety.basic, bfeSample.get_thread_safety());
        //        //Assert.True(bfeSample.is_valid());
        //    }
        //}
    }
}