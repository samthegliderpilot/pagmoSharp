using System;
using System.Reflection;
using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET.Algorithms;

[TestFixture]
public class Test_optional_solver_availability
{
    // Minimal differentiable UDP used to validate IPOPT execution path end-to-end.
    private sealed class IpoptDifferentiableProblem : ManagedProblemBase
    {
        private readonly DoubleVector _lb = new(new[] { -10.0, -10.0 });
        private readonly DoubleVector _ub = new(new[] { 10.0, 10.0 });

        public override string get_name() => "IpoptDifferentiableProblem";
        public override PairOfDoubleVectors get_bounds() => new(_lb, _ub);
        public override ThreadSafety get_thread_safety() => ThreadSafety.Constant;
        public override DoubleVector fitness(DoubleVector x) => new(new[] { x[0] * x[0] + (x[1] - 3.0) * (x[1] - 3.0) });
        public override bool has_gradient() => true;
        public override DoubleVector gradient(DoubleVector x) => new(new[] { 2.0 * x[0], 2.0 * x[1] - 6.0 });
        public override bool has_gradient_sparsity() => true;
        public override SparsityPattern gradient_sparsity() => Sparsity((0u, 0u), (0u, 1u));
    }

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
    public void Snopt7AvailabilityIsExplicit()
    {
        AssertOptionalSolverSurface("pagmo.snopt7", "snopt7");
    }

