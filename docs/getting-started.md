# Getting Started

## Goal

Run a first optimization and read the result end-to-end.

## Runnable Source

- Example project: [Examples.Pagmo.NET.csproj](..\Examples\Examples.Pagmo.NET\Examples.Pagmo.NET.csproj)
- Scenario implementation: [Program.cs](..\Examples\Examples.Pagmo.NET\Program.cs) (`single`)
- Problem used in examples: [Program.cs](..\Examples\Examples.Pagmo.NET\Program.cs) (`RastriginLikeProblem`)

## Run

From repo root:

```powershell
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- single
```

## What It Teaches

- How to create a managed problem (`ManagedProblemBase`).
- How to run one algorithm on one island (`island.Create(...)`).
- How to retrieve champion fitness/result from evolved population.

## Read Next

- [Islands, Archipelagos, Topology, Policies](archipelago-topology-policies.md)
