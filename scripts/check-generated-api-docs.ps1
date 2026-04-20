param(
    [string]$Configuration = "Debug",
    [string]$Framework = "net8.0"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$env:DOTNET_CLI_HOME = Join-Path $repoRoot ".dotnet"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"

$assemblyPath = Join-Path $repoRoot "Pagmo.NET/bin/$Configuration/$Framework/Pagmo.NET.dll"
$apiDocPath = Join-Path $repoRoot "docs/api-reference.md"
$toolProjectPath = Join-Path $repoRoot "scripts/ApiDocGen/ApiDocGen.csproj"
$mutexName = "Global\pagmoNet_dotnet_library_build"
$mutex = New-Object System.Threading.Mutex($false, $mutexName)
$hasLock = $false

try {
    $hasLock = $mutex.WaitOne([TimeSpan]::FromMinutes(15))
    if (-not $hasLock) {
        throw "Timed out waiting for dotnet build lock '$mutexName'."
    }

    if (-not (Test-Path $assemblyPath)) {
        dotnet build (Join-Path $repoRoot "Pagmo.NET/Pagmo.NET.csproj") -c $Configuration -f $Framework | Out-Null
    }

    dotnet run --project $toolProjectPath --configuration $Configuration -- $assemblyPath $apiDocPath --verify | Out-Null
    Write-Host "Generated API docs coverage check passed."
}
finally {
    if ($hasLock) {
        $mutex.ReleaseMutex()
    }
    $mutex.Dispose()
}
