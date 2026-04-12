using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Tests.PagmoSharp.Interop;

[TestFixture]
public class Test_handwritten_api_surface_audit
{
    [Test]
    public void HandwrittenPublicApiDoesNotExposeSwigtypeSignatures()
    {
        var extensionsRoot = GetExtensionsRoot();
        var excludedFiles = new[]
        {
            Path.Combine("Problems", "ProblemCallbackAdapter.cs")
        };

        var offenders = Directory
            .EnumerateFiles(extensionsRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => excludedFiles.All(exclusion => !path.EndsWith(exclusion, StringComparison.OrdinalIgnoreCase)))
            .Where(path => Regex.IsMatch(File.ReadAllText(path), @"public\s+[^\r\n;{]*SWIGTYPE_", RegexOptions.Multiline))
            .Select(path => Path.GetRelativePath(extensionsRoot, path))
            .ToArray();

        Assert.That(offenders, Is.Empty, "Handwritten extension public surface should not expose SWIGTYPE_* signatures.");
    }

    [Test]
    public void HandwrittenSourcesAvoidGeneratedArgumentPlaceholders()
    {
        var extensionsRoot = GetExtensionsRoot();
        var offenders = Directory
            .EnumerateFiles(extensionsRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => Regex.IsMatch(File.ReadAllText(path), @"\barg\d+\b", RegexOptions.Multiline))
            .Select(path => Path.GetRelativePath(extensionsRoot, path))
            .ToArray();

        Assert.That(offenders, Is.Empty, "Handwritten sources should use descriptive parameter names, not arg0/arg1 placeholders.");
    }

    [Test]
    public void HandwrittenSourcesDoNotUseUndisposedNativeSnapshotPattern()
    {
        var extensionsRoot = GetExtensionsRoot();
        var offenders = Directory
            .EnumerateFiles(extensionsRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => Regex.IsMatch(File.ReadAllText(path), @"^\s*var\s+native\s*=\s*get_[a-zA-Z0-9_]+\(", RegexOptions.Multiline))
            .Select(path => Path.GetRelativePath(extensionsRoot, path))
            .ToArray();

        Assert.That(
            offenders,
            Is.Empty,
            "Handwritten extension code should dispose native snapshot containers (prefer `using var native = get_*()` for temporary wrappers).");
    }

    [Test]
    public void DirectCreateProblemPointerUsageIsConstrainedToInteropBoundaryFiles()
    {
        var extensionsRoot = GetExtensionsRoot();
        var allowedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Path.Combine("problem.cs")
        };

        var offenders = Directory
            .EnumerateFiles(extensionsRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => File.ReadAllText(path).Contains("NativeInterop.CreateProblemPointer(", StringComparison.Ordinal))
            .Where(path => !allowedFiles.Contains(Path.GetRelativePath(extensionsRoot, path)))
            .Select(path => Path.GetRelativePath(extensionsRoot, path))
            .ToArray();

