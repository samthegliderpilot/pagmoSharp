using System.Collections.Generic;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms
{
    [TestFixture]
    public class Test_null_algorithm
    {
        [Test]
        public void EvolveLeavesPopulationUnchanged()
        {
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algorithm = new null_algorithm();
            using var initialPopulation = new population(problem, 20, 42);

            var xBefore = Snapshot(initialPopulation.get_x());
            var fBefore = Snapshot(initialPopulation.get_f());

            using var evolvedPopulation = algorithm.evolve(initialPopulation);
            var xAfter = Snapshot(evolvedPopulation.get_x());
            var fAfter = Snapshot(evolvedPopulation.get_f());

            CollectionAssert.AreEqual(xBefore, xAfter, "Decision vectors should remain unchanged.");
            CollectionAssert.AreEqual(fBefore, fAfter, "Fitness vectors should remain unchanged.");
        }

        [Test]
        public void BasicMetadataAndInterfaceMethodsWork()
        {
            using var algorithm = new null_algorithm();

            Assert.AreEqual("Null algorithm", algorithm.get_name());
            Assert.IsNotEmpty(algorithm.get_extra_info());

            algorithm.set_seed(11);
            algorithm.set_verbosity(3);

            Assert.AreEqual(11u, algorithm.get_seed());
            Assert.AreEqual(3u, algorithm.get_verbosity());
        }

        private static List<List<double>> Snapshot(VectorOfVectorOfDoubles data)
        {
            var snapshot = new List<List<double>>(data.Count);
            for (var row = 0; row < data.Count; row++)
            {
                var values = data[row];
                var rowSnapshot = new List<double>(values.Count);
                for (var col = 0; col < values.Count; col++)
                {
                    rowSnapshot.Add(values[col]);
                }

                snapshot.Add(rowSnapshot);
            }

            return snapshot;
        }
    }
}
