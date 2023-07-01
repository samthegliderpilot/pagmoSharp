using NUnit.Framework;
using pagmo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_unconnected
    {
        [Test]
        public void TestBasic() //TODO: Really understand topologies and retest
        {
            unconnected thing = new unconnected();
            Assert.IsNotNull(thing.get_connections(1));
            Assert.AreEqual("Unconnected", thing.get_name());
        }
    }
}