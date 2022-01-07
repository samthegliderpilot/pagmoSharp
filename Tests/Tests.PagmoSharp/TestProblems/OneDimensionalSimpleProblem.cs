using pagmo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.PagmoSharp.TestProblems
{
    public class OneDimensionalSimpleProblem : pagmo.problemBase
    {
        private double[] _lowerBounds = new[] {-10.0};
        private double[] _upperBounds = new[] {10.0};

        public static OneDimensionalSimpleProblem CreateProblem()
        {
            var problemWrap = new problem();
            var oneDProblem = new OneDimensionalSimpleProblem(problemWrap);
            problemWrap.setBaseProblem(oneDProblem);
            return oneDProblem;
        }

        public pagmo.problem TheProblem;

        private OneDimensionalSimpleProblem(pagmo.problem prob)
        {
            TheProblem = prob;}

        public override PairOfDoubleVectors get_bounds()
        {
            return new PairOfDoubleVectors(new DoubleVector(_lowerBounds), new DoubleVector(_upperBounds));
        }

        public override DoubleVector fitness(DoubleVector arg0)
        {
            return new DoubleVector(new[] { arg0[0]*arg0[0]});
        }

        public double ExpectedOptimalFunctionValue { get { return 0.0; } }
        public double[] ExpectedOptimalX { get { return new double[] { 0.0 }; } }

    }
}
