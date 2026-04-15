using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace pagmo
{
    /// <summary>
    /// Managed construction helpers for creating pagmo islands from managed problems and algorithms.
    /// </summary>
    public partial class island
    {
        private sealed class ConstructionRootBucket
        {
            private readonly List<object> _roots = new();
            private readonly object _gate = new();

            /// <summary>
            /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
            /// </summary>
            public void Add(object value)
            {
                if (value == null)
                {
                    return;
                }

                lock (_gate)
                {
                    _roots.Add(value);
                }
            }
        }

        private static readonly ConditionalWeakTable<island, ConstructionRootBucket> ConstructionRoots = new();

        private static void AttachConstructionRoot(island owner, object value)
        {
            if (owner == null || value == null)
            {
                return;
            }

            ConstructionRoots.GetOrCreateValue(owner).Add(value);
        }

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

            var wrappedProblem = new problem(problem);
            var nativePopSize = ToNativePopulationSize(popSize);
            var resolvedSeed = ResolveSeed(seed);
            island createdIsland;

            if (replacementPolicy == null && wrappedReplacementPolicy == null)
            {
                if (islandType == null)
                {
                    createdIsland = evaluator == null
                        ? island.Create(algorithm, wrappedProblem, nativePopSize, resolvedSeed)
                        : island.CreateWithBfe(algorithm, wrappedProblem, evaluator, nativePopSize, resolvedSeed);
                }
                else
                {
                    createdIsland = evaluator == null
                        ? island.CreateWithThreadIsland(islandType, algorithm, wrappedProblem, nativePopSize, resolvedSeed)
                        : island.CreateWithThreadIslandAndBfe(islandType, algorithm, wrappedProblem, evaluator, nativePopSize, resolvedSeed);
                }
            }
            else if (replacementPolicy != null)
            {
                if (islandType == null)
                {
                    createdIsland = evaluator == null
                        ? island.CreateWithPolicies(algorithm, wrappedProblem, nativePopSize, replacementPolicy, selectionPolicy, resolvedSeed)
                        : island.CreateWithBfeAndPolicies(algorithm, wrappedProblem, evaluator, nativePopSize, replacementPolicy, selectionPolicy, resolvedSeed);
                }
                else
                {
                    createdIsland = evaluator == null
                        ? island.CreateWithThreadIslandAndPolicies(islandType, algorithm, wrappedProblem, nativePopSize, replacementPolicy, selectionPolicy, resolvedSeed)
                        : island.CreateWithThreadIslandAndBfeAndPolicies(islandType, algorithm, wrappedProblem, evaluator, nativePopSize, replacementPolicy, selectionPolicy, resolvedSeed);
                }
            }
            else if (islandType == null)
            {
                createdIsland = evaluator == null
                    ? island.CreateWithPolicies(algorithm, wrappedProblem, nativePopSize, wrappedReplacementPolicy, wrappedSelectionPolicy, resolvedSeed)
                    : island.CreateWithBfeAndPolicies(algorithm, wrappedProblem, evaluator, nativePopSize, wrappedReplacementPolicy, wrappedSelectionPolicy, resolvedSeed);
            }
            else
            {
                createdIsland = evaluator == null
                    ? island.CreateWithThreadIslandAndPolicies(islandType, algorithm, wrappedProblem, nativePopSize, wrappedReplacementPolicy, wrappedSelectionPolicy, resolvedSeed)
                    : island.CreateWithThreadIslandAndBfeAndPolicies(islandType, algorithm, wrappedProblem, evaluator, nativePopSize, wrappedReplacementPolicy, wrappedSelectionPolicy, resolvedSeed);
            }

            // Keep wrapped managed-problem callback alive for the island lifetime.
            AttachConstructionRoot(createdIsland, wrappedProblem);
            return createdIsland;
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
            var normalized = AlgorithmInterop.NormalizeToTypeErased(algorithm);
            var createdIsland = CreateCore(
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

            // If normalization created a temporary type-erased algorithm, keep it rooted.
            if (!ReferenceEquals(normalized, algorithm))
            {
                AttachConstructionRoot(createdIsland, normalized);
            }

            return createdIsland;
        }

        /// <summary>
        /// Creates an island from a type-erased algorithm and managed problem.
        /// </summary>
        public static island Create(algorithm algorithm, IProblem problem, ulong popSize, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType);
        }

        /// <summary>
        /// Creates an island from a managed algorithm and managed problem.
        /// </summary>
        public static island Create(IAlgorithm algorithm, IProblem problem, ulong popSize, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType);
        }

        /// <summary>
        /// Creates an island with built-in policy objects.
        /// </summary>
        public static island CreateWithPolicies(algorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, replacementPolicy: replacementPolicy, selectionPolicy: selectionPolicy);
        }

        /// <summary>
        /// Creates an island with built-in policy objects.
        /// </summary>
        public static island CreateWithPolicies(IAlgorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, replacementPolicy: replacementPolicy, selectionPolicy: selectionPolicy);
        }

        /// <summary>
        /// Creates an island with wrapped replacement/selection policies.
        /// </summary>
        public static island CreateWithPolicies(algorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, wrappedReplacementPolicy: replacementPolicy, wrappedSelectionPolicy: selectionPolicy);
        }

        /// <summary>
        /// Creates an island with wrapped replacement/selection policies.
        /// </summary>
        public static island CreateWithPolicies(IAlgorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, wrappedReplacementPolicy: replacementPolicy, wrappedSelectionPolicy: selectionPolicy);
        }

        /// <summary>
        /// Creates an island with managed policy implementations.
        /// </summary>
        public static island CreateWithPolicies(algorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return CreateCore(algorithm, problem, popSize, seed, islandType, wrappedReplacementPolicy: wrappedReplacementPolicy, wrappedSelectionPolicy: wrappedSelectionPolicy);
        }

        /// <summary>
        /// Creates an island with managed policy implementations.
        /// </summary>
        public static island CreateWithPolicies(IAlgorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return CreateCore(algorithm, problem, popSize, seed, islandType, wrappedReplacementPolicy: wrappedReplacementPolicy, wrappedSelectionPolicy: wrappedSelectionPolicy);
        }

        /// <summary>
        /// Creates an island with an explicit batch fitness evaluator.
        /// </summary>
        public static island CreateWithBfe(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator);
        }

        /// <summary>
        /// Creates an island with an explicit batch fitness evaluator.
        /// </summary>
        public static island CreateWithBfe(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator);
        }

        /// <summary>
        /// Creates an island with explicit evaluator and built-in policies.
        /// </summary>
        public static island CreateWithBfeAndPolicies(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, replacementPolicy, selectionPolicy);
        }

        /// <summary>
        /// Creates an island with explicit evaluator and built-in policies.
        /// </summary>
        public static island CreateWithBfeAndPolicies(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, replacementPolicy, selectionPolicy);
        }

        /// <summary>
        /// Creates an island with explicit evaluator and wrapped policies.
        /// </summary>
        public static island CreateWithBfeAndPolicies(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, wrappedReplacementPolicy: replacementPolicy, wrappedSelectionPolicy: selectionPolicy);
        }

        /// <summary>
        /// Creates an island with explicit evaluator and wrapped policies.
        /// </summary>
        public static island CreateWithBfeAndPolicies(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, wrappedReplacementPolicy: replacementPolicy, wrappedSelectionPolicy: selectionPolicy);
        }

        /// <summary>
        /// Creates an island with explicit evaluator and managed policies.
        /// </summary>
        public static island CreateWithBfeAndPolicies(algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, wrappedReplacementPolicy: wrappedReplacementPolicy, wrappedSelectionPolicy: wrappedSelectionPolicy);
        }

        /// <summary>
        /// Creates an island with explicit evaluator and managed policies.
        /// </summary>
        public static island CreateWithBfeAndPolicies(IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null, thread_island islandType = null)
        {
            using var wrappedReplacementPolicy = new r_policy(replacementPolicy);
            using var wrappedSelectionPolicy = new s_policy(selectionPolicy);
            return CreateCore(algorithm, problem, popSize, seed, islandType, evaluator, wrappedReplacementPolicy: wrappedReplacementPolicy, wrappedSelectionPolicy: wrappedSelectionPolicy);
        }

        // Compatibility shims: keep explicit thread-island method names, route to canonical overloads.
        /// <summary>
        /// Compatibility helper equivalent to <see cref="Create(algorithm,IProblem,ulong,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIsland(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, uint? seed = null)
        {
            return Create(algorithm, problem, popSize, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="Create(IAlgorithm,IProblem,ulong,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIsland(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, uint? seed = null)
        {
            return Create(algorithm, problem, popSize, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithPolicies(algorithm,IProblem,ulong,fair_replace,select_best,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithPolicies(IAlgorithm,IProblem,ulong,fair_replace,select_best,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithPolicies(algorithm,IProblem,ulong,r_policy,s_policy,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithPolicies(IAlgorithm,IProblem,ulong,r_policy,s_policy,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithPolicies(algorithm,IProblem,ulong,r_policyBase,s_policyBase,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithPolicies(IAlgorithm,IProblem,ulong,r_policyBase,s_policyBase,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null)
        {
            return CreateWithPolicies(algorithm, problem, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithBfe(algorithm,IProblem,bfe,ulong,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndBfe(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, uint? seed = null)
        {
            return CreateWithBfe(algorithm, problem, evaluator, popSize, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithBfe(IAlgorithm,IProblem,bfe,ulong,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndBfe(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, uint? seed = null)
        {
            return CreateWithBfe(algorithm, problem, evaluator, popSize, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithBfeAndPolicies(algorithm,IProblem,bfe,ulong,fair_replace,select_best,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithBfeAndPolicies(IAlgorithm,IProblem,bfe,ulong,fair_replace,select_best,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, fair_replace replacementPolicy, select_best selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithBfeAndPolicies(algorithm,IProblem,bfe,ulong,r_policy,s_policy,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithBfeAndPolicies(IAlgorithm,IProblem,bfe,ulong,r_policy,s_policy,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policy replacementPolicy, s_policy selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithBfeAndPolicies(algorithm,IProblem,bfe,ulong,r_policyBase,s_policyBase,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, algorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }

        /// <summary>
        /// Compatibility helper equivalent to <see cref="CreateWithBfeAndPolicies(IAlgorithm,IProblem,bfe,ulong,r_policyBase,s_policyBase,uint?,thread_island)"/>.
        /// </summary>
        public static island CreateWithThreadIslandAndBfeAndPolicies(thread_island islandType, IAlgorithm algorithm, IProblem problem, bfe evaluator, ulong popSize, r_policyBase replacementPolicy, s_policyBase selectionPolicy, uint? seed = null)
        {
            return CreateWithBfeAndPolicies(algorithm, problem, evaluator, popSize, replacementPolicy, selectionPolicy, seed, islandType);
        }
    }
}

