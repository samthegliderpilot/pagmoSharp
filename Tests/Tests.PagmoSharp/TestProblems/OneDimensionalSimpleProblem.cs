using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.PagmoSharp.TestProblems
{
    public class OneDimensionalSimpleProblem
    {
        private double[] _lowerBounds = new[] {-10.0};
        private double[] _upperBounds = new[] {10.0};

        public double[] EvaluateFitness(double[] entireState)
        {
            return new[] {EvaluateFitness(entireState[0])};
        }

        public double EvaluateFitness(double x)
        {
            return x * x;
        }

        public void EvaluateFitness(double[] entireState, double[] answer, int sizeOfState, int sizeOfAnswer)
        {
            answer[0] = EvaluateFitness(entireState[0]);
        }

        public double[] LowerBounds
        {
            get { return _lowerBounds; }
        }

        public double[] UpperBounds
        {
            get { return _upperBounds; }
        }

        public double ExpectedOptimalFunctionValue
        {
            get { return 0.0; }
        }

        public double[] ExpectedOptimalX
        {
            get { return new double[] {0.0}; }
        }
    }
}
