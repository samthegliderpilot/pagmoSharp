using System;
using System.Collections.Generic;

namespace pagmo
{
    public partial class archipelago
    {
        private static ulong WithManagedProblem(IProblem problem, Func<problem, ulong> action)
        {
            problem.ThrowIfNotThreadSafe();
            using var wrappedProblem = new problem(problem);
            return action(wrappedProblem);
        }

        private static uint ToNativePopulationSize(ulong populationSize)
        {
            return SizeTInterop.ToNativeUInt32(populationSize, "popSize");
        }

        private static void ValidatePolicyPair(object replacementPolicy, object selectionPolicy, string replacementPolicyName, string selectionPolicyName)
        {
            var replacementProvided = replacementPolicy != null;
            var selectionProvided = selectionPolicy != null;
            if (replacementProvided == selectionProvided)
            {
                return;
            }

            throw new ArgumentException($"Both {replacementPolicyName} and {selectionPolicyName} must be provided together.");
        }

        private ulong PushBackCore(
            algorithm algorithm,
            IProblem problem,
            ulong populationSize,
            uint seed,
            bfe evaluator = null,
            thread_island islandType = null,
            fair_replace replacementPolicy = null,
            select_best selectionPolicy = null,
            r_policy wrappedReplacementPolicy = null,
            s_policy wrappedSelectionPolicy = null)
        {
            var nativePopulationSize = ToNativePopulationSize(populationSize);

            return WithManagedProblem(problem, wrappedProblem =>
            {
                var hasFairPolicies = replacementPolicy != null;
                var hasWrappedPolicies = wrappedReplacementPolicy != null;

                if (!hasFairPolicies && !hasWrappedPolicies)
                {
                    if (islandType == null)
                    {
                        return evaluator == null
                            ? push_back_island(algorithm, wrappedProblem, nativePopulationSize, seed)
                            : push_back_island(algorithm, wrappedProblem, evaluator, nativePopulationSize, seed);
                    }

                    return evaluator == null
                        ? push_back_island(islandType, algorithm, wrappedProblem, nativePopulationSize, seed)
                        : push_back_island(islandType, algorithm, wrappedProblem, evaluator, nativePopulationSize, seed);
                }

                if (hasFairPolicies)
                {
                    if (islandType == null)
                    {
                        return evaluator == null
                            ? push_back_island(algorithm, wrappedProblem, nativePopulationSize, replacementPolicy, selectionPolicy, seed)
                            : push_back_island(algorithm, wrappedProblem, evaluator, nativePopulationSize, replacementPolicy, selectionPolicy, seed);
                    }

                    return evaluator == null
                        ? push_back_island(islandType, algorithm, wrappedProblem, nativePopulationSize, replacementPolicy, selectionPolicy, seed)
                        : push_back_island(islandType, algorithm, wrappedProblem, evaluator, nativePopulationSize, replacementPolicy, selectionPolicy, seed);
                }

                if (islandType == null)
                {
                    return evaluator == null
                        ? push_back_island(algorithm, wrappedProblem, nativePopulationSize, wrappedReplacementPolicy, wrappedSelectionPolicy, seed)
                        : push_back_island(algorithm, wrappedProblem, evaluator, nativePopulationSize, wrappedReplacementPolicy, wrappedSelectionPolicy, seed);
                }

                return evaluator == null
                    ? push_back_island(islandType, algorithm, wrappedProblem, nativePopulationSize, wrappedReplacementPolicy, wrappedSelectionPolicy, seed)
                    : push_back_island(islandType, algorithm, wrappedProblem, evaluator, nativePopulationSize, wrappedReplacementPolicy, wrappedSelectionPolicy, seed);
            });
        }

        private ulong PushBackWithManagedPolicies(
            algorithm algorithm,
            IProblem problem,
            ulong populationSize,
            uint seed,
            r_policyBase replacementPolicy,
            s_policyBase selectionPolicy,
            bfe evaluator = null,
            thread_island islandType = null)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return PushBackCore(
                algorithm,
                problem,
                populationSize,
                seed,
                evaluator,
                islandType,
                wrappedReplacementPolicy: wrappedReplacementPolicy,
                wrappedSelectionPolicy: wrappedSelectionPolicy);
        }