    [Test]
    public void NloptWhenPresentSupportsConstructAndEvolve()
    {
        if (!OptionalSolverAvailability.IsNloptAvailable)
        {
            Assert.Pass("nlopt is not available in this build.");
            return;
        }

        var assembly = typeof(algorithm).Assembly;
        var nloptType = assembly.GetType("pagmo.nlopt", throwOnError: false, ignoreCase: false);
        Assert.That(nloptType, Is.Not.Null, "nlopt type should be present when native support is available.");

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

    [Test]
    public void IpoptWhenPresentSupportsConstructAndEvolve()
    {
        if (!OptionalSolverAvailability.IsIpoptAvailable)
        {
            Assert.Pass("ipopt is not available in this build.");
            return;
        }

        var assembly = typeof(algorithm).Assembly;
        var ipoptType = assembly.GetType("pagmo.ipopt", throwOnError: false, ignoreCase: false);
        Assert.That(ipoptType, Is.Not.Null, "ipopt type should be present when native support is available.");

        using var managed = new IpoptDifferentiableProblem();
        using var differentiableProblem = new problem(managed);
        using var population = new population(differentiableProblem, 1u, 2u);

        using var solver = (IDisposable)Activator.CreateInstance(ipoptType)!;
        Assert.That(solver, Is.Not.Null, "ipopt should be constructible when available.");

        var setVerbosity = ipoptType.GetMethod("set_verbosity", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(setVerbosity, Is.Not.Null);
        setVerbosity!.Invoke(solver, new object[] { 1u });
        var setIntegerOption = ipoptType.GetMethod("set_integer_option", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string), typeof(ulong) }, null);
        Assert.That(setIntegerOption, Is.Not.Null, "ipopt should expose primitive set_integer_option(name, ulong).");
        setIntegerOption!.Invoke(solver, new object[] { "max_iter", 20ul });

        var getName = ipoptType.GetMethod("get_name", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(getName, Is.Not.Null);
        var solverName = (string)getName!.Invoke(solver, Array.Empty<object>())!;
        Assert.That(solverName, Is.Not.Null.And.Not.Empty);

        var evolve = ipoptType.GetMethod("evolve", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(evolve, Is.Not.Null, "ipopt should expose evolve(population).");

        using var evolved = (population)evolve!.Invoke(solver, new object[] { population })!;
        Assert.That(evolved, Is.Not.Null, "ipopt evolve should return an evolved population.");
        Assert.That(evolved!.size(), Is.EqualTo(population.size()), "evolve() should preserve population size.");

        var setSeed = ipoptType.GetMethod("set_seed", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(setSeed, Is.Not.Null, "ipopt should provide an explicit IAlgorithm set_seed contract.");
        Assert.That(
            () => setSeed!.Invoke(solver, new object[] { 2u }),
            Throws.Exception.TypeOf<TargetInvocationException>().With.InnerException.TypeOf<NotSupportedException>());

        var getSeed = ipoptType.GetMethod("get_seed", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(getSeed, Is.Not.Null, "ipopt should provide an explicit IAlgorithm get_seed contract.");
        Assert.That(
            () => getSeed!.Invoke(solver, Array.Empty<object>()),
            Throws.Exception.TypeOf<TargetInvocationException>().With.InnerException.TypeOf<NotSupportedException>());

        var getVerbosity = ipoptType.GetMethod("get_verbosity", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(getVerbosity, Is.Not.Null, "ipopt should provide an explicit IAlgorithm get_verbosity contract.");
        Assert.That(
            () => getVerbosity!.Invoke(solver, Array.Empty<object>()),
            Throws.Exception.TypeOf<TargetInvocationException>().With.InnerException.TypeOf<NotSupportedException>());

        var toAlgorithm = ipoptType.GetMethod("to_algorithm", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(toAlgorithm, Is.Not.Null, "ipopt should expose to_algorithm() for type-erased flows.");

        using var erased = (algorithm)toAlgorithm!.Invoke(solver, Array.Empty<object>())!;
        Assert.That(erased, Is.Not.Null);
        Assert.That(erased!.get_name(), Is.Not.Empty);

        var getLogLines = ipoptType.GetMethod("GetLogLines", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(getLogLines, Is.Not.Null, "ipopt should expose shared algorithm log projection.");
        var lines = (System.Collections.IEnumerable)getLogLines!.Invoke(solver, Array.Empty<object>())!;
        Assert.That(lines, Is.Not.Null);

        var getLastOptimizationResultCode = ipoptType.GetMethod("GetLastOptimizationResultCode", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(getLastOptimizationResultCode, Is.Not.Null, "ipopt should expose primitive last-result status code.");
        var statusCode = (int)getLastOptimizationResultCode!.Invoke(solver, Array.Empty<object>())!;
        Assert.That(statusCode, Is.Not.EqualTo(0), "ipopt status code should reflect a real optimization result after evolve().");

        AssertNoSwigTypeLeaksOnPublicSurface(ipoptType);
    }

    [Test]
    public void Snopt7WhenPresentSupportsConstructAndEvolve()
    {
        var assembly = typeof(algorithm).Assembly;
        var snopt7Type = assembly.GetType("pagmo.snopt7", throwOnError: false, ignoreCase: false);
        if (snopt7Type == null)
        {
            Assert.Pass("snopt7 is not available in this build.");
            return;
        }

        var snopt7Lib = Environment.GetEnvironmentVariable("SNOPT7_LIB");
        if (string.IsNullOrEmpty(snopt7Lib))
        {
            Assert.Pass("SNOPT7_LIB env var not set; skipping live snopt7 execution test.");
            return;
        }

        using var managed = new IpoptDifferentiableProblem();
        using var differentiableProblem = new problem(managed);
        using var population = new population(differentiableProblem, 1u, 2u);

        using var solver = (IDisposable)Activator.CreateInstance(snopt7Type, false, snopt7Lib, 6u)!;
        Assert.That(solver, Is.Not.Null, "snopt7 should be constructible when available.");

        var setVerbosity = snopt7Type.GetMethod("set_verbosity", BindingFlags.Public | BindingFlags.Instance);
        setVerbosity?.Invoke(solver, new object[] { 1u });

        var getName = snopt7Type.GetMethod("get_name", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(getName, Is.Not.Null);
        var solverName = (string)getName!.Invoke(solver, Array.Empty<object>())!;
        Assert.That(solverName, Is.Not.Null.And.Not.Empty);

        var evolve = snopt7Type.GetMethod("evolve", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(evolve, Is.Not.Null, "snopt7 should expose evolve(population).");

        using var evolved = (population)evolve!.Invoke(solver, new object[] { population })!;
        Assert.That(evolved, Is.Not.Null, "snopt7 evolve should return an evolved population.");
        Assert.That(evolved!.size(), Is.EqualTo(population.size()), "evolve() should preserve population size.");

        var setSeed = snopt7Type.GetMethod("set_seed", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(setSeed, Is.Not.Null);
        Assert.That(
            () => setSeed!.Invoke(solver, new object[] { 2u }),
            Throws.Exception.TypeOf<TargetInvocationException>().With.InnerException.TypeOf<NotSupportedException>());

        var toAlgorithm = snopt7Type.GetMethod("to_algorithm", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(toAlgorithm, Is.Not.Null, "snopt7 should expose to_algorithm() for type-erased flows.");

        using var erased = (algorithm)toAlgorithm!.Invoke(solver, Array.Empty<object>())!;
        Assert.That(erased, Is.Not.Null);
        Assert.That(erased!.get_name(), Is.Not.Empty);

        var getLogLines = snopt7Type.GetMethod("GetLogLines", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(getLogLines, Is.Not.Null, "snopt7 should expose shared algorithm log projection.");
        var lines = (System.Collections.IEnumerable)getLogLines!.Invoke(solver, Array.Empty<object>())!;
        Assert.That(lines, Is.Not.Null);

        AssertNoSwigTypeLeaksOnPublicSurface(snopt7Type);
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

    private static void AssertNoSwigTypeLeaksOnPublicSurface(Type type)
    {
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        foreach (var method in methods)
        {
            Assert.That(
                method.ReturnType.Name.StartsWith("SWIGTYPE_", StringComparison.Ordinal),
                Is.False,
                $"Public method {method.Name} should not return SWIGTYPE_*.");

            foreach (var parameter in method.GetParameters())
            {
                Assert.That(
                    parameter.ParameterType.Name.StartsWith("SWIGTYPE_", StringComparison.Ordinal),
                    Is.False,
                    $"Public method {method.Name} should not take SWIGTYPE_* parameters.");
            }
        }
    }
}
