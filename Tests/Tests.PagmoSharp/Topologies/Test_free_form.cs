using NUnit.Framework;
using pagmo;

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
            Assert.AreEqual("Free form", topo.get_name());
        }
    }
}
