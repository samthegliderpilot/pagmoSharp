using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_ring
    {
        [Test]
        public void TestBasic()
        {
            using var topo = new ring(4, 0.8);
            Assert.AreEqual("Ring", topo.get_name());
            Assert.AreEqual(0.8, topo.get_weight(), 1e-12);
            Assert.AreEqual(4u, topo.num_vertices());
            Assert.IsNotNull(topo.get_connections(0));
        }
    }
}
