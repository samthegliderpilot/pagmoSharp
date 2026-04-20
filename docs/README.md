# Pagmo.NET Docs

This documentation is executable-first:
- Concepts are explained in docs pages.
- Every page links to runnable code in `Examples/Examples.Pagmo.NET`.
- A smoke runner verifies documented scenarios stay working.

## Walkthroughs

- [Getting Started](getting-started.md)
- [Islands, Archipelagos, Topology, Policies](archipelago-topology-policies.md)
- [API Reference (Generated)](api-reference.md)

## API Reference Generation

The API reference is generated from the compiled `Pagmo.NET` assembly so it covers
all public symbols visible to consumers (including generated wrapper surfaces).

From repo root:

```powershell
powershell -ExecutionPolicy Bypass -File scripts/generate-api-reference.ps1
```

For handwritten wrapper APIs (`pagmoExtensions/*`) we also enforce XML-doc coverage:

```powershell
powershell -ExecutionPolicy Bypass -File scripts/check-handwritten-api-docs.ps1
```

For full consumer-visible coverage (all public types and all public/protected members),
run the generated API docs coverage gate:

```powershell
powershell -ExecutionPolicy Bypass -File scripts/check-generated-api-docs.ps1
```

## Drift Guard

Run the documented scenario smoke checks from repo root:

```powershell
powershell -ExecutionPolicy Bypass -File scripts/docs-smoke.ps1
```

This runs:
- `single`
- `archipelago`
- `policies`

against [Program.cs](..\Examples\Examples.Pagmo.NET\Program.cs).
