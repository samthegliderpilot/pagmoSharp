param(
    [string]$Configuration = "Debug",
    [ValidateSet("all", "build", "test")]
    [string]$Stage = "all",
    [switch]$NoRestore
)

$ErrorActionPreference = "Stop"

$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH = "0"
$env:DOTNET_CLI_HOME = Join-Path $PSScriptRoot "..\.dotnet"
$env:NUGET_PACKAGES = Join-Path $PSScriptRoot "..\.nuget\packages"
$baseOutputPath = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path + "\artifacts\dotnet\"
$baseIntermediatePath = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path + "\artifacts\obj\"
$dotnetProps = @("-p:Platform=x64", "-p:BaseOutputPath=$baseOutputPath", "-p:BaseIntermediateOutputPath=$baseIntermediatePath")

if (-not (Test-Path $env:DOTNET_CLI_HOME)) {
    New-Item -ItemType Directory -Path $env:DOTNET_CLI_HOME | Out-Null
}
if (-not (Test-Path $baseOutputPath)) {
    New-Item -ItemType Directory -Path $baseOutputPath | Out-Null
}
if (-not (Test-Path $baseIntermediatePath)) {
    New-Item -ItemType Directory -Path $baseIntermediatePath | Out-Null
}

$testProject = "Tests\Tests.PagmoSharp\Tests.PagmoSharp.csproj"

function Invoke-DotnetWithRetry {
    param(
        [string[]]$DotnetArgs,
        [string]$FailureLabel,
        [int]$MaxAttempts = 3
    )

    for ($attempt = 1; $attempt -le $MaxAttempts; $attempt++) {
        dotnet @DotnetArgs
        $exitCode = $LASTEXITCODE
        if ($exitCode -eq 0) {
            return
        }

        if ($attempt -lt $MaxAttempts) {
            Start-Sleep -Seconds 1
        } else {
            throw "$FailureLabel failed ($exitCode)."
        }
    }
}

if (($Stage -eq "all" -or $Stage -eq "build") -and -not $NoRestore) {
    Invoke-DotnetWithRetry -DotnetArgs (@("restore", $testProject) + $dotnetProps) -FailureLabel "dotnet restore"
}

if ($Stage -eq "all" -or $Stage -eq "build") {
    Invoke-DotnetWithRetry -DotnetArgs (@("build", $testProject, "-c", $Configuration, "--no-restore") + $dotnetProps) -FailureLabel "dotnet build"
}

if ($Stage -eq "all" -or $Stage -eq "test") {
    Invoke-DotnetWithRetry -DotnetArgs (@("test", $testProject, "-c", $Configuration, "--no-build") + $dotnetProps) -FailureLabel "dotnet test"
}
