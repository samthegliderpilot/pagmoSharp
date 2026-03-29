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

            if (source is cmaes covarianceMatrixAdaptationEvolutionStrategy)
            {
                return covarianceMatrixAdaptationEvolutionStrategy.to_algorithm();
            }

            if (source is compass_search compassSearch)
            {
                return compassSearch.to_algorithm();
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

            if (source is de differentialEvolution)
            {
                return differentialEvolution.to_algorithm();
            }

            if (source is de1220 differentialEvolution1220)
            {
                return differentialEvolution1220.to_algorithm();
            }

            if (source is gaco generalizedAntColonyOptimization)
            {
                return generalizedAntColonyOptimization.to_algorithm();
            }

            if (source is gwo greyWolfOptimizer)
            {
                return greyWolfOptimizer.to_algorithm();
            }

            if (source is nspso nonDominatedSortingParticleSwarmOptimizer)
            {
                return nonDominatedSortingParticleSwarmOptimizer.to_algorithm();
            }

            if (source is null_algorithm nullAlgorithm)
            {
                return nullAlgorithm.to_algorithm();
            }

            if (source is pso particleSwarmOptimizer)
            {
                return particleSwarmOptimizer.to_algorithm();
            }

            if (source is pso_gen particleSwarmOptimizerGenerational)
            {
                return particleSwarmOptimizerGenerational.to_algorithm();
            }

            if (source is sade selfAdaptiveDifferentialEvolution)
            {
                return selfAdaptiveDifferentialEvolution.to_algorithm();
            }

            if (source is sea simpleEvolutionaryAlgorithm)
            {
                return simpleEvolutionaryAlgorithm.to_algorithm();
            }

            if (source is sga simpleGeneticAlgorithm)
            {
                return simpleGeneticAlgorithm.to_algorithm();
            }

            if (source is simulated_annealing simulatedAnnealing)
            {
                return simulatedAnnealing.to_algorithm();
            }

            if (source is xnes exponentialNaturalEvolutionStrategy)
            {
                return exponentialNaturalEvolutionStrategy.to_algorithm();
            }

            throw new NotSupportedException(
                $"Algorithm type '{source.GetType().Name}' is not currently supported in type-erased contexts. " +
                "Pass pagmo.algorithm directly or add an explicit conversion bridge for this UDA.");
        }
    }
}
