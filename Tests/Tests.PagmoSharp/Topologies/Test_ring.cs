using NUnit.Framework;
using pagmo;
using System.Linq;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_ring
    {
        [Test]
        public void TestBasic()
        {
            using var topo = new ring(4, 0.8);
            var connections = topo.GetConnectionsData(0u);
            Assert.AreEqual("Ring", topo.get_name());
            Assert.AreEqual(0.8, topo.get_weight(), 1e-12);
            Assert.AreEqual(4u, topo.num_vertices());
            Assert.AreEqual(connections.NeighborIds.Length, connections.Weights.Length);
            Assert.Greater(connections.NeighborIds.Length, 0, "Ring nodes should have at least one outgoing connection.");
            Assert.IsFalse(connections.NeighborIds.Contains(0u), "A node should not be connected to itself.");
            Assert.IsTrue(connections.Weights.All(w => System.Math.Abs(w - 0.8) <= 1e-12), "All outgoing weights should match configured value.");
        }
    }
}
