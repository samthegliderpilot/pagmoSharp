param(
    [string]$Configuration = "Debug",
    [string]$Platform = "x64"
)

$ErrorActionPreference = "Stop"

$mutexName = "Global\pagmoNet_swig_native_build"
$mutex = New-Object System.Threading.Mutex($false, $mutexName)
$hasLock = $false

try {
    $hasLock = $mutex.WaitOne([TimeSpan]::FromMinutes(30))
    if (-not $hasLock) {
        throw "Timed out waiting for build/SWIG lock '$mutexName'."
    }

    $repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
    $wrapperDir = Join-Path $repoRoot "pagmoWrapper"

    if ($IsLinux -or $IsMacOS) {
        # CMake + vcpkg path for Linux/macOS.
        # Uses the x64-linux-static-pic triplet to build pagmo2 and its dependencies
        # (Boost.Serialization, TBB) as static PIC libraries, so libPagmoWrapper.so
        # has no external runtime dependencies beyond the standard C++ runtime.

        if (-not $env:VCPKG_ROOT) {
            throw "VCPKG_ROOT must be set for Linux/macOS builds. " +
                  "Clone vcpkg (https://github.com/microsoft/vcpkg) and set VCPKG_ROOT."
        }
        $vcpkgExe       = Join-Path $env:VCPKG_ROOT "vcpkg"
        $vcpkgToolchain = Join-Path $env:VCPKG_ROOT "scripts/buildsystems/vcpkg.cmake"
        $triplet        = "x64-linux-static-pic"
        $tripletOverlay = (Resolve-Path (Join-Path $repoRoot "triplets")).Path

        if (-not (Test-Path $vcpkgExe)) {
            throw "vcpkg executable not found at '$vcpkgExe'. Run bootstrap-vcpkg.sh first."
        }

        # Install pagmo2 with the static-PIC triplet (idempotent — skips if already installed).
        Write-Host "==> vcpkg install pagmo2:$triplet"
        & $vcpkgExe install "pagmo2:$triplet" "--overlay-triplets=$tripletOverlay"
        if ($LASTEXITCODE -ne 0) { throw "vcpkg install failed ($LASTEXITCODE)." }

        $buildDir = Join-Path $wrapperDir "build"

        & cmake `
            "-B$buildDir" `
            "-S$wrapperDir" `
            "-DCMAKE_BUILD_TYPE=$Configuration" `
            "-DCMAKE_TOOLCHAIN_FILE=$vcpkgToolchain" `
            "-DVCPKG_TARGET_TRIPLET=$triplet" `
            "-DVCPKG_OVERLAY_TRIPLETS=$tripletOverlay"
        if ($LASTEXITCODE -ne 0) { throw "cmake configure failed ($LASTEXITCODE)." }

        & cmake --build $buildDir --config $Configuration
        if ($LASTEXITCODE -ne 0) { throw "cmake build failed ($LASTEXITCODE)." }
    } else {
        # Windows: MSBuild path
        $vsWhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
        if (-not (Test-Path $vsWhere)) {
            throw "vswhere.exe was not found. Install Visual Studio Build Tools 2022."
        }

        $msbuildExe = & $vsWhere -latest -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
        if (-not $msbuildExe) {
            throw "MSBuild.exe was not found. Install Visual Studio Build Tools 2022."
        }

        & $msbuildExe (Join-Path $wrapperDir "pagmoWrapper.vcxproj") /m /p:Configuration=$Configuration /p:Platform=$Platform
        if ($LASTEXITCODE -ne 0) { throw "MSBuild failed ($LASTEXITCODE)." }
    }
}
finally {
    if ($hasLock) {
        $mutex.ReleaseMutex()
    }
    $mutex.Dispose()
}
