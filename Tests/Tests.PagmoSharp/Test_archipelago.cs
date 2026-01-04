using NUnit.Framework;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_archipelago
    {
        [Test]
        public void TestSomething()
        {
            using var bfeSample = new default_bfe();
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var pop = new population(problem, 4);
            archipelago archi = new archipelago();
            using var bees = new bee_colony();
            archi.push_back_island(bees.to_algorithm(), problem, 4, 1);
            archi.evolve(5);

            //var islands = archi.get_island(0);
            //Assert.IsNotNull(islands);

            //var db = archi.MigrantsDb;
            //Assert.IsNotNull(db);

            //var log = archi.MigrationLog;
            //Assert.IsNotNull(log);
        }
    }
}
