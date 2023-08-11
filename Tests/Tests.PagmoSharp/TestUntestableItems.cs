global using pagmo;
global using sparcity_pattern = pagmo.SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t;
global using individuals_group_t = pagmo.SWIGTYPE_p_std__tupleT_std__vectorT_unsigned_long_long_t_std__vectorT_std__vectorT_double_t_t_std__vectorT_std__vectorT_double_t_t_t;


using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pagmo;
using Tests.PagmoSharp.Problems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class TestUntestableItems
    {
        [Test]
        public void TestTopology()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void TestExceptions()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void TestSomething()
        {
            //sparcity_pattern
            //var cecTest = new Testcec2006();
            //var problem = new cec2006(1);
            //pagmo.pagmo.estimate_gradient(new SWIGTYPE_p_fn_ptr(problem.), new DoubleVector(Testcec2006.RegressionData.First().X));

            pagmo.fair_replace replace = new fair_replace();
            Assert.IsNotNull(replace.get_extra_info());

            replace.replace(null, 1, 2, 3, 4, 5, new DoubleVector(new[] { 1.0, 2.0, 3.0, 4.0, 5.0 }), null);
        }
    }
}
