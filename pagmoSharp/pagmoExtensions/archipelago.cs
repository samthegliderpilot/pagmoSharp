using System;
using System.Collections.Generic;

namespace pagmo
{
    public partial class archipelago
    {
        private static ulong WithManagedProblem(IProblem managedProblem, Func<problem, ulong> action)
        {
            managedProblem.ThrowIfNotThreadSafe();
            using var wrappedProblem = new problem(managedProblem);
            return action(wrappedProblem);
        }

        public IReadOnlyList<MigrationEntry> MigrationLog
        {
            get
            {
                using var native = get_migration_log_entries();
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
                using var native = get_migrants_db();
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
            return get_island_copy(SizeTInterop.ToNativeUInt32(index, nameof(index)));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, ulong popSize, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), seed));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), r, s, seed));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return push_back_island(algo, prob, popSize, wrappedReplacementPolicy, wrappedSelectionPolicy, seed);
        }

        public ulong push_back_island(algorithm algo, IProblem prob, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), replacementPolicy, selectionPolicy, seed));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, bfe b, ulong popSize, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, b, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), seed));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, b, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), r, s, seed));
        }

        public ulong push_back_island(algorithm algo, IProblem prob, bfe b, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return push_back_island(algo, prob, b, popSize, wrappedReplacementPolicy, wrappedSelectionPolicy, seed);
        }

        public ulong push_back_island(algorithm algo, IProblem prob, bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(algo, wrappedProblem, b, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), replacementPolicy, selectionPolicy, seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, ulong popSize, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), r, s, seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return push_back_island(isl, algo, prob, popSize, wrappedReplacementPolicy, wrappedSelectionPolicy, seed);
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), replacementPolicy, selectionPolicy, seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, bfe b, ulong popSize, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, b, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, b, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), r, s, seed));
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, bfe b, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return push_back_island(isl, algo, prob, b, popSize, wrappedReplacementPolicy, wrappedSelectionPolicy, seed);
        }

        public ulong push_back_island(thread_island isl, algorithm algo, IProblem prob, bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            return WithManagedProblem(prob, wrappedProblem => push_back_island(isl, algo, wrappedProblem, b, SizeTInterop.ToNativeUInt32(popSize, nameof(popSize)), replacementPolicy, selectionPolicy, seed));
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

        public ulong push_back_island(IAlgorithm algo, IProblem prob, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(normalized, prob, popSize, replacementPolicy, selectionPolicy, seed);
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

        public ulong push_back_island(IAlgorithm algo, IProblem prob, bfe b, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(normalized, prob, b, popSize, replacementPolicy, selectionPolicy, seed);
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

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(isl, normalized, prob, popSize, replacementPolicy, selectionPolicy, seed);
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

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, bfe b, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algo);
            return push_back_island(isl, normalized, prob, b, popSize, replacementPolicy, selectionPolicy, seed);
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

        public ulong push_back_island(IAlgorithm algo, IProblem prob, default_bfe b, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
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

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, default_bfe b, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, thread_bfe b, ulong popSize, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, thread_bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, thread_bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, r, s, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, thread_bfe b, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, thread_bfe b, ulong popSize, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, thread_bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, thread_bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, r, s, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, thread_bfe b, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, member_bfe b, ulong popSize, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, member_bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, member_bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, r, s, seed);
        }

        public ulong push_back_island(IAlgorithm algo, IProblem prob, member_bfe b, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, member_bfe b, ulong popSize, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, member_bfe b, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, member_bfe b, ulong popSize, r_policy r, s_policy s, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, r, s, seed);
        }

        public ulong push_back_island(thread_island isl, IAlgorithm algo, IProblem prob, member_bfe b, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            using var typeErasedBfe = b.to_bfe();
            return push_back_island(isl, algo, prob, typeErasedBfe, popSize, replacementPolicy, selectionPolicy, seed);
        }

        public island GetIsland(ulong index)
        {
            return GetIslandCopy(index);
        }

        public ulong PushBackIsland(algorithm algorithm, IProblem problem, ulong populationSize, uint seed)
        {
            return push_back_island(algorithm, problem, populationSize, seed);
        }

        public ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, ulong populationSize, uint seed)
        {
            return push_back_island(algorithm, problem, populationSize, seed);
        }

        public ulong PushBackIsland(algorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, uint seed)
        {
            return push_back_island(algorithm, problem, evaluator, populationSize, seed);
        }

        public ulong PushBackIsland(algorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            return push_back_island(algorithm, problem, evaluator, populationSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong PushBackIsland(algorithm algorithm, IProblem problem, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            return push_back_island(algorithm, problem, populationSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, uint seed)
        {
            return push_back_island(algorithm, problem, evaluator, populationSize, seed);
        }

        public ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            return push_back_island(algorithm, problem, evaluator, populationSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            return push_back_island(algorithm, problem, populationSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong PushBackIsland(thread_island islandType, algorithm algorithm, IProblem problem, ulong populationSize, uint seed)
        {
            return push_back_island(islandType, algorithm, problem, populationSize, seed);
        }

        public ulong PushBackIsland(thread_island islandType, algorithm algorithm, IProblem problem, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            return push_back_island(islandType, algorithm, problem, populationSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong PushBackIsland(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            return push_back_island(islandType, algorithm, problem, evaluator, populationSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong PushBackIsland(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong populationSize, uint seed)
        {
            return push_back_island(islandType, algorithm, problem, populationSize, seed);
        }

        public ulong PushBackIsland(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            return push_back_island(islandType, algorithm, problem, populationSize, replacementPolicy, selectionPolicy, seed);
        }

        public ulong PushBackIsland(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed)
        {
            return push_back_island(islandType, algorithm, problem, evaluator, populationSize, replacementPolicy, selectionPolicy, seed);
        }
    }
}

