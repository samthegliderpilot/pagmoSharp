param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"

$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH = "0"
$env:DOTNET_CLI_HOME = Join-Path $PSScriptRoot "..\.dotnet"

$project = "Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj"
$scenarios = @("single", "archipelago", "policies")

if (-not (Test-Path $env:DOTNET_CLI_HOME)) {
    New-Item -ItemType Directory -Path $env:DOTNET_CLI_HOME | Out-Null
}

foreach ($scenario in $scenarios) {
    Write-Host "Running docs scenario: $scenario"
    dotnet run --project $project -c $Configuration -- $scenario
    if ($LASTEXITCODE -ne 0) {
        throw "Documentation smoke scenario '$scenario' failed with exit code $LASTEXITCODE."
    }
}

Write-Host "Documentation smoke checks passed."
