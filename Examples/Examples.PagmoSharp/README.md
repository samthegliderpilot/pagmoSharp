# Examples.PagmoSharp

Runnable, non-test examples that teach both:
- how to use pagmoSharp APIs, and
- why optimization structures like islands, archipelagos, topology, and policies matter.

## Run

From repo root:

```powershell
dotnet run --project Examples/Examples.PagmoSharp/Examples.PagmoSharp.csproj -- all
```

Scenario options:

```powershell
dotnet run --project Examples/Examples.PagmoSharp/Examples.PagmoSharp.csproj -- single
dotnet run --project Examples/Examples.PagmoSharp/Examples.PagmoSharp.csproj -- archipelago
dotnet run --project Examples/Examples.PagmoSharp/Examples.PagmoSharp.csproj -- policies
```

## What each scenario demonstrates

- `single`: baseline optimization on one island.
- `archipelago`: teaches topology connectivity intuition (`ring` vs `unconnected`) and compares single-island vs archipelago multi-start search results.
- `policies`: compares default policy wiring against explicit `fair_replace` + `select_best` policy wiring through archipelago APIs.

Known limitation: archipelago runtime topology replacement (set_topology_* followed by evolve/wait_check) has a tracked issue for some paths; see roadmap for planned fix.

