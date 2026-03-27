using System;

namespace pagmo
{
    internal static class AlgorithmInterop
    {
        internal static algorithm NormalizeToTypeErased(IAlgorithm source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is algorithm typeErased)
            {
                return typeErased;
            }

            if (source is bee_colony beeColony)
            {
                return beeColony.to_algorithm();
            }
            
            if (source is ihs improvedHarmonySearch)
            {
                return improvedHarmonySearch.to_algorithm();
            }

            if (source is nsga2 nsga2Algorithm)
            {
                return nsga2Algorithm.to_algorithm();
            }

            if (source is moead moeadAlgorithm)
            {
                return moeadAlgorithm.to_algorithm();
            }

            if (source is moead_gen moeadGenerationalAlgorithm)
            {
                return moeadGenerationalAlgorithm.to_algorithm();
            }

            if (source is maco multiObjectiveAntColonyOptimization)
            {
                return multiObjectiveAntColonyOptimization.to_algorithm();
            }

            if (source is mbh monotonicBasinHopping)
            {
                return monotonicBasinHopping.to_algorithm();
            }

            if (source is cstrs_self_adaptive selfAdaptiveConstraints)
            {
                return selfAdaptiveConstraints.to_algorithm();
            }

            throw new NotSupportedException(
                $"Algorithm type '{source.GetType().Name}' is not currently supported in type-erased contexts. " +
                "Pass pagmo.algorithm directly or add an explicit conversion bridge for this UDA.");
        }
    }
}
