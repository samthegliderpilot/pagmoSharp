using System;
using System.Reflection;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_optional_solver_availability
{
    [Test]
    public void IpoptAvailabilityIsExplicit()
    {
        AssertOptionalSolverSurface("pagmo.ipopt", "ipopt");
    }

    [Test]
    public void NloptAvailabilityIsExplicit()
    {
        AssertOptionalSolverSurface("pagmo.nlopt", "nlopt");
    }

    [Test]
    public void NloptWhenPresentSupportsConstructAndEvolve()
    {
        var assembly = typeof(algorithm).Assembly;
        var nloptType = assembly.GetType("pagmo.nlopt", throwOnError: false, ignoreCase: false);
        if (nloptType == null)
        {
            Assert.Pass("nlopt is not available in this build.");
            return;
        }

        using var rosenbrockProblem = new rosenbrock(2u);
        using var population = new population(rosenbrockProblem, 32u, 2u);

        using var solver = (IDisposable)Activator.CreateInstance(nloptType)!;
        Assert.That(solver, Is.Not.Null, "nlopt should be constructible when available.");

        var setVerbosity = nloptType.GetMethod("set_verbosity", BindingFlags.Public | BindingFlags.Instance);
        setVerbosity?.Invoke(solver, new object[] { 1u });

        var getName = nloptType.GetMethod("get_name", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(getName, Is.Not.Null);
        var solverName = (string)getName!.Invoke(solver, Array.Empty<object>())!;
        Assert.That(solverName, Is.Not.Null.And.Not.Empty);

        var evolve = nloptType.GetMethod("evolve", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(evolve, Is.Not.Null, "nlopt should expose evolve(population).");

        using var evolved = (population)evolve!.Invoke(solver, new object[] { population })!;
        Assert.That(evolved, Is.Not.Null, "nlopt evolve should return an evolved population.");
        Assert.That(evolved!.size(), Is.EqualTo(population.size()), "evolve() should preserve population size.");

        var setSeed = nloptType.GetMethod("set_seed", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(setSeed, Is.Not.Null, "nlopt should provide an explicit IAlgorithm set_seed contract.");
        Assert.That(
            () => setSeed!.Invoke(solver, new object[] { 2u }),
            Throws.Exception.TypeOf<TargetInvocationException>().With.InnerException.TypeOf<NotSupportedException>());

        var getSeed = nloptType.GetMethod("get_seed", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(getSeed, Is.Not.Null, "nlopt should provide an explicit IAlgorithm get_seed contract.");
        Assert.That(
            () => getSeed!.Invoke(solver, Array.Empty<object>()),
            Throws.Exception.TypeOf<TargetInvocationException>().With.InnerException.TypeOf<NotSupportedException>());

        var getVerbosity = nloptType.GetMethod("get_verbosity", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(getVerbosity, Is.Not.Null, "nlopt should provide an explicit IAlgorithm get_verbosity contract.");
        Assert.That(
            () => getVerbosity!.Invoke(solver, Array.Empty<object>()),
            Throws.Exception.TypeOf<TargetInvocationException>().With.InnerException.TypeOf<NotSupportedException>());

        var toAlgorithm = nloptType.GetMethod("to_algorithm", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(toAlgorithm, Is.Not.Null, "nlopt should expose to_algorithm() for type-erased flows.");

        using var erased = (algorithm)toAlgorithm!.Invoke(solver, Array.Empty<object>())!;
        Assert.That(erased, Is.Not.Null);
        Assert.That(erased!.get_name(), Is.Not.Empty);
    }

    private static void AssertOptionalSolverSurface(string fullTypeName, string solverName)
    {
        var assembly = typeof(algorithm).Assembly;
        var type = assembly.GetType(fullTypeName, throwOnError: false, ignoreCase: false);
        if (type == null)
        {
            Assert.Pass($"{solverName} is not available in this build.");
            return;
        }

        Assert.That(typeof(IAlgorithm).IsAssignableFrom(type), Is.True, $"{solverName} should implement IAlgorithm when present.");
        Assert.That(typeof(IDisposable).IsAssignableFrom(type), Is.True, $"{solverName} should implement IDisposable when present.");

        var publicConstructors = type.GetConstructors();
        Assert.That(publicConstructors.Length, Is.GreaterThan(0), $"{solverName} should expose at least one public constructor when present.");
    }
}
