using System;
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
