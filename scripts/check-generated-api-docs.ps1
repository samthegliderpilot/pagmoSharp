param(
    [string]$Configuration = "Debug",
    [string]$Framework = "net10.0"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$env:DOTNET_CLI_HOME = Join-Path $repoRoot ".dotnet"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"

$assemblyPath = Join-Path $repoRoot "pagmoSharp/bin/$Configuration/$Framework/pagmoSharp.dll"
$apiDocPath = Join-Path $repoRoot "docs/api-reference.md"
$toolProjectPath = Join-Path $repoRoot "scripts/ApiDocGen/ApiDocGen.csproj"

if (-not (Test-Path $assemblyPath)) {
    dotnet build (Join-Path $repoRoot "pagmoSharp/pagmoSharp.csproj") -c $Configuration | Out-Null
}

dotnet run --project $toolProjectPath --configuration $Configuration -- $assemblyPath $apiDocPath --verify | Out-Null
Write-Host "Generated API docs coverage check passed."
