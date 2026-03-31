using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_cstrs_self_adaptive
{
    [Test]
    public void NameAndBasicPropertiesAreAccessible()
    {
        using var algorithm = new cstrs_self_adaptive(6u);
        Assert.AreEqual("sa-CNSTR: Self-adaptive constraints handling", algorithm.get_name());

        algorithm.set_seed(3u);
        Assert.AreEqual(3u, algorithm.get_seed());

        algorithm.set_verbosity(2u);
        Assert.AreEqual(2u, algorithm.get_verbosity());
        Assert.That(algorithm.get_extra_info(), Is.Not.Null);
    }

    [Test]
    public void EvolvesConstrainedProblemAndPreservesPopulationSize()
    {
        using var problem = new TwoDimensionalConstrainedProblem();
        using var algorithm = new cstrs_self_adaptive(10u);
        using var pop = new population(problem, 64u, 2u);

        var originalSize = pop.size();
        using var evolved = algorithm.evolve(pop);

        Assert.AreEqual(originalSize, evolved.size());
        using var evolvedProblem = evolved.get_problem();
        Assert.AreEqual(problem.get_nobj(), evolvedProblem.get_nobj());
    }

    [Test]
    public void ImprovesFeasibleObjectiveOnTwoParabolaConstrainedProblem()
    {
        using var problem = new TwoParabolaConstrainedProblemWrapper();
        using var innerAlgorithm = new de(2u);
        using var erasedInnerAlgorithm = innerAlgorithm.to_algorithm();
        using var algorithm = new cstrs_self_adaptive(30u, erasedInnerAlgorithm, 7u);
        using var initialPopulation = new population(problem, 96u, 11u);

        var initialBestFeasibleObjective = GetBestFeasibleObjective(initialPopulation);
        Assert.That(initialBestFeasibleObjective, Is.Not.EqualTo(double.PositiveInfinity), "initial population should contain feasible points");

        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        var evolvedBestFeasibleObjective = GetBestFeasibleObjective(evolvedPopulation);

        Assert.That(evolvedBestFeasibleObjective, Is.Not.EqualTo(double.PositiveInfinity), "evolved population should contain feasible points");
        Assert.That(evolvedBestFeasibleObjective, Is.LessThan(initialBestFeasibleObjective), "optimization should improve best feasible objective");
        Assert.That(evolvedBestFeasibleObjective, Is.LessThan(1.2), "problem minimum is 1.0 at (0.5, -0.5), evolved solution should get close");
    }

    [Test]
    public void TypedAndGenericLogsAreExposed()
    {
        using var problem = new TwoDimensionalConstrainedProblem();
        using var algorithm = new cstrs_self_adaptive(12u);
        using var population = new population(problem, 64u, 2u);

        algorithm.set_seed(2u);
        algorithm.set_verbosity(1u);

        using var _ = algorithm.evolve(population);

        var typedLines = algorithm.GetTypedLogLines();
        Assert.That(typedLines.Count, Is.GreaterThan(0), "cstrs_self_adaptive verbosity should produce at least one log line");

        IAlgorithm algorithmInterface = algorithm;
        var genericLines = algorithmInterface.GetLogLines();
        Assert.That(genericLines.Count, Is.EqualTo(typedLines.Count));

        var raw = genericLines[0].RawFields;
        Assert.That(genericLines[0].AlgorithmName, Is.EqualTo("cstrs_self_adaptive"));
        Assert.That(raw.ContainsKey("iteration"), Is.True);
        Assert.That(raw.ContainsKey("function_evaluations"), Is.True);
        Assert.That(raw.ContainsKey("best_fitness"), Is.True);
        Assert.That(raw.ContainsKey("infeasibility"), Is.True);
        Assert.That(raw.ContainsKey("violated_constraints"), Is.True);
        Assert.That(raw.ContainsKey("violation_norm"), Is.True);
        Assert.That(raw.ContainsKey("feasible_count"), Is.True);
        Assert.That(genericLines[0].ToDisplayString(), Does.Contain("iter="));

        Assert.That((uint)raw["iteration"], Is.EqualTo(typedLines[0].Iteration));
        Assert.That((ulong)raw["function_evaluations"], Is.EqualTo(typedLines[0].FunctionEvaluations));
        Assert.That((double)raw["best_fitness"], Is.EqualTo(typedLines[0].BestFitness));
        Assert.That((double)raw["infeasibility"], Is.EqualTo(typedLines[0].Infeasibility));
        Assert.That((ulong)raw["feasible_count"], Is.EqualTo(typedLines[0].FeasibleCount));
    }

    private static double GetBestFeasibleObjective(population pop)
    {
        using var allFitness = pop.get_f();

        var best = double.PositiveInfinity;
        for (var individualIndex = 0; individualIndex < allFitness.Count; individualIndex++)
        {
            var fitness = allFitness[individualIndex];
            if (fitness.Count < 2)
            {
                continue;
            }

            var objective = fitness[0];
            var inequalityConstraint = fitness[1];
            if (inequalityConstraint <= 0.0 && objective < best)
            {
                best = objective;
            }
        }

        return best;
    }
}
