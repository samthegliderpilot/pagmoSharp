param(
    [string[]]$NativeConfigurations = @("Debug", "Release"),
    [string]$Platform = "x64",
    [string]$DotnetConfiguration = "Debug",
    [switch]$SkipNativeBuild,
    [switch]$SkipSwigReproCheck,
    [switch]$SkipManagedTests,
    [switch]$NoRestore
)

$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")

function Invoke-CheckedCommand {
    param(
        [Parameter(Mandatory = $true)][string]$Name,
        [Parameter(Mandatory = $true)][scriptblock]$Command
    )

    Write-Host ""
    Write-Host "==> $Name"
    & $Command
}

function Get-SwigGeneratedFileHashes {
    $files = New-Object System.Collections.Generic.List[string]

    $knownFiles = @(
        "pagmoWrapper\GeneratedWrappers.cxx",
        "pagmoWrapper\PagmoNETSwigInterface_wrap.h",
        "Pagmo.NET\pygmoWrappers\pagmoPINVOKE.cs"
    )

    foreach ($relativePath in $knownFiles) {
        $fullPath = Join-Path $repoRoot $relativePath
        if (Test-Path $fullPath) {
            $files.Add($fullPath)
        }
    }

    $wrapperDir = Join-Path $repoRoot "Pagmo.NET\pygmoWrappers"
    if (Test-Path $wrapperDir) {
        Get-ChildItem -Path $wrapperDir -Filter "*.cs" -File | ForEach-Object {
            $files.Add($_.FullName)
        }
    }

    $hashMap = @{}
    foreach ($file in ($files | Sort-Object -Unique)) {
        $hashMap[$file] = (Get-FileHash -Algorithm SHA256 -Path $file).Hash
    }

    return $hashMap
}

function Assert-HashMapsEqual {
    param(
        [Parameter(Mandatory = $true)]$Left,
        [Parameter(Mandatory = $true)]$Right,
        [Parameter(Mandatory = $true)][string]$FailureMessage
    )

    $leftKeys = @($Left.Keys | Sort-Object)
    $rightKeys = @($Right.Keys | Sort-Object)

    if ($leftKeys.Count -ne $rightKeys.Count) {
        throw $FailureMessage
    }

    for ($i = 0; $i -lt $leftKeys.Count; $i++) {
        if ($leftKeys[$i] -ne $rightKeys[$i]) {
            throw $FailureMessage
        }
    }

    foreach ($key in $leftKeys) {
        if ($Left[$key] -ne $Right[$key]) {
            throw $FailureMessage
        }
    }
}

Push-Location $repoRoot
try {
    $env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
    $env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
    $env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH = "0"
    $env:DOTNET_CLI_HOME = Join-Path $repoRoot ".dotnet"
    $env:NUGET_PACKAGES = Join-Path $repoRoot ".nuget\packages"
    $baseOutputPath = (Join-Path $repoRoot "artifacts\dotnet\")
    if (-not (Test-Path $env:DOTNET_CLI_HOME)) {
        New-Item -ItemType Directory -Path $env:DOTNET_CLI_HOME | Out-Null
    }
    if (-not (Test-Path $baseOutputPath)) {
        New-Item -ItemType Directory -Path $baseOutputPath | Out-Null
    }
    $dotnetProps = @(
        "-p:Platform=x64",
        "-p:BaseOutputPath=$baseOutputPath"
    )

    if (-not $SkipSwigReproCheck) {
        Invoke-CheckedCommand -Name "SWIG regen (pass 1)" -Command {
            & (Join-Path $repoRoot "scripts\regen-swig.ps1")
            if ($LASTEXITCODE -ne 0) {
                throw "scripts/regen-swig.ps1 failed ($LASTEXITCODE)."
            }
        }

        $hashAfterFirstRegen = Get-SwigGeneratedFileHashes

        Invoke-CheckedCommand -Name "SWIG regen (pass 2)" -Command {
            & (Join-Path $repoRoot "scripts\regen-swig.ps1")
            if ($LASTEXITCODE -ne 0) {
                throw "scripts/regen-swig.ps1 failed ($LASTEXITCODE)."
            }
        }

        $hashAfterSecondRegen = Get-SwigGeneratedFileHashes

        Assert-HashMapsEqual `
            -Left $hashAfterFirstRegen `
            -Right $hashAfterSecondRegen `
            -FailureMessage "SWIG regen reproducibility failed: generated files changed between consecutive regenerations."
    }

    if (-not $SkipNativeBuild) {
        foreach ($configuration in $NativeConfigurations) {
            Invoke-CheckedCommand -Name "Native build ($configuration|$Platform)" -Command {
                & (Join-Path $repoRoot "scripts\build-native.ps1") -Configuration $configuration -Platform $Platform
                if ($LASTEXITCODE -ne 0) {
                    throw "scripts/build-native.ps1 failed for configuration '$configuration' ($LASTEXITCODE)."
                }
            }
        }
    }

    if (-not $SkipManagedTests) {
        Invoke-CheckedCommand -Name "Handwritten API docs gate ($DotnetConfiguration)" -Command {
            & (Join-Path $repoRoot "scripts\check-handwritten-api-docs.ps1")
            if ($LASTEXITCODE -ne 0) {
                throw "check-handwritten-api-docs.ps1 failed ($LASTEXITCODE)."
            }
        }

        Invoke-CheckedCommand -Name "Generated API docs gate ($DotnetConfiguration)" -Command {
            & (Join-Path $repoRoot "scripts\check-generated-api-docs.ps1") -Configuration $DotnetConfiguration -Framework "net8.0"
            if ($LASTEXITCODE -ne 0) {
                throw "check-generated-api-docs.ps1 failed ($LASTEXITCODE)."
            }
        }

        $testArgs = @(
            "test",
            "Tests\Tests.Pagmo.NET\Tests.Pagmo.NET.csproj",
            "-c",
            $DotnetConfiguration
        ) + $dotnetProps
        if ($NoRestore) {
            $testArgs += "--no-restore"
        }

        Invoke-CheckedCommand -Name "Managed test suite ($DotnetConfiguration)" -Command {
            dotnet @testArgs
            if ($LASTEXITCODE -ne 0) {
                throw "dotnet test failed ($LASTEXITCODE)."
            }
        }

        Invoke-CheckedCommand -Name "Optional solver availability tests ($DotnetConfiguration)" -Command {
            dotnet test Tests\Tests.Pagmo.NET\Tests.Pagmo.NET.csproj `
                -c $DotnetConfiguration `
                @dotnetProps `
                --filter "FullyQualifiedName~Test_optional_solver_availability"
            if ($LASTEXITCODE -ne 0) {
                throw "Optional solver availability tests failed ($LASTEXITCODE)."
            }
        }
    }

    Write-Host ""
    Write-Host "Release gates completed successfully."
}
finally {
    Pop-Location
}
