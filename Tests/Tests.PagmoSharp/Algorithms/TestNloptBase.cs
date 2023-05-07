using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public abstract class TestNloptBase : TestAlgorithmBase
{
    public abstract string NloptAlgorithmString { get; }

    public override IAlgorithm CreateAlgorithm()
    {
        var alg = new nlopt(NloptAlgorithmString);
        ConfigureNlopt(alg);
        return alg;
    }

    public virtual void ConfigureNlopt(nlopt alg)
    {
        // do nothing by default
    }

    [Test]
    public override void TestNameIsCorrect()
    {
        using var alg = CreateAlgorithm();
        Assert.AreEqual("NLopt - " + NloptAlgorithmString + ":", alg.get_name());
    }

    [Test]
    public override void TestBasicFunctions()
    {
        using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
        using (var algorithm = CreateAlgorithm(problem))
        {
            Assert.NotNull(algorithm.get_extra_info(), "getting non-null extra info");
            Assert.NotNull(algorithm.get_name(), "getting non-null name");
            algorithm.set_seed(2);
            Assert.AreEqual(0, algorithm.get_seed(), "getting set seed");
            Assert.AreEqual(0, algorithm.get_verbosity(), "getting original verbosity");
            algorithm.set_verbosity(2);
            Assert.AreEqual(0, algorithm.get_verbosity(), "getting set verbosity");
        }
    }

    [Test]
    [Explicit("A thorn in my side, going to stop fighting this, hopefully swig will fix the default constructor bug")]
    public void TestGettingAndSettingLocalOptimizer()
    {
        using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
        using (nlopt algorithm = (nlopt)CreateAlgorithm(problem))
        {
            Assert.IsNull(algorithm.get_local_optimizer());
            var alg2 = new nlopt();
            alg2.set_maxeval(1230); 
            algorithm.set_local_optimizer(alg2);
            Assert.AreEqual(1230, algorithm.get_local_optimizer().get_maxeval());
            algorithm.unset_local_optimizer();
            Assert.IsNull(algorithm.get_local_optimizer());
        }
    }

    public override bool SupportsGeneration
    {
        get { return false; }
    }

    public override bool Constrained
    {
        get { return false; }
    }
    public override bool Unconstrained
    {
        get { return true; }
    }
    public override bool SingleObjective
    {
        get { return true; }
    }
    public override bool MultiObjective
    {
        get { return false; }
    }
    public override bool IntegerPrograming
    {
        get { return false; }
    }
    public override bool Stochastic
    {
        get { return false; }
    }
}