param()

$ErrorActionPreference = "Stop"

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
