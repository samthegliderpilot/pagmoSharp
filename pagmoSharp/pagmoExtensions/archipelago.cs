using System;
using System.Collections.Generic;

namespace pagmo
{
    public partial class archipelago
    {
        private static ulong WithManagedProblem(IProblem managedProblem, Func<problem, ulong> action)
        {
            managedProblem.ThrowIfNotThreadSafe();
            var problemPtr = NativeInterop.CreateProblemPointer(managedProblem);
            try
            {
                using var wrappedProblem = new problem(problemPtr, false);
                return action(wrappedProblem);
            }
            finally
            {
                NativeInterop.problem_delete(problemPtr);
            }
        }

        public IReadOnlyList<MigrationEntry> MigrationLog
        {
            get
            {
                var native = get_migration_log_entries();
                var list = new List<MigrationEntry>(native.Count);
                for (int i = 0; i < native.Count; ++i)
                {
                    list.Add(native[i]);
                }

                return list;
            }
        }

        public IReadOnlyList<IndividualsGroup> MigrantsDb
        {
            get
            {
                var native = get_migrants_db();
                var list = new List<IndividualsGroup>(native.Count);
                for (int i = 0; i < native.Count; ++i)
                {
                    list.Add(native[i]);
                }

                return list;
            }
        }

        public island GetIslandCopy(ulong index)
        {
            return get_island_copy(checked((uint)index));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, ulong popSize, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, checked((uint)popSize), seed));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, checked((uint)popSize), r, s, seed));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, checked((uint)popSize), replacementPolicy, selectionPolicy, seed));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, bfe b, ulong popSize, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, b, checked((uint)popSize), seed));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, b, checked((uint)popSize), r, s, seed));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, b, checked((uint)popSize), replacementPolicy, selectionPolicy, seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, ulong popSize, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, checked((uint)popSize), seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, checked((uint)popSize), r, s, seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, checked((uint)popSize), replacementPolicy, selectionPolicy, seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, bfe b, ulong popSize, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, b, checked((uint)popSize), seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, b, checked((uint)popSize), r, s, seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, b, checked((uint)popSize), replacementPolicy, selectionPolicy, seed));
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, ulong popSize, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(normalized, prob, popSize, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(normalized, prob, popSize, r, s, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(normalized, prob, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, bfe b, ulong popSize, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(normalized, prob, b, popSize, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(normalized, prob, b, popSize, r, s, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(normalized, prob, b, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, ulong popSize, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(isl, normalized, prob, popSize, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(isl, normalized, prob, popSize, r, s, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(isl, normalized, prob, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, bfe b, ulong popSize, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(isl, normalized, prob, b, popSize, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(isl, normalized, prob, b, popSize, r, s, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(isl, normalized, prob, b, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, default_bfe b, ulong popSize, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, default_bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, default_bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, r, s, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, default_bfe b, ulong popSize, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, default_bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, default_bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, r, s, seed);
        }
    }
}
