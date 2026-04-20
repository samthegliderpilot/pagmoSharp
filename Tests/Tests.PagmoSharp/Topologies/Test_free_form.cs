using NUnit.Framework;
using pagmo;
using System;
namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_free_form
    {
        [Test]
        public void TestBasic()
        {
            using var topo = new free_form();
            topo.push_back();
            topo.push_back();
            topo.push_back();
            topo.add_edge(0u, 1u, 0.25);
            topo.add_edge(0u, 2u, 0.75);

            var connections = topo.GetConnectionsData(0u);

            Assert.AreEqual("Free form", topo.get_name());
            Assert.AreEqual(3u, topo.num_vertices());
            var extraInfo = topo.get_extra_info();
            Assert.That(extraInfo, Does.Contain("Number of vertices: 3"));
            Assert.That(extraInfo, Does.Contain("Number of edges: 2"));
            Assert.AreEqual(connections.NeighborIds.Length, connections.Weights.Length);
            Assert.AreEqual(0.25, topo.get_edge_weight(0u, 1u), 1e-12);
            Assert.AreEqual(0.75, topo.get_edge_weight(0u, 2u), 1e-12);

            topo.set_weight(0u, 1u, 0.5);
            Assert.AreEqual(0.5, topo.get_edge_weight(0u, 1u), 1e-12);

            topo.set_all_weights(0.9);
            Assert.AreEqual(0.9, topo.get_edge_weight(0u, 1u), 1e-12);
            Assert.AreEqual(0.9, topo.get_edge_weight(0u, 2u), 1e-12);

            topo.remove_edge(0u, 2u);
            var updatedExtraInfo = topo.get_extra_info();
            Assert.That(updatedExtraInfo, Does.Contain("Number of edges: 1"));
        }

        [Test]
        public void GetConnectionsDataThrowsOnNullFreeForm()
        {
            free_form nullTopology = null;
            Assert.Throws<ArgumentNullException>(() => TopologyConnectionExtensions.GetConnectionsData(nullTopology, 0u));
        }
    }
}
