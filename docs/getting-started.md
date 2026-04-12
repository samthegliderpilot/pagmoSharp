# Getting Started

## Goal

Run a first optimization and read the result end-to-end.

## Runnable Source

- Example project: [Examples.PagmoSharp.csproj](..\Examples\Examples.PagmoSharp\Examples.PagmoSharp.csproj)
- Scenario implementation: [Program.cs](..\Examples\Examples.PagmoSharp\Program.cs) (`single`)
- Problem used in examples: [Program.cs](..\Examples\Examples.PagmoSharp\Program.cs) (`RastriginLikeProblem`)

## Run

From repo root:

```powershell
dotnet run --project Examples/Examples.PagmoSharp/Examples.PagmoSharp.csproj -- single
```

## What It Teaches

- How to create a managed problem (`ManagedProblemBase`).
- How to run one algorithm on one island (`island.Create(...)`).
- How to retrieve champion fitness/result from evolved population.

## Read Next

- [Islands, Archipelagos, Topology, Policies](archipelago-topology-policies.md)
