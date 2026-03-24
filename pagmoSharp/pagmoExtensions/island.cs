using pagmo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pagmo
{
    public partial class island
    {
        public static island Create(algorithm a, IProblem p, ulong popSize, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.Create(a, prob, checked((uint)popSize), seed ?? new random_device().next());
        }

        public static island CreateWithPolicies(algorithm a, IProblem p, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithPolicies(a, prob, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }

        public static island CreateWithPolicies(algorithm a, IProblem p, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithPolicies(a, prob, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }

        public static island CreateWithBfe(algorithm a, IProblem p, bfe b, ulong popSize, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithBfe(a, prob, b, checked((uint)popSize), seed ?? new random_device().next());
        }

        public static island CreateWithBfeAndPolicies(algorithm a, IProblem p, bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithBfeAndPolicies(a, prob, b, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }

        public static island CreateWithBfeAndPolicies(algorithm a, IProblem p, bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithBfeAndPolicies(a, prob, b, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }
    }
}
