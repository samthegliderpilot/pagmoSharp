using System;
using System.Collections.Generic;
using System.Linq;
namespace pagmo;

//Copied, pasted and ported from the pagmo/utils/gradiant_and_hessian's type.

public class GradientsAndHessians
{
    public static List<Tuple<int, int>> EstimateSparsity(Func<DoubleVector, DoubleVector> f, DoubleVector x, double dx = 1e-8)
    {
        DoubleVector f0 = f(x);
        DoubleVector x_new = x;
        List<Tuple<int, int>> retval = new List<Tuple<int, int>>();
        // We change one by one each variable by dx and detect changes in the f call
        for (int j = 0; j<x.Count; ++j) {
            x_new[j] = x[j] + Math.Max(Math.Abs(x[j]), 1.0) * dx;
            var f_new = f(x_new);
            if (f_new.Count != f0.Count) {
                throw new Exception("Change in the size of the returned vector around the reference point. Cannot estimate a sparisty.");
            }
            for (int i = 0; i<f_new.Count; ++i) {
                if (f_new[i] != f0[i]) {
                    retval.Add(new Tuple<int, int>(i, j));
                }
            }
            x_new[j] = x[j];
        }
        // Restore the lexicographic order required by pagmo::problem::gradient_sparsity
        retval= retval.OrderBy(s => s.Item1).OrderBy(s => s.Item2).ToList(); //TODO: Faster?
        return retval;
    }

    public static DoubleVector estimate_gradient(Func<DoubleVector, DoubleVector> f, DoubleVector x, double dx = 1e-8)
    {
        DoubleVector f0 = f(x);
        DoubleVector gradient = new DoubleVector(f0.Count* x.Count);
        DoubleVector x_r = x, x_l = x;
        // We change one by one each variable by dx and estimate the derivative
        for (int j = 0; j<x.Count; ++j) {
            double h = Math.Max(Math.Abs(x[j]), 1.0) * dx;
            x_r[j] = x[j] + h;
            x_l[j] = x[j] - h;
            DoubleVector f_r = f(x_r);
            DoubleVector f_l = f(x_l);
            if (f_r.Count != f0.Count || f_l.Count != f0.Count) {
                throw new Exception("Change in the size of the returned vector around the reference point.Cannot compute a gradient");
            }
            for (int i = 0; i<f_r.Count; ++i) {
                gradient[j + i * x.Count] = (f_r[i] - f_l[i]) / 2.0 / h;
            }
            x_r[j] = x[j];
            x_l[j] = x[j];
        }
        return gradient;
    }

    public static DoubleVector estimate_gradient_h(Func<DoubleVector, DoubleVector> f, DoubleVector x, double dx = 1e-2)
    {
        DoubleVector f0 = f(x);
        DoubleVector gradient= new DoubleVector(f0.Count * x.Count);
        DoubleVector x_r1 = x, x_l1 = x;
        DoubleVector x_r2 = x, x_l2 = x;
        DoubleVector x_r3 = x, x_l3 = x;
        // We change one by one each variable by dx and estimate the derivative
        for (int j = 0; j<x.Count; ++j) {
            double h = Math.Max(Math.Abs(x[j]), 1.0) * dx;
            x_r1[j] = x[j] + h;
            x_l1[j] = x[j] - h;
            x_r2[j] = x[j] + 2.0 * h;
            x_l2[j] = x[j] - 2.0 * h;
            x_r3[j] = x[j] + 3.0 * h;
            x_l3[j] = x[j] - 3.0 * h;
            DoubleVector f_r1 = f(x_r1);
            DoubleVector f_l1 = f(x_l1);
            DoubleVector f_r2 = f(x_r2);
            DoubleVector f_l2 = f(x_l2);
            DoubleVector f_r3 = f(x_r3);
            DoubleVector f_l3 = f(x_l3);
            if (f_r1.Count != f0.Count || f_l1.Count != f0.Count || f_r2.Count != f0.Count || f_l2.Count != f0.Count
                || f_r3.Count != f0.Count || f_l3.Count != f0.Count)
            {
                throw new Exception(
                    "Change in the size of the returned vector detected around the reference point. Cannot compute a gradient");
            }

            for (int i = 0; i<f_r1.Count; ++i) {
                double m1 = (f_r1[i] - f_l1[i]) / 2.0;
                double m2 = (f_r2[i] - f_l2[i]) / 4.0;
                double m3 = (f_r3[i] - f_l3[i]) / 6.0;
                double fifteen_m1 = 15.0 * m1;
                double six_m2 = 6.0 * m2;
                double ten_h = 10.0 * h;
                gradient[j + i * x.Count] = ((fifteen_m1 - six_m2) + m3) / ten_h;
            }
            x_r1[j] = x[j];
            x_l1[j] = x[j];
            x_r2[j] = x[j];
            x_l2[j] = x[j];
            x_r3[j] = x[j];
            x_l3[j] = x[j];
        }
        return gradient;
    }
}