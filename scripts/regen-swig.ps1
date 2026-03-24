param()

$ErrorActionPreference = "Stop"

$mutexName = "Global\pagmoSharp_swig_native_build"
$mutex = New-Object System.Threading.Mutex($false, $mutexName)
$hasLock = $false

try {
    $hasLock = $mutex.WaitOne([TimeSpan]::FromMinutes(15))
    if (-not $hasLock) {
        throw "Timed out waiting for build/SWIG lock '$mutexName'."
    }

    $repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
    Push-Location $repoRoot
    try {
        cmd /c createSwigWrappersAndPlaceThem.bat
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
