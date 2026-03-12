using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_archipelago
    {
        [Test]
        public void PushBackIslandRejectsManagedProblemWithThreadSafetyNone()
        {
            using var archi = new archipelago();
            using var algo = new bee_colony();
            using var problem = new OneDimensionalSimpleProblem();

            var ex = Assert.Throws<InvalidOperationException>(
                () => archi.push_back_island(algo.to_algorithm(), problem, 8, 2));
            Assert.That(ex!.Message, Does.Contain("thread_safety.basic or thread_safety.constant"));
        }

        [Test]
        public void ManagedThreadSafeProblemCanEvolveInArchipelago()
        {
            using var archi = new archipelago();
            using var algo = new bee_colony();
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();

            archi.push_back_island(algo.to_algorithm(), problem, 32, 2);
            Assert.AreEqual(1u, archi.size());

            archi.evolve(1);
            archi.wait_check();
            Assert.AreEqual(evolve_status.idle, archi.status());

            var db = archi.MigrantsDb;
            Assert.IsNotNull(db);
            var log = archi.MigrationLog;
            Assert.IsNotNull(log);
        }
    }
}