        Assert.That(
            offenders,
            Is.Empty,
            "Direct CreateProblemPointer usage should stay constrained to dedicated interop boundary files.");
    }

    [Test]
    public void DirectCreateProblemHandleUsageIsConstrainedToInteropBoundaryFiles()
    {
        var extensionsRoot = GetExtensionsRoot();
        var allowedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Path.Combine("population.cs"),
            Path.Combine("BatchEvaluators", "bfe.cs"),
            Path.Combine("Utils", "GradientsAndHessians.cs")
        };

        var offenders = Directory
            .EnumerateFiles(extensionsRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => File.ReadAllText(path).Contains("NativeInterop.CreateProblemHandle(", StringComparison.Ordinal))
            .Where(path => !allowedFiles.Contains(Path.GetRelativePath(extensionsRoot, path)))
            .Select(path => Path.GetRelativePath(extensionsRoot, path))
            .ToArray();

        Assert.That(
            offenders,
            Is.Empty,
            "Direct CreateProblemHandle usage should stay constrained to dedicated interop boundary files.");
    }

    [Test]
    public void DirectProblemDeleteCallsStayInsideInteropOwnershipLayer()
    {
        var extensionsRoot = GetExtensionsRoot();
        var allowedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Path.Combine("NativeInterop.cs"),
            Path.Combine("Interop", "ProblemHandle.cs")
        };

        var offenders = Directory
            .EnumerateFiles(extensionsRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => File.ReadAllText(path).Contains("NativeInterop.problem_delete(", StringComparison.Ordinal))
            .Where(path => !allowedFiles.Contains(Path.GetRelativePath(extensionsRoot, path)))
            .Select(path => Path.GetRelativePath(extensionsRoot, path))
            .ToArray();

        Assert.That(
            offenders,
            Is.Empty,
            "NativeInterop.problem_delete should only be called by centralized ownership primitives.");
    }

    [Test]
    public void SwigReleaseUsageIsConstrainedToInteropOwnershipFiles()
    {
        var extensionsRoot = GetExtensionsRoot();
        var allowedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Path.Combine("NativeInterop.cs"),
            Path.Combine("r_policy.cs"),
            Path.Combine("s_policy.cs"),
            Path.Combine("Problems", "ProblemCallbackAdapter.cs")
        };

        var offenders = Directory
            .EnumerateFiles(extensionsRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => File.ReadAllText(path).Contains("swigRelease(", StringComparison.Ordinal))
            .Where(path => !allowedFiles.Contains(Path.GetRelativePath(extensionsRoot, path)))
            .Select(path => Path.GetRelativePath(extensionsRoot, path))
            .ToArray();

        Assert.That(
            offenders,
            Is.Empty,
            "swigRelease ownership handoff should remain constrained to dedicated interop ownership files.");
    }

    [Test]
    public void NonOwningRawPointerWrapperConstructionIsConstrained()
    {
        var extensionsRoot = GetExtensionsRoot();
        var allowedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Path.Combine("r_policy.cs"),
            Path.Combine("s_policy.cs")
        };

        var offenders = Directory
            .EnumerateFiles(extensionsRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => Regex.IsMatch(File.ReadAllText(path), @"new\s+[A-Za-z0-9_<>]+\([^)]*false\)", RegexOptions.Multiline))
            .Where(path => !allowedFiles.Contains(Path.GetRelativePath(extensionsRoot, path)))
            .Select(path => Path.GetRelativePath(extensionsRoot, path))
            .ToArray();

        Assert.That(
            offenders,
            Is.Empty,
            "Non-owning raw-pointer wrapper construction should stay constrained to explicit transfer-ownership wrappers.");
    }

    [Test]
    public void CatchAllExceptionUsageIsConstrainedToCallbackBoundaryAdapter()
    {
        var extensionsRoot = GetExtensionsRoot();
        var allowedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Path.Combine("Problems", "ProblemCallbackAdapter.cs"),
            Path.Combine("Algorithms", "AlgorithmCallbackAdapter.cs")
        };

        var offenders = Directory
            .EnumerateFiles(extensionsRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => Regex.IsMatch(File.ReadAllText(path), @"catch\s*\(\s*Exception\b", RegexOptions.Multiline))
            .Where(path => !allowedFiles.Contains(Path.GetRelativePath(extensionsRoot, path)))
            .Select(path => Path.GetRelativePath(extensionsRoot, path))
            .ToArray();

        Assert.That(
            offenders,
            Is.Empty,
            "catch (Exception) should remain constrained to the managed callback boundary adapter where deferred bubbling is intentionally implemented.");
    }

    private static string GetExtensionsRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null)
        {
            var candidate = Path.Combine(current.FullName, "pagmoSharp", "pagmoExtensions");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate pagmoSharp/pagmoExtensions from test runtime directory.");
    }
}
