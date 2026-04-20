param()

$ErrorActionPreference = "Stop"

$mutexName = "Global\pagmoNet_swig_native_build"
$mutex = New-Object System.Threading.Mutex($false, $mutexName)
$hasLock = $false

try {
    $hasLock = $mutex.WaitOne([TimeSpan]::FromMinutes(15))
    if (-not $hasLock) {
        throw "Timed out waiting for build/SWIG lock '$mutexName'."
    }

    $repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
    $batPath = Join-Path $repoRoot "createSwigWrappersAndPlaceThem.bat"
    Push-Location $repoRoot
    try {
        # Use the full Windows path to the bat so cmd.exe can find it when
        # invoked from a POSIX shell (e.g. Git Bash) where relative paths
        # may not resolve correctly through cmd /c.
        cmd /c "$batPath"
        if ($LASTEXITCODE -ne 0) {
            throw "createSwigWrappersAndPlaceThem.bat failed ($LASTEXITCODE)."
        }
    }
    finally {
        Pop-Location
    }
}
finally {
    if ($hasLock) {
        $mutex.ReleaseMutex()
    }
    $mutex.Dispose()
}
