using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class TestNloptBobyqa : TestNloptBase
{
    public override string NloptAlgorithmString => "bobyqa";
}

[TestFixture]
public class TestNloptNewuoa : TestNloptBase
{
    public override string NloptAlgorithmString => "newuoa";
}

//[TestFixture]
//public class TestNloptPraxis : TestNloptBase
//{
//    public override string NloptAlgorithmString => "praxis";
//}

[TestFixture]
public class TestNloptNeldermead : TestNloptBase
{
    public override string NloptAlgorithmString => "neldermead";
}

[TestFixture]
public class TestNloptSbplx : TestNloptBase
{
    public override string NloptAlgorithmString => "sbplx";
}

[TestFixture]
public class TestNloptLbfgs : TestNloptBase
{
    public override string NloptAlgorithmString => "lbfgs";
}

[TestFixture]
public class TestNloptTnewton_precond_restart : TestNloptBase
{
    public override string NloptAlgorithmString => "tnewton_precond_restart";
}

[TestFixture]
public class TestNloptTnewton_precond : TestNloptBase
{
    public override string NloptAlgorithmString => "tnewton_precond";
}

[TestFixture]
public class TestNloptTnewton_restart : TestNloptBase
{
    public override string NloptAlgorithmString => "tnewton_restart";
}

[TestFixture]
public class TestNloptTnewton : TestNloptBase
{
    public override string NloptAlgorithmString => "tnewton";
}

[TestFixture]
public class TestNloptVar1 : TestNloptBase
{
    public override string NloptAlgorithmString => "var1";
}

[TestFixture]
public class TestNloptVar2 : TestNloptBase
{
    public override string NloptAlgorithmString => "var2";
}



[TestFixture]
public class TestNloptCobyla : TestNloptBase
{
    public override string NloptAlgorithmString => "cobyla";
    public override bool Constrained => false;
}

[TestFixture]
public class TestNloptMma : TestNloptBase
{
    public override string NloptAlgorithmString => "mma";
    public override bool Constrained => false;
}

[TestFixture]
public class TestNloptCcsaq : TestNloptBase
{
    public override string NloptAlgorithmString => "ccsaq";
    public override bool Constrained => false;
}

[TestFixture]
public class TestNloptSlsqp : TestNloptBase
{
    public override string NloptAlgorithmString => "slsqp";
    public override bool Constrained => false;
}