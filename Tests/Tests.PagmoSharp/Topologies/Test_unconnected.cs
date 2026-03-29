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

            AssertNoOutgoingConnections(topology, 0u);
            AssertNoOutgoingConnections(topology, 1u);

            topology.push_back();
            topology.push_back();
            AssertNoOutgoingConnections(topology, 2u);
            AssertNoOutgoingConnections(topology, 3u);
        }
    }
}
