# Release Readiness Checklist (v1.0)

## Versioning
- [ ] Decide release version (SemVer) and tag name.
- [ ] Update package version in all publish targets.
- [ ] Freeze API surface changes for release candidate.

## Build & Test Gates
- [ ] Clean rebuild native wrapper (`Debug x64` and `Release x64`).
- [ ] Run full managed test suite (`dotnet test`) and record pass count.
- [ ] Validate SWIG regen reproducibility (no unexpected diffs after regeneration).
- [ ] Verify optional solver availability tests pass in current build profile.

## Artifacts
- [ ] Produce managed assembly outputs (`pagmoSharp.dll` + symbols as needed).
- [ ] Produce native runtime artifacts (`pagmoWrapper.dll` and dependencies).
- [ ] Verify artifact folder structure and load-path assumptions.
- [ ] Confirm license/NOTICE inclusion for redistributed dependencies.

## Documentation
- [ ] Update README quickstart if release API differs from docs.
- [ ] Update supported feature matrix for released build profile.
- [ ] Add migration notes for any compatibility aliases/deprecations.

## Changelog
- [ ] Summarize major features delivered in v1.0.
- [ ] Summarize breaking changes (if any).
- [ ] Summarize known limitations/deferred items (Linux/CMake, thread-clone strategy, etc.).

## Final Publish
- [ ] Create annotated git tag.
- [ ] Publish release notes.
- [ ] Upload release artifacts to target distribution channel(s).
- [ ] Post-release smoke test from fresh checkout.
