# Release Readiness Checklist (v1.0)

## Versioning
- [x] Decide release version (SemVer) and tag name. (`1.0.0-beta.1`)
- [x] Update package version in all publish targets.
- [x] Freeze API surface changes for release candidate.

## Build & Test Gates
- [x] Run consolidated release gates script: `powershell -ExecutionPolicy Bypass -File scripts/release-gates.ps1`.
- [x] Ensure release gate runs with repo-local cache environment (`NUGET_PACKAGES=./.nuget/packages`) to avoid machine-profile package drift.
- [x] Clean rebuild native wrapper (`Debug x64` and `Release x64`).
- [x] Run full managed test suite (`dotnet test`) — 589/589 pass.
- [x] Validate SWIG regen reproducibility (no unexpected diffs after regeneration).
- [x] Verify optional solver availability tests pass in current build profile.

## Artifacts
- [x] Produce managed assembly outputs (`pagmoSharp.dll` + symbols as needed).
- [x] Produce native runtime artifacts (`pagmoWrapper.dll` and dependencies).
- [x] Verify artifact folder structure and load-path assumptions.
- [x] Confirm license/NOTICE inclusion for redistributed dependencies. (THIRD_PARTY_LICENSES.md)

## Documentation
- [x] Update README quickstart if release API differs from docs.
- [x] Update supported feature matrix for released build profile.
- [x] Add migration notes for any compatibility aliases/deprecations.

## Changelog
- [x] Summarize major features delivered in v1.0. (RELEASE_NOTES.md)
- [x] Summarize breaking changes (if any). (RELEASE_NOTES.md)
- [x] Summarize known limitations/deferred items (Linux/CMake, thread-clone strategy, etc.). (RELEASE_NOTES.md)

## Final Publish
- [ ] Create annotated git tag.
- [ ] Publish release notes.
- [ ] Upload release artifacts to target distribution channel(s).
- [ ] Post-release smoke test from fresh checkout.
