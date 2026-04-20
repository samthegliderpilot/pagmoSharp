using NUnit.Framework;
using pagmo;
using System;
using System.Linq;

namespace Tests.Pagmo.NET
{
    [TestFixture]
    public class Test_fully_connected
    {
        [Test]
        public void TestBasic()
        {
            using var topo = new fully_connected(3, 0.5);
            var connections = topo.GetConnectionsData(1u);

            Assert.AreEqual("Fully connected", topo.get_name());
            Assert.GreaterOrEqual(topo.num_vertices(), 3u);
            Assert.AreEqual(0.5, topo.get_weight(), 1e-12);
            Assert.AreEqual(connections.NeighborIds.Length, connections.Weights.Length);
            Assert.AreEqual(2, connections.NeighborIds.Length, "A 3-node fully connected topology should expose 2 neighbors per node.");
            Assert.IsFalse(connections.NeighborIds.Contains(1u), "A node should not be connected to itself.");
            Assert.IsTrue(connections.Weights.All(w => System.Math.Abs(w - 0.5) <= 1e-12), "All outgoing weights should match configured value.");
        }

        [Test]
        public void GetConnectionsDataThrowsOnNullFullyConnected()
        {
            fully_connected nullTopology = null;
            Assert.Throws<ArgumentNullException>(() => TopologyConnectionExtensions.GetConnectionsData(nullTopology, 0u));
        }
    }
}
