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

            return source switch
            {
                algorithm typeErased => typeErased,
                bee_colony beeColony => beeColony.to_algorithm(),
                cmaes covarianceMatrixAdaptationEvolutionStrategy => covarianceMatrixAdaptationEvolutionStrategy.to_algorithm(),
                compass_search compassSearch => compassSearch.to_algorithm(),
                ihs improvedHarmonySearch => improvedHarmonySearch.to_algorithm(),
                nsga2 nsga2Algorithm => nsga2Algorithm.to_algorithm(),
                nlopt nloptAlgorithm => nloptAlgorithm.to_algorithm(),
                moead moeadAlgorithm => moeadAlgorithm.to_algorithm(),
                moead_gen moeadGenerationalAlgorithm => moeadGenerationalAlgorithm.to_algorithm(),
                maco multiObjectiveAntColonyOptimization => multiObjectiveAntColonyOptimization.to_algorithm(),
                mbh monotonicBasinHopping => monotonicBasinHopping.to_algorithm(),
                cstrs_self_adaptive selfAdaptiveConstraints => selfAdaptiveConstraints.to_algorithm(),
                de differentialEvolution => differentialEvolution.to_algorithm(),
                de1220 differentialEvolution1220 => differentialEvolution1220.to_algorithm(),
                gaco generalizedAntColonyOptimization => generalizedAntColonyOptimization.to_algorithm(),
                gwo greyWolfOptimizer => greyWolfOptimizer.to_algorithm(),
                nspso nonDominatedSortingParticleSwarmOptimizer => nonDominatedSortingParticleSwarmOptimizer.to_algorithm(),
                null_algorithm nullAlgorithm => nullAlgorithm.to_algorithm(),
                pso particleSwarmOptimizer => particleSwarmOptimizer.to_algorithm(),
                pso_gen particleSwarmOptimizerGenerational => particleSwarmOptimizerGenerational.to_algorithm(),
                sade selfAdaptiveDifferentialEvolution => selfAdaptiveDifferentialEvolution.to_algorithm(),
                sea simpleEvolutionaryAlgorithm => simpleEvolutionaryAlgorithm.to_algorithm(),
                sga simpleGeneticAlgorithm => simpleGeneticAlgorithm.to_algorithm(),
                simulated_annealing simulatedAnnealing => simulatedAnnealing.to_algorithm(),
                xnes exponentialNaturalEvolutionStrategy => exponentialNaturalEvolutionStrategy.to_algorithm(),
                _ => new algorithm(source)
            };
        }
    }
}
