# Release Readiness Checklist (v1.0)

## Versioning
- [x] Decide release version (SemVer) and tag name. (`1.0.0-beta.1`)
- [x] Update package version in all publish targets.
- [x] Freeze API surface changes for release candidate.

## Build & Test Gates

### Windows
- [x] Run consolidated release gates script: `pwsh scripts/release-gates.ps1` (or `powershell -ExecutionPolicy Bypass -File scripts/release-gates.ps1`).
- [x] Ensure release gate runs with repo-local cache environment (`NUGET_PACKAGES=./.nuget/packages`) to avoid machine-profile package drift.
- [x] Clean rebuild native wrapper (`Debug x64` and `Release x64`).
- [x] Run full managed test suite (`dotnet test`) — 593/593 pass.
- [x] Validate SWIG regen reproducibility (no unexpected diffs after regeneration).
- [x] Verify optional solver availability tests pass in current build profile.

### Linux (Ubuntu 24.04 / Linux Mint 22.1 verified)
- [x] Install prerequisites: `sudo apt install -y cmake build-essential swig libpagmo-dev dotnet-sdk-10.0 powershell`
- [x] Build native library: `cmake -B pagmoWrapper/build -S pagmoWrapper -DCMAKE_BUILD_TYPE=Release && cmake --build pagmoWrapper/build`
- [x] Run full managed test suite: `dotnet test Tests/Tests.Pagmo.NET/Tests.Pagmo.NET.csproj -p:Platform=x64` — 593/593 pass.
- [x] Verify optional solver tests self-exclude cleanly (NLopt/IPOPT not present in apt pagmo — tests call `Assert.Pass` via `OptionalSolverAvailability`).

## Artifacts

### Windows
- [x] Produce managed assembly outputs (`Pagmo.NET.dll` + `.snupkg` symbols).
- [x] Produce native runtime artifacts (`pagmoWrapper.dll` and dependencies) via `scripts/build-release-artifacts.ps1`.
- [x] Verify artifact folder structure and load-path assumptions.
- [x] Confirm license/NOTICE inclusion for redistributed dependencies. (`THIRD_PARTY_LICENSES.md`)

### Linux
- [ ] Run `pwsh scripts/build-release-artifacts.ps1 -Version 1.0.0-beta.1` on Linux to produce `Pagmo.NET-native-linux-x64-{version}.zip` containing `libPagmoWrapper.so`.
- [ ] Verify NuGet pack on Linux includes `runtimes/linux-x64/native/libPagmoWrapper.so` (confirm with `unzip -l *.nupkg | grep linux`).
- [ ] Document Linux consumer system dependency: `sudo apt install libpagmo9t64` (runtime-only package, no dev headers needed).

## Documentation
- [x] Update README quickstart if release API differs from docs.
- [x] Update supported feature matrix for released build profile (Linux now listed as Supported).
- [x] Add migration notes for any compatibility aliases/deprecations.
- [x] Linux build and troubleshooting guide in `.ai/LINUX_TESTING_HANDOFF.md`.

## Changelog
- [x] Summarize major features delivered in v1.0. (`RELEASE_NOTES.md`)
- [x] Summarize breaking changes (if any). (`RELEASE_NOTES.md`)
- [ ] Update known limitations in `RELEASE_NOTES.md`: Linux is now supported; remove "Linux deferred post-v1" note. Add note that Linux consumers must install `libpagmo9t64` as a system dependency.

## Final Publish
- [ ] Create annotated git tag.
- [ ] Publish release notes.
- [ ] Upload release artifacts to GitHub Releases — both `Pagmo.NET-native-win-x64-{version}.zip` and `Pagmo.NET-native-linux-x64-{version}.zip` alongside the NuGet package.
- [ ] Post-release smoke test from fresh checkout (Windows and Linux).

---

## GitHub Actions — Cross-Platform Release Workflow (future)

The current release process requires manually running `scripts/build-release-artifacts.ps1`
on both a Windows and a Linux machine and uploading the resulting zips. Once GitHub Actions is
set up, this collapses to pushing a version tag.

Create `.github/workflows/release.yml` with the following structure:

