# Examples.Pagmo.NET

Runnable, non-test examples that teach both:
- how to use Pagmo.NET APIs, and
- why optimization structures like islands, archipelagos, topology, and policies matter.

Concept-first walkthrough pages that reference these examples live in:
- `docs/getting-started.md`
- `docs/archipelago-topology-policies.md`

## Run

From repo root:

```powershell
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- all
```

Scenario options:

```powershell
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- single
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- archipelago
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- policies
```

## What each scenario demonstrates

- `single`: baseline optimization on one island.
- `archipelago`: teaches topology connectivity intuition (`ring` vs `unconnected`) and compares single-island vs archipelago multi-start search results.
- `policies`: compares default policy wiring against explicit `fair_replace` + `select_best` policy wiring through archipelago APIs.

Known limitation: archipelago runtime topology replacement (set_topology_* followed by evolve/wait_check) has a tracked issue for some paths; see roadmap for planned fix.

