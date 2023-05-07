using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pagmo;

namespace Tests.PagmoSharp.Utils
{
    [TestFixture]
    public class TestHypervolume
    {
        [Test]
        public void TestSomething() //TODO: Obviously
        {
            hypervolume hyper = new hypervolume();
            Assert.AreEqual(0, hyper.compute(new DoubleVector(1,2)));
        }
    }
}
