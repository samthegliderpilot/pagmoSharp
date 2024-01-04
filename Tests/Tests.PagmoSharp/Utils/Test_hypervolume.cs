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
    public class Test_hypervolume
    {
        [Test]
        [Explicit("I don't know how this works")]
        public void TestSomething() //TODO: Obviously
        {
            hypervolume hyper = new hypervolume(new VectorOfVectorOfDoubles(new []{new DoubleVector(3, 4), new DoubleVector(3, -4), new DoubleVector(-3, -4) , new DoubleVector(-3, 4) }));
            Assert.AreEqual(0, hyper.compute(new DoubleVector(3.5, 3.5)));
        }
    }
}
