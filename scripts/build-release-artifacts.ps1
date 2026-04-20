param(
    [string]$Version = "1.0.0-beta.1",
    [string]$Configuration = "Release",
    [string]$Platform = "x64",
    [switch]$SkipReleaseGates,
    [switch]$IncludeDebugNativeArtifacts
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH = "0"
$env:DOTNET_CLI_HOME = Join-Path $repoRoot ".dotnet"
$env:NUGET_PACKAGES = Join-Path $repoRoot ".nuget\packages"
$dotnetMutexName = "Global\pagmoSharp_dotnet_library_build"
$dotnetMutex = New-Object System.Threading.Mutex($false, $dotnetMutexName)
$hasDotnetLock = $false

if (-not (Test-Path $env:DOTNET_CLI_HOME)) {
    New-Item -ItemType Directory -Path $env:DOTNET_CLI_HOME | Out-Null
}

$artifactRoot = Join-Path $repoRoot "artifacts\release\$Version"
$nugetOut = Join-Path $artifactRoot "nuget"
$nativeOut = Join-Path $artifactRoot "native\win-x64"
$sourceOut = Join-Path $artifactRoot "source"

if (Test-Path $artifactRoot) {
    Remove-Item -Recurse -Force $artifactRoot
}

New-Item -ItemType Directory -Path $nugetOut | Out-Null
New-Item -ItemType Directory -Path $nativeOut | Out-Null
New-Item -ItemType Directory -Path $sourceOut | Out-Null

Push-Location $repoRoot
try {
    $hasDotnetLock = $dotnetMutex.WaitOne([TimeSpan]::FromMinutes(30))
    if (-not $hasDotnetLock) {
        throw "Timed out waiting for dotnet build lock '$dotnetMutexName'."
    }

    if (-not $SkipReleaseGates) {
        Write-Host "==> Running release gates"
        & (Join-Path $repoRoot "scripts\release-gates.ps1")
        if ($LASTEXITCODE -ne 0) {
            throw "release-gates.ps1 failed ($LASTEXITCODE)."
        }
    }

    Write-Host "==> Building native release binaries"
    & (Join-Path $repoRoot "scripts\build-native.ps1") -Configuration $Configuration -Platform $Platform
    if ($LASTEXITCODE -ne 0) {
        throw "build-native.ps1 failed ($LASTEXITCODE)."
    }

    Write-Host "==> Packing NuGet package"
    dotnet pack (Join-Path $repoRoot "pagmoSharp\pagmoSharp.csproj") `
        -c $Configuration `
        -o $nugetOut `
        -p:Platform=$Platform `
        -p:Version=$Version `
        -p:PackageVersion=$Version
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet pack failed ($LASTEXITCODE)."
    }

    Write-Host "==> Collecting native runtime bundle"
    $nativeSource = Join-Path $repoRoot "pagmoWrapper\pagmoWrapper\bin"
    $nativeFiles = @("*.dll", "*.pdb")
    foreach ($pattern in $nativeFiles) {
        Get-ChildItem -Path $nativeSource -File -Filter $pattern | Where-Object {
            if ($IncludeDebugNativeArtifacts) {
                return $true
            }

            $name = $_.Name.ToLowerInvariant()
            if ($name -like "*-gd-*") { return $false }
            if ($name -like "*debug*") { return $false }
            if ($name -like "*.pdb") { return $false }
            if ($name -eq "zlibd1.dll") { return $false }
            if ($name -eq "bz2d.dll") { return $false }
            return $true
        } | ForEach-Object {
            Copy-Item -Path $_.FullName -Destination (Join-Path $nativeOut $_.Name)
        }
    }

    $nativeZip = Join-Path $artifactRoot "pagmoSharp-native-win-x64-$Version.zip"
    if (Test-Path $nativeZip) {
        Remove-Item -Force $nativeZip
    }
    Compress-Archive -Path (Join-Path $nativeOut "*") -DestinationPath $nativeZip

    Write-Host "==> Creating source archive"
    $sourceZip = Join-Path $sourceOut "pagmoSharp-$Version-source.zip"
    git archive --format=zip --output="$sourceZip" HEAD
    if ($LASTEXITCODE -ne 0) {
        throw "git archive failed ($LASTEXITCODE)."
    }

    Write-Host "==> Writing checksums"
    $checksumsPath = Join-Path $artifactRoot "SHA256SUMS.txt"
    $hashLines = Get-ChildItem -Path $artifactRoot -Recurse -File `
        | Where-Object { $_.FullName -ne $checksumsPath } `
        | ForEach-Object {
            $hash = (Get-FileHash -Algorithm SHA256 -Path $_.FullName).Hash.ToLowerInvariant()
            $relative = $_.FullName.Substring($artifactRoot.Length + 1).Replace('\', '/')
            "$hash  $relative"
        }
    Set-Content -Path $checksumsPath -Value $hashLines

    Write-Host ""
    Write-Host "Release artifacts generated:"
    Write-Host "  $artifactRoot"
}
finally {
    if ($hasDotnetLock) {
        $dotnetMutex.ReleaseMutex()
    }
    $dotnetMutex.Dispose()
    Pop-Location
}
