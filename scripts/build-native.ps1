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
        # CMake path for Linux/macOS
        $buildDir = Join-Path $wrapperDir "build"

        $cmakeArgs = @(
            "-B", $buildDir,
            "-S", $wrapperDir,
            "-DCMAKE_BUILD_TYPE=$Configuration"
        )

        # Use vcpkg toolchain if available
        $vcpkgToolchain = $env:VCPKG_ROOT `
            ? (Join-Path $env:VCPKG_ROOT "scripts/buildsystems/vcpkg.cmake") `
            : $null
        if ($vcpkgToolchain -and (Test-Path $vcpkgToolchain)) {
            $cmakeArgs += "-DCMAKE_TOOLCHAIN_FILE=$vcpkgToolchain"
        }

        & cmake @cmakeArgs
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
