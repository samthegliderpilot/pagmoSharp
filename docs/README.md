# PagmoSharp Docs

This documentation is executable-first:
- Concepts are explained in docs pages.
- Every page links to runnable code in `Examples/Examples.PagmoSharp`.
- A smoke runner verifies documented scenarios stay working.

## Walkthroughs

- [Getting Started](getting-started.md)
- [Islands, Archipelagos, Topology, Policies](archipelago-topology-policies.md)

## Drift Guard

Run the documented scenario smoke checks from repo root:

```powershell
powershell -ExecutionPolicy Bypass -File scripts/docs-smoke.ps1
```

This runs:
- `single`
- `archipelago`
- `policies`

against [Program.cs](..\Examples\Examples.PagmoSharp\Program.cs).
