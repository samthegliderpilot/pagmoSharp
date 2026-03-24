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

        public static island Create(IAlgorithm a, IProblem p, ulong popSize, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return Create(normalized, p, popSize, seed);
        }

        public static island CreateWithThreadIsland(thread_island isl, algorithm a, IProblem p, ulong popSize, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithThreadIsland(isl, a, prob, checked((uint)popSize), seed ?? new random_device().next());
        }

        public static island CreateWithThreadIsland(thread_island isl, IAlgorithm a, IProblem p, ulong popSize, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIsland(isl, normalized, p, popSize, seed);
        }

        public static island CreateWithPolicies(algorithm a, IProblem p, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithPolicies(a, prob, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }

        public static island CreateWithPolicies(IAlgorithm a, IProblem p, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithPolicies(normalized, p, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndPolicies(thread_island isl, algorithm a, IProblem p, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithThreadIslandAndPolicies(isl, a, prob, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }

        public static island CreateWithThreadIslandAndPolicies(thread_island isl, IAlgorithm a, IProblem p, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndPolicies(isl, normalized, p, popSize, r, s, seed);
        }

        public static island CreateWithPolicies(algorithm a, IProblem p, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithPolicies(a, prob, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }

        public static island CreateWithPolicies(IAlgorithm a, IProblem p, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithPolicies(normalized, p, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndPolicies(thread_island isl, algorithm a, IProblem p, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithThreadIslandAndPolicies(isl, a, prob, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }

        public static island CreateWithThreadIslandAndPolicies(thread_island isl, IAlgorithm a, IProblem p, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndPolicies(isl, normalized, p, popSize, r, s, seed);
        }

        public static island CreateWithBfe(algorithm a, IProblem p, bfe b, ulong popSize, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithBfe(a, prob, b, checked((uint)popSize), seed ?? new random_device().next());
        }

        public static island CreateWithThreadIslandAndBfe(thread_island isl, algorithm a, IProblem p, bfe b, ulong popSize, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithThreadIslandAndBfe(isl, a, prob, b, checked((uint)popSize), seed ?? new random_device().next());
        }

        public static island CreateWithBfe(IAlgorithm a, IProblem p, bfe b, ulong popSize, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfe(normalized, p, b, popSize, seed);
        }

        public static island CreateWithThreadIslandAndBfe(thread_island isl, IAlgorithm a, IProblem p, bfe b, ulong popSize, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfe(isl, normalized, p, b, popSize, seed);
        }

        public static island CreateWithBfe(algorithm a, IProblem p, default_bfe b, ulong popSize, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithBfe(a, p, typeErasedBfe, popSize, seed);
        }

        public static island CreateWithThreadIslandAndBfe(thread_island isl, algorithm a, IProblem p, default_bfe b, ulong popSize, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithThreadIslandAndBfe(isl, a, p, typeErasedBfe, popSize, seed);
        }

        public static island CreateWithBfe(algorithm a, IProblem p, thread_bfe b, ulong popSize, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithBfe(a, p, typeErasedBfe, popSize, seed);
        }

        public static island CreateWithThreadIslandAndBfe(thread_island isl, algorithm a, IProblem p, thread_bfe b, ulong popSize, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithThreadIslandAndBfe(isl, a, p, typeErasedBfe, popSize, seed);
        }

        public static island CreateWithBfe(algorithm a, IProblem p, member_bfe b, ulong popSize, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithBfe(a, p, typeErasedBfe, popSize, seed);
        }

        public static island CreateWithThreadIslandAndBfe(thread_island isl, algorithm a, IProblem p, member_bfe b, ulong popSize, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithThreadIslandAndBfe(isl, a, p, typeErasedBfe, popSize, seed);
        }

        public static island CreateWithBfe(IAlgorithm a, IProblem p, default_bfe b, ulong popSize, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfe(normalized, p, b, popSize, seed);
        }

        public static island CreateWithThreadIslandAndBfe(thread_island isl, IAlgorithm a, IProblem p, default_bfe b, ulong popSize, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfe(isl, normalized, p, b, popSize, seed);
        }

        public static island CreateWithBfe(IAlgorithm a, IProblem p, thread_bfe b, ulong popSize, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfe(normalized, p, b, popSize, seed);
        }

        public static island CreateWithThreadIslandAndBfe(thread_island isl, IAlgorithm a, IProblem p, thread_bfe b, ulong popSize, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfe(isl, normalized, p, b, popSize, seed);
        }

        public static island CreateWithBfe(IAlgorithm a, IProblem p, member_bfe b, ulong popSize, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfe(normalized, p, b, popSize, seed);
        }

        public static island CreateWithThreadIslandAndBfe(thread_island isl, IAlgorithm a, IProblem p, member_bfe b, ulong popSize, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfe(isl, normalized, p, b, popSize, seed);
        }


        public static island CreateWithBfeAndPolicies(algorithm a, IProblem p, bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithBfeAndPolicies(a, prob, b, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, algorithm a, IProblem p, bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithThreadIslandAndBfeAndPolicies(isl, a, prob, b, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }

        public static island CreateWithBfeAndPolicies(IAlgorithm a, IProblem p, bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfeAndPolicies(normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, IAlgorithm a, IProblem p, bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfeAndPolicies(isl, normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(algorithm a, IProblem p, default_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithBfeAndPolicies(a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, algorithm a, IProblem p, default_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithThreadIslandAndBfeAndPolicies(isl, a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(algorithm a, IProblem p, thread_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithBfeAndPolicies(a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, algorithm a, IProblem p, thread_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithThreadIslandAndBfeAndPolicies(isl, a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(algorithm a, IProblem p, member_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithBfeAndPolicies(a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, algorithm a, IProblem p, member_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithThreadIslandAndBfeAndPolicies(isl, a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(IAlgorithm a, IProblem p, default_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfeAndPolicies(normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, IAlgorithm a, IProblem p, default_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfeAndPolicies(isl, normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(IAlgorithm a, IProblem p, thread_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfeAndPolicies(normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, IAlgorithm a, IProblem p, thread_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfeAndPolicies(isl, normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(IAlgorithm a, IProblem p, member_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfeAndPolicies(normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, IAlgorithm a, IProblem p, member_bfe b, ulong popSize, fair_replace r, select_best s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfeAndPolicies(isl, normalized, p, b, popSize, r, s, seed);
        }


        public static island CreateWithBfeAndPolicies(algorithm a, IProblem p, bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithBfeAndPolicies(a, prob, b, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, algorithm a, IProblem p, bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var prob = new problem(p);
            return island.CreateWithThreadIslandAndBfeAndPolicies(isl, a, prob, b, checked((uint)popSize), r, s, seed ?? new random_device().next());
        }

        public static island CreateWithBfeAndPolicies(IAlgorithm a, IProblem p, bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfeAndPolicies(normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, IAlgorithm a, IProblem p, bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfeAndPolicies(isl, normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(algorithm a, IProblem p, default_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithBfeAndPolicies(a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, algorithm a, IProblem p, default_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithThreadIslandAndBfeAndPolicies(isl, a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(algorithm a, IProblem p, thread_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithBfeAndPolicies(a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, algorithm a, IProblem p, thread_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithThreadIslandAndBfeAndPolicies(isl, a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(algorithm a, IProblem p, member_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithBfeAndPolicies(a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, algorithm a, IProblem p, member_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var typeErasedBfe = b.to_bfe();
            return CreateWithThreadIslandAndBfeAndPolicies(isl, a, p, typeErasedBfe, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(IAlgorithm a, IProblem p, default_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfeAndPolicies(normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, IAlgorithm a, IProblem p, default_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfeAndPolicies(isl, normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(IAlgorithm a, IProblem p, thread_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfeAndPolicies(normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, IAlgorithm a, IProblem p, thread_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfeAndPolicies(isl, normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithBfeAndPolicies(IAlgorithm a, IProblem p, member_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithBfeAndPolicies(normalized, p, b, popSize, r, s, seed);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island isl, IAlgorithm a, IProblem p, member_bfe b, ulong popSize, r_policy r, s_policy s, uint? seed = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(a);
            return CreateWithThreadIslandAndBfeAndPolicies(isl, normalized, p, b, popSize, r, s, seed);
        }

    }
}
