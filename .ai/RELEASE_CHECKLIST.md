# Release Readiness Checklist (v1.0)

## Versioning
- [x] Decide release version (SemVer) and tag name. (`1.0.0-beta.1`)
- [x] Update package version in all publish targets.
- [x] Freeze API surface changes for release candidate.

## Build & Test Gates

### Windows

The Windows release build uses cmake + vcpkg (`x64-windows-static-md` triplet) to produce a
**single self-contained `PagmoWrapper.dll`** with pagmo2, Boost, TBB, NLopt, and IPOPT statically
linked. No additional DLLs are required at runtime.

**Prerequisites:**
- Visual Studio 2022 (MSVC v143)
- vcpkg cloned and bootstrapped; `VCPKG_ROOT` set in the environment
- .NET 10 SDK

**Build:**
```
# First time (or after vcpkg updates): installs pagmo2[nlopt,ipopt] — takes ~30 min, cached thereafter
pwsh scripts/build-native.ps1 -Configuration Release

# Run release gates (SWIG reproducibility + native build + full managed test suite)
pwsh scripts/release-gates.ps1

# Produce artifacts (NuGet pack + native zip + source archive + checksums)
pwsh scripts/build-release-artifacts.ps1 -Version 1.0.0-beta.1 -SkipReleaseGates
```

- [x] Run consolidated release gates script: `pwsh scripts/release-gates.ps1`.
- [x] Ensure release gate runs with repo-local cache environment (`NUGET_PACKAGES=./.nuget/packages`) to avoid machine-profile package drift.
- [ ] Clean rebuild native wrapper via cmake (Debug and Release): `pwsh scripts/build-native.ps1 -Configuration Debug` then `-Configuration Release`.
- [x] Run full managed test suite (`dotnet test`) — 593/593 pass.
- [x] Validate SWIG regen reproducibility (no unexpected diffs after regeneration).
- [ ] Verify optional solver availability tests pass: NLopt and IPOPT should now be available (statically linked), so `IsNloptAvailable` and `IsIpoptAvailable` return `true`.

### Linux (Ubuntu 24.04 / Linux Mint 22.1)

The Linux release build also uses cmake + vcpkg (`x64-linux-static-pic` triplet) to produce a
**self-contained `libPagmoWrapper.so`** with all dependencies statically linked.

**Prerequisites:**
- `sudo apt install -y cmake build-essential swig dotnet-sdk-10.0 powershell`
- vcpkg cloned and bootstrapped; `VCPKG_ROOT` set in the environment
- See `.ai/LINUX_TESTING_HANDOFF.md` for full setup details.

**Build:**
```bash
# Install pagmo2[nlopt,ipopt] and build libPagmoWrapper.so (first time ~30 min, cached thereafter)
pwsh scripts/build-native.ps1 -Configuration Release

# Run full managed test suite
dotnet test Tests/Tests.Pagmo.NET/Tests.Pagmo.NET.csproj -p:Platform=x64

# Produce Linux artifacts
pwsh scripts/build-release-artifacts.ps1 -Version 1.0.0-beta.1 -SkipReleaseGates
```

- [ ] Install prerequisites (cmake, build-essential, swig, dotnet-sdk-10.0, powershell, vcpkg).
- [ ] Build native library via vcpkg + cmake: `pwsh scripts/build-native.ps1 -Configuration Release`.
- [ ] Run full managed test suite: `dotnet test Tests/Tests.Pagmo.NET/Tests.Pagmo.NET.csproj -p:Platform=x64` — 593/593 pass.
- [ ] Verify NLopt and IPOPT availability tests pass (`IsNloptAvailable` and `IsIpoptAvailable` return `true`).
- [ ] Confirm `ldd pagmoWrapper/build/libPagmoWrapper.so` shows no pagmo/boost/tbb/nlopt/ipopt deps.

## Artifacts

### Windows
- [x] Confirm license/NOTICE inclusion for redistributed dependencies. (`THIRD_PARTY_LICENSES.md`)
- [ ] Run `pwsh scripts/build-release-artifacts.ps1 -Version 1.0.0-beta.1` on Windows to produce:
  - `Pagmo.NET.1.0.0-beta.1.nupkg` (includes `runtimes/win-x64/native/PagmoWrapper.dll`)
  - `Pagmo.NET-native-win-x64-1.0.0-beta.1.zip` (the standalone DLL for manual deployment)
  - `SHA256SUMS.txt`
- [ ] Verify NuGet pack includes exactly one Windows DLL: `unzip -l *.nupkg | grep win-x64` should show `PagmoWrapper.dll` only (not multiple pagmo/boost/tbb DLLs).
- [ ] Verify the packed DLL has no dynamic pagmo/nlopt/ipopt deps: `dumpbin /dependents PagmoWrapper.dll` on the packed DLL should show only standard Windows/CRT DLLs.

### Linux
- [ ] Run `pwsh scripts/build-release-artifacts.ps1 -Version 1.0.0-beta.1` on Linux to produce `Pagmo.NET-native-linux-x64-{version}.zip` containing `libPagmoWrapper.so`.
- [ ] Verify NuGet pack on Linux includes `runtimes/linux-x64/native/libPagmoWrapper.so` (confirm with `unzip -l *.nupkg | grep linux`).
- [ ] Verify `ldd libPagmoWrapper.so` shows no pagmo/boost/tbb/nlopt/ipopt deps (all statically linked).
- [ ] No consumer system dependency required (`libpagmo9t64` is no longer needed — everything is self-contained).

## Documentation
- [x] Update README quickstart if release API differs from docs.
- [x] Update supported feature matrix for released build profile (Linux now listed as Supported).
- [x] Add migration notes for any compatibility aliases/deprecations.
- [x] Linux build and troubleshooting guide in `.ai/LINUX_TESTING_HANDOFF.md`.

## Changelog
- [x] Summarize major features delivered in v1.0. (`RELEASE_NOTES.md`)
- [x] Summarize breaking changes (if any). (`RELEASE_NOTES.md`)
- [ ] Update `RELEASE_NOTES.md`: confirm that NLopt and IPOPT are now fully available on both platforms (statically linked); update environment matrix to remove any remaining `libpagmo9t64` system dependency notes.

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