```yaml
name: Release

on:
  push:
    tags: ['v*']          # trigger on v1.0.0-beta.1, v1.0.0, etc.
  workflow_dispatch:
    inputs:
      version:
        description: 'Version (e.g. 1.0.0-beta.1)'
        required: true

jobs:
  # ── Native build: runs in parallel on both platforms ──────────────────────
  build-native:
    strategy:
      matrix:
        include:
          - os: windows-latest
            rid: win-x64
          - os: ubuntu-24.04
            rid: linux-x64

    runs-on: ${{ matrix.os }}

    steps:
      - uses: actions/checkout@v4

      - name: Install prerequisites (Linux)
        if: runner.os == 'Linux'
        run: |
          # Pin .NET packages to Ubuntu's repo to avoid conflicts
          sudo tee /etc/apt/preferences.d/dotnet-ubuntu << 'EOF'
          Package: dotnet* aspnetcore*
          Pin: release o=Ubuntu
          Pin-Priority: 1001
          EOF
          sudo apt-get update
          sudo apt-get install -y cmake build-essential swig libpagmo-dev dotnet-sdk-10.0 powershell

      - name: Install prerequisites (Windows)
        if: runner.os == 'Windows'
        run: |
          # vcpkg is pre-installed on windows-latest; install pagmo2
          vcpkg install pagmo2:x64-windows
          # Adjust VCPKG_INSTALLED_DIR env var if needed for the .vcxproj

      - name: Set up .NET (Windows)
        if: runner.os == 'Windows'
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.x'

      - name: Determine version
        id: version
        shell: pwsh
        run: |
          $v = if ('${{ github.event.inputs.version }}') {
                 '${{ github.event.inputs.version }}'
               } else {
                 '${{ github.ref_name }}' -replace '^v', ''
               }
          echo "VERSION=$v" >> $env:GITHUB_OUTPUT

      - name: Build and package
        shell: pwsh
        run: |
          pwsh scripts/build-release-artifacts.ps1 `
            -Version ${{ steps.version.outputs.VERSION }} `
            -SkipReleaseGates

      - name: Upload artifacts
        uses: actions/upload-artifact@v4
        with:
          name: release-${{ matrix.rid }}
          path: artifacts/release/

  # ── Publish: merges both platform artifacts and creates the GitHub Release ─
  publish:
    needs: build-native
    runs-on: ubuntu-24.04

    steps:
      - uses: actions/download-artifact@v4
        with:
          name: release-win-x64
          path: release-win

      - uses: actions/download-artifact@v4
        with:
          name: release-linux-x64
          path: release-linux

      - name: Determine version
        id: version
        run: |
          VERSION=${{ github.event.inputs.version || github.ref_name }}
          echo "VERSION=${VERSION#v}" >> $GITHUB_OUTPUT

      - name: Merge and publish to GitHub Releases
        uses: softprops/action-gh-release@v2
        with:
          tag_name: v${{ steps.version.outputs.VERSION }}
          name: Pagmo.NET v${{ steps.version.outputs.VERSION }}
          draft: true       # review before making public
          files: |
            release-win/*/nuget/*.nupkg
            release-win/*/nuget/*.snupkg
            release-win/*/Pagmo.NET-native-win-x64-*.zip
            release-linux/*/Pagmo.NET-native-linux-x64-*.zip
            release-win/*/SHA256SUMS.txt
```

### Notes on the workflow

**NuGet merge:** The Windows job packs a NuGet with `runtimes/win-x64/native/` and the Linux
job packs one with `runtimes/linux-x64/native/`. For a single fully cross-platform `.nupkg`
you would need an extra merge step — but for a beta, publishing both separately as GitHub
release assets is acceptable. Consumers can download the managed NuGet from NuGet.org and the
appropriate native bundle from the GitHub release.

**vcpkg on Windows:** The `windows-latest` runner has vcpkg pre-installed but the pagmo2
vcpkg port and include/lib paths may need aligning with what `pagmoWrapper.vcxproj` expects.
Set `VCPKG_INSTALLED_DIR` as an environment variable or pass `PagmoVcpkgInstalledDir` as an
MSBuild property if the default fallback path doesn't match the runner's vcpkg location.

**NuGet publish:** Add a separate step to push `.nupkg` to NuGet.org using
`dotnet nuget push` with `NUGET_API_KEY` stored as a GitHub secret. Only push from the
publish job after the draft release is reviewed.

**Release gates:** `build-release-artifacts.ps1` is called with `-SkipReleaseGates` in CI
because the gates (SWIG regen reproducibility, full test suite) should be run in a dedicated
pre-release CI job on every PR/push to main, not as part of the release packaging job.
Add a `ci.yml` workflow that runs `scripts/release-gates.ps1` on push to `main` for that.
