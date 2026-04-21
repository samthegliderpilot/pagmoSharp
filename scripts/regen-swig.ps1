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
    Push-Location $repoRoot
    try {
        if ($IsWindows -or (-not $IsLinux -and -not $IsMacOS)) {
            # Windows: prefer the PowerShell script; fall back to the .bat for
            # environments that don't have pwsh 7+ but do have cmd.exe.
            $ps1 = Join-Path $repoRoot "createSwigWrappersAndPlaceThem.ps1"
            if (Test-Path $ps1) {
                & pwsh -NoProfile -NonInteractive -ExecutionPolicy Bypass -File $ps1
                if ($LASTEXITCODE -ne 0) {
                    throw "createSwigWrappersAndPlaceThem.ps1 failed ($LASTEXITCODE)."
                }
            } else {
                $batPath = Join-Path $repoRoot "createSwigWrappersAndPlaceThem.bat"
                cmd /c "$batPath"
                if ($LASTEXITCODE -ne 0) {
                    throw "createSwigWrappersAndPlaceThem.bat failed ($LASTEXITCODE)."
                }
            }
        } else {
            # Linux / macOS
            $ps1 = Join-Path $repoRoot "createSwigWrappersAndPlaceThem.ps1"
            & pwsh -NoProfile -NonInteractive -ExecutionPolicy Bypass -File $ps1
            if ($LASTEXITCODE -ne 0) {
                throw "createSwigWrappersAndPlaceThem.ps1 failed ($LASTEXITCODE)."
            }
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
