using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp
{
    //TODO: Needs a better test
    [TestFixture]
    public class Test_threadIsland
    {
        [Test]
        public void TestBasicMethods()
        {
            using thread_island island = new thread_island();
            Assert.AreEqual("Thread island", island.get_name(), "name");
            Assert.IsNotNull(island.get_extra_info(), "extra info");
        }
    }
}