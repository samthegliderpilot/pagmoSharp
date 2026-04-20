# Islands, Archipelagos, Topology, Policies

## Goal

Understand why archipelago structures can help optimization quality, not just how to call APIs.

## Runnable Source

- Scenario implementation: [Program.cs](..\Examples\Examples.Pagmo.NET\Program.cs)
- Archipelago scenario entry: `RunArchipelagoTeachingScenario()`
- Policy scenario entry: `RunPolicyComparison()`

## Run Archipelago Scenario

```powershell
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- archipelago
```

What to look for:
- Topology connectivity difference (`ring` vs `unconnected` neighbor counts).
- Single-island vs multi-island best fitness comparison.

## Run Policy Scenario

```powershell
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- policies
```

What to look for:
- Default policy flow vs explicit `fair_replace` + `select_best`.
- Migration log counts and resulting best-fitness behavior.

## Why This Structure Helps

- Islands create multiple search trajectories in parallel.
- Topology controls how quickly information spreads.
- Selection/replacement policies tune exploration vs exploitation.

## Drift Guard

To verify docs stay aligned with runnable behavior:

```powershell
powershell -ExecutionPolicy Bypass -File scripts/docs-smoke.ps1
```
