using System.Reflection;
using System.Text;

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: ApiDocGen <assemblyPath> <outputPath>");
    return 2;
}

var assemblyPath = Path.GetFullPath(args[0]);
var outputPath = Path.GetFullPath(args[1]);

if (!File.Exists(assemblyPath))
{
    Console.Error.WriteLine($"Assembly not found: {assemblyPath}");
    return 3;
}

var assembly = Assembly.LoadFrom(assemblyPath);
var types = assembly.GetExportedTypes()
    .Where(t => t.Namespace == "pagmo")
    .OrderBy(t => t.FullName, StringComparer.Ordinal)
    .ToArray();

var lines = new List<string>
{
    "# API Reference (Generated)",
    "",
    "This document enumerates the public API visible to consumers of pagmoSharp.",
    "",
    $"- Generated from assembly: {assemblyPath}",
    "- Upstream pagmo C++ API index: https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html",
    $"- Generation timestamp (UTC): {DateTime.UtcNow:u}",
    ""
};

const BindingFlags publicDeclared = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

foreach (var type in types)
{
    var typeKind = type.IsInterface ? "interface" : type.IsEnum ? "enum" : type.IsValueType ? "struct" : "class";
    lines.Add($"## {type.FullName}");
    lines.Add("");
    lines.Add($"- Kind: {typeKind}");
    lines.Add($"- Upstream reference: {GetPagmoReference(type.Name)}");
    lines.Add("");

    if (type.IsEnum)
    {
        lines.Add("### Values");
        lines.Add("");
        foreach (var name in Enum.GetNames(type))
        {
            lines.Add($"- {name}");
        }
        lines.Add("");
        continue;
    }

    var ctors = type.GetConstructors(publicDeclared)
        .OrderBy(c => c.ToString(), StringComparer.Ordinal)
        .ToArray();
    if (ctors.Length > 0)
    {
        lines.Add("### Constructors");
        lines.Add("");
        foreach (var ctor in ctors)
        {
            var parameters = string.Join(", ", ctor.GetParameters().Select(FormatParameter));
            lines.Add($"- `{type.Name}({parameters})`");
        }
        lines.Add("");
    }

    var properties = type.GetProperties(publicDeclared)
        .OrderBy(p => p.Name, StringComparer.Ordinal)
        .ToArray();
    if (properties.Length > 0)
    {
        lines.Add("### Properties");
        lines.Add("");
        foreach (var property in properties)
        {
            var accessors = new List<string>(2);
            if (property.CanRead) accessors.Add("get");
            if (property.CanWrite) accessors.Add("set");
            lines.Add($"- `{GetSimpleTypeName(property.PropertyType)} {property.Name} {{ {string.Join("; ", accessors)} }}`");
        }
        lines.Add("");
    }

    var methods = type.GetMethods(publicDeclared)
        .Where(m => !m.IsSpecialName)
        .OrderBy(m => m.Name, StringComparer.Ordinal)
        .ThenBy(m => m.GetParameters().Length)
        .ToArray();
    if (methods.Length > 0)
    {
        lines.Add("### Methods");
        lines.Add("");
        foreach (var method in methods)
        {
            var parameters = string.Join(", ", method.GetParameters().Select(FormatParameter));
            var staticPrefix = method.IsStatic ? "static " : string.Empty;
            lines.Add($"- `{staticPrefix}{GetSimpleTypeName(method.ReturnType)} {method.Name}({parameters})`");
        }
        lines.Add("");
    }
}

Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
File.WriteAllLines(outputPath, lines, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
Console.WriteLine($"Generated API reference: {outputPath}");
return 0;

static string FormatParameter(ParameterInfo parameter)
{
    var modifier = parameter.IsOut ? "out " : parameter.ParameterType.IsByRef ? "ref " : string.Empty;
    var parameterType = parameter.ParameterType.IsByRef ? parameter.ParameterType.GetElementType()! : parameter.ParameterType;
    return $"{modifier}{GetSimpleTypeName(parameterType)} {parameter.Name}";
}

static string GetSimpleTypeName(Type type)
{
    if (type.IsGenericType)
    {
        var name = type.Name[..type.Name.IndexOf('`')];
        var args = string.Join(", ", type.GetGenericArguments().Select(GetSimpleTypeName));
        return $"{name}<{args}>";
    }

    if (type.IsArray)
    {
        return $"{GetSimpleTypeName(type.GetElementType()!)}[]";
    }

    return Type.GetTypeCode(type) switch
    {
        TypeCode.Boolean => "bool",
        TypeCode.Byte => "byte",
        TypeCode.SByte => "sbyte",
        TypeCode.Int16 => "short",
        TypeCode.UInt16 => "ushort",
        TypeCode.Int32 => "int",
        TypeCode.UInt32 => "uint",
        TypeCode.Int64 => "long",
        TypeCode.UInt64 => "ulong",
        TypeCode.Single => "float",
        TypeCode.Double => "double",
        TypeCode.Decimal => "decimal",
        TypeCode.String => "string",
        _ when type == typeof(void) => "void",
        _ when type == typeof(object) => "object",
        _ => type.Name
    };
}

static string GetPagmoReference(string typeName)
{
    var coreMap = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["problem"] = "https://esa.github.io/pagmo2/docs/cpp/problem.html",
        ["algorithm"] = "https://esa.github.io/pagmo2/docs/cpp/algorithm.html",
        ["population"] = "https://esa.github.io/pagmo2/docs/cpp/population.html",
        ["island"] = "https://esa.github.io/pagmo2/docs/cpp/island.html",
        ["archipelago"] = "https://esa.github.io/pagmo2/docs/cpp/archipelago.html",
        ["bfe"] = "https://esa.github.io/pagmo2/docs/cpp/bfe.html",
        ["topology"] = "https://esa.github.io/pagmo2/docs/cpp/topology.html",
        ["r_policy"] = "https://esa.github.io/pagmo2/docs/cpp/r__policy.html",
        ["s_policy"] = "https://esa.github.io/pagmo2/docs/cpp/s__policy.html",
        ["pagmo"] = "https://esa.github.io/pagmo2/docs/cpp/utils/multi_objective.html",
    };
    if (coreMap.TryGetValue(typeName, out var coreUrl))
    {
        return coreUrl;
    }

    string[] problemTypes =
    [
        "ackley","cec2006","cec2009","cec2013","cec2014","decompose","dtlz","golomb_ruler","griewank",
        "hock_schittkowski_71","inventory","lennard_jones","luksan_vlcek1","minlp_rastrigin","null_problem",
        "rastrigin","rosenbrock","schwefel","translate","unconstrain","wfg","zdt"
    ];
    if (problemTypes.Contains(typeName, StringComparer.Ordinal))
    {
        return $"https://esa.github.io/pagmo2/docs/cpp/problems/{typeName}.html";
    }

    string[] algorithmTypes =
    [
        "bee_colony","cmaes","compass_search","cstrs_self_adaptive","de","de1220","gaco","gwo","ihs","ipopt","maco",
        "mbh","moead","moead_gen","nsga2","nspso","null_algorithm","pso","pso_gen","sade","sea","sga",
        "simulated_annealing","xnes","nlopt"
    ];
    if (algorithmTypes.Contains(typeName, StringComparer.Ordinal))
    {
        return $"https://esa.github.io/pagmo2/docs/cpp/algorithms/{typeName}.html";
    }

    string[] topologyTypes = ["unconnected", "fully_connected", "ring", "free_form"];
    if (topologyTypes.Contains(typeName, StringComparer.Ordinal))
    {
        return $"https://esa.github.io/pagmo2/docs/cpp/topologies/{typeName}.html";
    }

    string[] bfeTypes = ["default_bfe", "thread_bfe", "member_bfe"];
    if (bfeTypes.Contains(typeName, StringComparer.Ordinal))
    {
        return $"https://esa.github.io/pagmo2/docs/cpp/batch_evaluators/{typeName}.html";
    }

    return "https://esa.github.io/pagmo2/docs/cpp/cpp_docs.html";
}

