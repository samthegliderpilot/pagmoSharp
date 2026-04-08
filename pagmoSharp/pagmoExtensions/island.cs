using System;

namespace pagmo
{
    public partial class island
    {
        private static uint ToNativePopulationSize(ulong popSize)
        {
            return SizeTInterop.ToNativeUInt32(popSize, "popSize");
        }

        private static uint ResolveSeed(uint? seed)
        {
            return seed ?? new random_device().next();
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

        private static island CreateCore(
            algorithm algorithm,
            IProblem problem,
            ulong popSize,
            uint? seed,
            thread_island islandType = null,
            bfe evaluator = null,
            fair_replace replacementPolicy = null,
            select_best selectionPolicy = null,
            r_policy wrappedReplacementPolicy = null,
            s_policy wrappedSelectionPolicy = null)
        {
            ValidatePolicyPair(replacementPolicy, selectionPolicy, nameof(replacementPolicy), nameof(selectionPolicy));
            ValidatePolicyPair(wrappedReplacementPolicy, wrappedSelectionPolicy, nameof(wrappedReplacementPolicy), nameof(wrappedSelectionPolicy));

            if (replacementPolicy != null && wrappedReplacementPolicy != null)
            {
                throw new ArgumentException("Specify either fair/select policies or wrapped policies, but not both.");
            }

            using var wrappedProblem = new problem(problem);
            var nativePopSize = ToNativePopulationSize(popSize);
            var resolvedSeed = ResolveSeed(seed);

            if (replacementPolicy == null && wrappedReplacementPolicy == null)
            {
                if (islandType == null)
                {
                    return evaluator == null
                        ? island.Create(algorithm, wrappedProblem, nativePopSize, resolvedSeed)
                        : island.CreateWithBfe(algorithm, wrappedProblem, evaluator, nativePopSize, resolvedSeed);
                }

                return evaluator == null
                    ? island.CreateWithThreadIsland(islandType, algorithm, wrappedProblem, nativePopSize, resolvedSeed)
                    : island.CreateWithThreadIslandAndBfe(islandType, algorithm, wrappedProblem, evaluator, nativePopSize, resolvedSeed);
            }

            if (replacementPolicy != null)
            {
                if (islandType == null)
                {
                    return evaluator == null
                        ? island.CreateWithPolicies(algorithm, wrappedProblem, nativePopSize, replacementPolicy, selectionPolicy, resolvedSeed)
                        : island.CreateWithBfeAndPolicies(algorithm, wrappedProblem, evaluator, nativePopSize, replacementPolicy, selectionPolicy, resolvedSeed);
                }

                return evaluator == null
                    ? island.CreateWithThreadIslandAndPolicies(islandType, algorithm, wrappedProblem, nativePopSize, replacementPolicy, selectionPolicy, resolvedSeed)
                    : island.CreateWithThreadIslandAndBfeAndPolicies(islandType, algorithm, wrappedProblem, evaluator, nativePopSize, replacementPolicy, selectionPolicy, resolvedSeed);
            }

            if (islandType == null)
            {
                return evaluator == null
                    ? island.CreateWithPolicies(algorithm, wrappedProblem, nativePopSize, wrappedReplacementPolicy, wrappedSelectionPolicy, resolvedSeed)
                    : island.CreateWithBfeAndPolicies(algorithm, wrappedProblem, evaluator, nativePopSize, wrappedReplacementPolicy, wrappedSelectionPolicy, resolvedSeed);
            }

            return evaluator == null
                ? island.CreateWithThreadIslandAndPolicies(islandType, algorithm, wrappedProblem, nativePopSize, wrappedReplacementPolicy, wrappedSelectionPolicy, resolvedSeed)
                : island.CreateWithThreadIslandAndBfeAndPolicies(islandType, algorithm, wrappedProblem, evaluator, nativePopSize, wrappedReplacementPolicy, wrappedSelectionPolicy, resolvedSeed);
        }

        private static island CreateCore(
            IAlgorithm algorithm,
            IProblem problem,
            ulong popSize,
            uint? seed,
            thread_island islandType = null,
            bfe evaluator = null,
            fair_replace replacementPolicy = null,
            select_best selectionPolicy = null,
            r_policy wrappedReplacementPolicy = null,
            s_policy wrappedSelectionPolicy = null)
        {
            using var normalized = AlgorithmInterop.NormalizeToTypeErased(algorithm);
            return CreateCore(
                normalized,
                problem,
                popSize,
                seed,
                islandType,
                evaluator,
                replacementPolicy,
                selectionPolicy,
                wrappedReplacementPolicy,
                wrappedSelectionPolicy);
        }

        public static island Create(algorithm algorithm, IProblem problem, ulong popSize, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType);
        }