        private ulong PushBackNormalizedAlgorithm(
            IAlgorithm algorithm,
            IProblem problem,
            ulong populationSize,
            uint seed,
            bfe evaluator = null,
            thread_island islandType = null,
            fair_replace replacementPolicy = null,
            select_best selectionPolicy = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algorithm);
            return PushBackCore(normalized, problem, populationSize, seed, evaluator, islandType, replacementPolicy, selectionPolicy);
        }

        public IReadOnlyList<MigrationEntry> MigrationLog
        {
            get
            {
                using var native = get_migration_log_entries();
                var list = new List<MigrationEntry>(native.Count);
                for (var i = 0; i < native.Count; ++i)
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
                for (var i = 0; i < native.Count; ++i)
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

        public ulong push_back_island(algorithm algorithm, IProblem problem, ulong populationSize, uint seed = 0u, thread_island islandType = null)
        {
            return PushBackCore(algorithm, problem, populationSize, seed, islandType: islandType);
        }

        public ulong push_back_island(algorithm algorithm, IProblem problem, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed = 0u, thread_island islandType = null)
        {
            ValidatePolicyPair(replacementPolicy, selectionPolicy, nameof(replacementPolicy), nameof(selectionPolicy));
            return PushBackWithManagedPolicies(algorithm, problem, populationSize, seed, replacementPolicy, selectionPolicy, islandType: islandType);
        }

        public ulong push_back_island(algorithm algorithm, IProblem problem, ulong populationSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed = 0u, thread_island islandType = null)
        {
            ValidatePolicyPair(replacementPolicy, selectionPolicy, nameof(replacementPolicy), nameof(selectionPolicy));
            return PushBackCore(algorithm, problem, populationSize, seed, islandType: islandType, replacementPolicy: replacementPolicy, selectionPolicy: selectionPolicy);
        }

        public ulong push_back_island(algorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, uint seed = 0u, thread_island islandType = null)
        {
            return PushBackCore(algorithm, problem, populationSize, seed, evaluator, islandType);
        }

        public ulong push_back_island(algorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed = 0u, thread_island islandType = null)
        {
            ValidatePolicyPair(replacementPolicy, selectionPolicy, nameof(replacementPolicy), nameof(selectionPolicy));
            return PushBackWithManagedPolicies(algorithm, problem, populationSize, seed, replacementPolicy, selectionPolicy, evaluator, islandType);
        }

        public ulong push_back_island(algorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed = 0u, thread_island islandType = null)
        {
            ValidatePolicyPair(replacementPolicy, selectionPolicy, nameof(replacementPolicy), nameof(selectionPolicy));
            return PushBackCore(algorithm, problem, populationSize, seed, evaluator, islandType, replacementPolicy, selectionPolicy);
        }

        public ulong push_back_island(IAlgorithm algorithm, IProblem problem, ulong populationSize, uint seed = 0u, thread_island islandType = null)
        {
            return PushBackNormalizedAlgorithm(algorithm, problem, populationSize, seed, islandType: islandType);
        }

        public ulong push_back_island(IAlgorithm algorithm, IProblem problem, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed = 0u, thread_island islandType = null)
        {
            ValidatePolicyPair(replacementPolicy, selectionPolicy, nameof(replacementPolicy), nameof(selectionPolicy));
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algorithm);
            return PushBackWithManagedPolicies(normalized, problem, populationSize, seed, replacementPolicy, selectionPolicy, islandType: islandType);
        }

        public ulong push_back_island(IAlgorithm algorithm, IProblem problem, ulong populationSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed = 0u, thread_island islandType = null)
        {
            ValidatePolicyPair(replacementPolicy, selectionPolicy, nameof(replacementPolicy), nameof(selectionPolicy));
            return PushBackNormalizedAlgorithm(algorithm, problem, populationSize, seed, islandType: islandType, replacementPolicy: replacementPolicy, selectionPolicy: selectionPolicy);
        }

        public ulong push_back_island(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, uint seed = 0u, thread_island islandType = null)
        {
            return PushBackNormalizedAlgorithm(algorithm, problem, populationSize, seed, evaluator, islandType);
        }

        public ulong push_back_island(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed = 0u, thread_island islandType = null)
        {
            ValidatePolicyPair(replacementPolicy, selectionPolicy, nameof(replacementPolicy), nameof(selectionPolicy));
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algorithm);
            return PushBackWithManagedPolicies(normalized, problem, populationSize, seed, replacementPolicy, selectionPolicy, evaluator, islandType);
        }

        public ulong push_back_island(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, fair_replace replacementPolicy, select_best selectionPolicy, uint seed = 0u, thread_island islandType = null)
        {
            ValidatePolicyPair(replacementPolicy, selectionPolicy, nameof(replacementPolicy), nameof(selectionPolicy));
            return PushBackNormalizedAlgorithm(algorithm, problem, populationSize, seed, evaluator, islandType, replacementPolicy, selectionPolicy);
        }

        public island GetIsland(ulong index)
        {
            return GetIslandCopy(index);
        }

        public ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, ulong populationSize, uint seed = 0u, thread_island islandType = null)
        {
            return push_back_island(algorithm, problem, populationSize, seed, islandType);
        }

        public ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, uint seed = 0u, thread_island islandType = null)
        {
            return push_back_island(algorithm, problem, evaluator, populationSize, seed, islandType);
        }

        public ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed = 0u, thread_island islandType = null)
        {
            return push_back_island(algorithm, problem, populationSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public ulong PushBackIsland(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong populationSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint seed = 0u, thread_island islandType = null)
        {
            return push_back_island(algorithm, problem, evaluator, populationSize, replacementPolicy, selectionPolicy, seed, islandType);
        }
    }
}
