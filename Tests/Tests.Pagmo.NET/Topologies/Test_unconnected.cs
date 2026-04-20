using System;
using System.Linq;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_unconnected
    {
        private static void AssertNoOutgoingConnections(unconnected topology, uint vertexId)
        {
            var connections = topology.GetConnectionsData(vertexId);
            Assert.AreEqual(
                connections.NeighborIds.Length,
                connections.Weights.Length,
                $"Neighbor/weight shape mismatch for vertex {vertexId}.");
            Assert.AreEqual(0, connections.NeighborIds.Length, $"Vertex {vertexId} should have no outgoing neighbors.");
            Assert.AreEqual(0, connections.Weights.Length, $"Vertex {vertexId} should have no outgoing weights.");
        }

        [Test]
        public void TestBasic()
        {
            using var topology = new unconnected();
            Assert.AreEqual("Unconnected", topology.get_name());

            topology.push_back();
            topology.push_back();

            var checkedVertices = new[] { 0u, 1u, 2u, 3u };
            foreach (var vertexId in checkedVertices)
            {
                AssertNoOutgoingConnections(topology, vertexId);

                using var rawConnections = topology.get_connections(vertexId);
                var projectedConnections = topology.GetConnectionsData(vertexId);
                CollectionAssert.AreEqual(rawConnections.first.ToArray(), projectedConnections.NeighborIds);
                CollectionAssert.AreEqual(rawConnections.second.ToArray(), projectedConnections.Weights);
            }
        }

        [Test]
        public void GetConnectionsDataThrowsOnNullTopology()
        {
            unconnected nullTopology = null;
            Assert.Throws<ArgumentNullException>(() => TopologyConnectionExtensions.GetConnectionsData(nullTopology, 0u));
        }
    }
}
