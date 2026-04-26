param(
    [string]$Version = "1.0.0-beta.1",
    [string]$Configuration = "Release",
    [string]$Platform = "x64",
    [switch]$SkipReleaseGates
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# $IsLinux / $IsMacOS are automatic variables in PowerShell 7+ but undefined in 5.1.
if (-not (Get-Variable -Name IsLinux -ErrorAction SilentlyContinue)) { $IsLinux = $false }
if (-not (Get-Variable -Name IsMacOS -ErrorAction SilentlyContinue)) { $IsMacOS = $false }

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH = "0"
$env:DOTNET_CLI_HOME = Join-Path $repoRoot ".dotnet"
$env:NUGET_PACKAGES = Join-Path $repoRoot ".nuget\packages"
$dotnetMutexName = "Global\pagmoNet_dotnet_library_build"
$dotnetMutex = New-Object System.Threading.Mutex($false, $dotnetMutexName)
$hasDotnetLock = $false

if (-not (Test-Path $env:DOTNET_CLI_HOME)) {
    New-Item -ItemType Directory -Path $env:DOTNET_CLI_HOME | Out-Null
}

$artifactRoot   = Join-Path $repoRoot "artifacts/release/$Version"
$nugetOut       = Join-Path $artifactRoot "nuget"
$nativeWinOut   = Join-Path $artifactRoot "native/win-x64"
$nativeLinuxOut = Join-Path $artifactRoot "native/linux-x64"
$nativeOut      = if ($IsLinux -eq $true -or $IsMacOS -eq $true) { $nativeLinuxOut } else { $nativeWinOut }
$sourceOut      = Join-Path $artifactRoot "source"

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
        & (Join-Path $repoRoot "scripts/release-gates.ps1")
        if ($LASTEXITCODE -ne 0) {
            throw "release-gates.ps1 failed ($LASTEXITCODE)."
        }
    }

    Write-Host "==> Building native release binaries"
    & (Join-Path $repoRoot "scripts/build-native.ps1") -Configuration $Configuration -Platform $Platform
    if ($LASTEXITCODE -ne 0) {
        throw "build-native.ps1 failed ($LASTEXITCODE)."
    }

    Write-Host "==> Packing NuGet package"
    dotnet pack (Join-Path $repoRoot "Pagmo.NET/Pagmo.NET.csproj") `
        -c $Configuration `
        -o $nugetOut `
        -p:Platform=$Platform `
        -p:Version=$Version `
        -p:PackageVersion=$Version
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet pack failed ($LASTEXITCODE)."
    }

    Write-Host "==> Collecting native runtime bundle"
    if ($IsLinux -eq $true -or $IsMacOS -eq $true) {
        # Linux: collect libPagmoWrapper.so from the cmake build directory.
        # All dependencies (pagmo2, Boost, TBB, NLopt, IPOPT) are statically linked via
        # the x64-linux-static-pic vcpkg triplet — no system runtime dependencies required.
        $cmakeBuildDir = Join-Path $repoRoot "pagmoWrapper/build"
        Get-ChildItem -Path $cmakeBuildDir -File -Filter "libPagmoWrapper.*" | ForEach-Object {
            Copy-Item -Path $_.FullName -Destination (Join-Path $nativeOut $_.Name)
        }

        $nativeZipName = "Pagmo.NET-native-linux-x64-$Version.zip"
    } else {
        # Windows: cmake static build (x64-windows-static-md) produces a single self-contained
        # PagmoWrapper.dll with pagmo2, Boost, TBB, NLopt, and IPOPT statically linked.
        # No additional DLLs are required at runtime.
        $cmakeWinBuildDir = Join-Path $repoRoot "pagmoWrapper/win-build"
        $releaseDll = Join-Path $cmakeWinBuildDir "PagmoWrapper.dll"
        if (-not (Test-Path $releaseDll)) {
            throw "Windows cmake DLL not found at '$releaseDll'. Ensure VCPKG_ROOT is set and build-native.ps1 completed successfully."
        }
        Copy-Item -Path $releaseDll -Destination (Join-Path $nativeOut "PagmoWrapper.dll")

        $nativeZipName = "Pagmo.NET-native-win-x64-$Version.zip"
    }

    $nativeZip = Join-Path $artifactRoot $nativeZipName
    if (Test-Path $nativeZip) { Remove-Item -Force $nativeZip }
    Compress-Archive -Path (Join-Path $nativeOut "*") -DestinationPath $nativeZip

    Write-Host "==> Creating source archive"
    $sourceZip = Join-Path $sourceOut "Pagmo.NET-$Version-source.zip"
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
            $relative = $_.FullName.Substring($artifactRoot.Length + 1).Replace('\\', '/')
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