        public static island Create(IAlgorithm algorithm, IProblem problem, ulong popSize, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType);
        }

        public static island CreateWithPolicies(algorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, replacementPolicy: replacementPolicy, selectionPolicy: selectionPolicy);
        }

        public static island CreateWithPolicies(IAlgorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, replacementPolicy: replacementPolicy, selectionPolicy: selectionPolicy);
        }

        public static island CreateWithPolicies(algorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, wrappedReplacementPolicy: replacementPolicy, wrappedSelectionPolicy: selectionPolicy);
        }

        public static island CreateWithPolicies(IAlgorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, wrappedReplacementPolicy: replacementPolicy, wrappedSelectionPolicy: selectionPolicy);
        }

        public static island CreateWithPolicies(algorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return CreateCore(algorithm, problem, popSize, seed, islandType, wrappedReplacementPolicy: wrappedReplacementPolicy, wrappedSelectionPolicy: wrappedSelectionPolicy);
        }

        public static island CreateWithPolicies(IAlgorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return CreateCore(algorithm, problem, popSize, seed, islandType, wrappedReplacementPolicy: wrappedReplacementPolicy, wrappedSelectionPolicy: wrappedSelectionPolicy);
        }

        public static island CreateWithBfe(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator);
        }

        public static island CreateWithBfe(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator);
        }

        public static island CreateWithBfeAndPolicies(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, replacementPolicy, selectionPolicy);
        }

        public static island CreateWithBfeAndPolicies(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, replacementPolicy, selectionPolicy);
        }

        public static island CreateWithBfeAndPolicies(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, wrappedReplacementPolicy: replacementPolicy, wrappedSelectionPolicy: selectionPolicy);
        }

        public static island CreateWithBfeAndPolicies(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, wrappedReplacementPolicy: replacementPolicy, wrappedSelectionPolicy: selectionPolicy);
        }

        public static island CreateWithBfeAndPolicies(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, wrappedReplacementPolicy: wrappedReplacementPolicy, wrappedSelectionPolicy: wrappedSelectionPolicy);
        }

        public static island CreateWithBfeAndPolicies(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, wrappedReplacementPolicy: wrappedReplacementPolicy, wrappedSelectionPolicy: wrappedSelectionPolicy);
        }

        // Compatibility shims: keep explicit thread-island method names, route to canonical overloads.
        public static island CreateWithThreadIsland(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, uint? seed = null)
        {
            return Create(algorithm, problem, popSize, seed, islandType);
        }

        public static island CreateWithThreadIsland(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, uint? seed = null)
        {
            return Create(algorithm, problem, popSize, seed, islandType);
        }

        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public static island CreateWithThreadIslandAndBfe(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, uint? seed = null)
        {
            return CreateWithBfe(algorithm, problem, evaluator, popSize, seed, islandType);
        }

        public static island CreateWithThreadIslandAndBfe(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, uint? seed = null)
        {
            return CreateWithBfe(algorithm, problem, evaluator, popSize, seed, islandType);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }
    }
}
