param(
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"

$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_CLI_HOME = Join-Path $PSScriptRoot "..\.dotnet"

if (-not (Test-Path $env:DOTNET_CLI_HOME)) {
    New-Item -ItemType Directory -Path $env:DOTNET_CLI_HOME | Out-Null
}

dotnet restore "Tests\Tests.PagmoSharp\Tests.PagmoSharp.csproj"
if ($LASTEXITCODE -ne 0) { throw "dotnet restore failed ($LASTEXITCODE)." }

dotnet build "Tests\Tests.PagmoSharp\Tests.PagmoSharp.csproj" -c $Configuration -p:Platform=x64 --no-restore
if ($LASTEXITCODE -ne 0) { throw "dotnet build failed ($LASTEXITCODE)." }

dotnet test "Tests\Tests.PagmoSharp\Tests.PagmoSharp.csproj" -c $Configuration -p:Platform=x64 --no-build
if ($LASTEXITCODE -ne 0) { throw "dotnet test failed ($LASTEXITCODE)." }
