using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_fully_connected
    {
        [Test]
        public void TestBasic()
        {
            using var topo = new fully_connected(3, 0.5);
            var connections = topo.get_connections(1);

            Assert.IsNotNull(connections);
            Assert.AreEqual("Fully connected", topo.get_name());
            Assert.GreaterOrEqual(topo.num_vertices(), 3u);
            Assert.AreEqual(0.5, topo.get_weight(), 1e-12);
        }
    }
}
