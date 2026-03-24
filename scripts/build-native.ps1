param(
    [string]$Configuration = "Debug",
    [string]$Platform = "x64"
)

$ErrorActionPreference = "Stop"

$vsWhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
if (-not (Test-Path $vsWhere)) {
    throw "vswhere.exe was not found. Install Visual Studio Build Tools 2022."
}

$msbuildExe = & $vsWhere -latest -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
if (-not $msbuildExe) {
    throw "MSBuild.exe was not found. Install Visual Studio Build Tools 2022."
}

$mutexName = "Global\pagmoSharp_swig_native_build"
$mutex = New-Object System.Threading.Mutex($false, $mutexName)
$hasLock = $false

try {
    $hasLock = $mutex.WaitOne([TimeSpan]::FromMinutes(30))
    if (-not $hasLock) {
        throw "Timed out waiting for build/SWIG lock '$mutexName'."
    }

    & $msbuildExe "pagmoWrapper\pagmoWrapper.vcxproj" /m /p:Configuration=$Configuration /p:Platform=$Platform
}
finally {
    if ($hasLock) {
        $mutex.ReleaseMutex()
    }
    $mutex.Dispose()
}
