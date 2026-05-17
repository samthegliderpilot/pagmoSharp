<#
.SYNOPSIS
    Builds the pagmo4j JNI native shared library.
.DESCRIPTION
    Uses CMake + vcpkg to build pagmo4j.dll / libpagmo4j.so / libpagmo4j.dylib.
    The SWIG-generated pagmo4j_wrap.cxx must already exist in
    java/pagmoWrapper/generated/ — run java/scripts/regen-swig.ps1 first.
.PARAMETER Configuration
    Build configuration: Debug or Release. Default: Release.
.PARAMETER VcpkgTriplet
    vcpkg triplet override. Auto-detected from platform if omitted.
.EXAMPLE
    pwsh java/scripts/build-native.ps1 -Configuration Release
#>
param(
    [ValidateSet("Debug", "Release")] [string]$Configuration = "Release",
    [string]$VcpkgTriplet = ""
)

$ErrorActionPreference = "Stop"

$JavaRoot     = Split-Path $PSScriptRoot -Parent
$MonorepoRoot = Split-Path $JavaRoot -Parent

$vcpkgRoot = $env:VCPKG_ROOT
if (-not $vcpkgRoot) { throw "VCPKG_ROOT is not set." }

$javaHome = $env:JAVA_HOME
if (-not $javaHome) { throw "JAVA_HOME is not set. JNI headers require a JDK installation." }

if ($IsWindows -or $env:OS -eq "Windows_NT") {
    if (-not $VcpkgTriplet) { $VcpkgTriplet = "x64-windows-static-md" }
    $buildDir = Join-Path $JavaRoot "pagmoWrapper\win-build"
} elseif ($IsMacOS) {
    $arch = (uname -m).Trim()
    if (-not $VcpkgTriplet) {
        $VcpkgTriplet = if ($arch -eq "arm64") { "arm64-osx-static-pic" } else { "x64-osx-static-pic" }
    }
    $buildDir = Join-Path $JavaRoot "pagmoWrapper/build"
} else {
    if (-not $VcpkgTriplet) { $VcpkgTriplet = "x64-linux-static-pic" }
    $buildDir = Join-Path $JavaRoot "pagmoWrapper/build"
}

Write-Host "Installing pagmo2 via vcpkg (triplet: $VcpkgTriplet)..."
& "$vcpkgRoot/vcpkg" install "pagmo2[nlopt]:$VcpkgTriplet" `
    "--overlay-triplets=$MonorepoRoot/triplets/" `
    "--overlay-ports=$MonorepoRoot/ports/" `
    "--recurse"

$toolchainFile = (Join-Path $vcpkgRoot "scripts/buildsystems/vcpkg.cmake") -replace '\\', '/'
$javaHomeFwd   = $javaHome -replace '\\', '/'
$buildDirFwd   = $buildDir -replace '\\', '/'
$sourceDirFwd  = (Join-Path $JavaRoot "pagmoWrapper") -replace '\\', '/'
$overlayFwd    = "$MonorepoRoot/triplets/".Replace('\', '/')

New-Item -ItemType Directory -Force -Path $buildDir | Out-Null

# On Windows: use the same VS/MSVC version that compiled pagmo.lib via vcpkg.
# pagmo.lib uses VS 2022 (MSVC 14.4x/14.5x) STL symbols. Without matching the
# generator, VS 2019 may be picked which produces unresolved external errors.
$cmakeArgs = @(
    "-B", $buildDirFwd,
    "-S", $sourceDirFwd,
    "-DCMAKE_BUILD_TYPE=$Configuration",
    "-DCMAKE_TOOLCHAIN_FILE=$toolchainFile",
    "-DVCPKG_TARGET_TRIPLET=$VcpkgTriplet",
    "-DVCPKG_OVERLAY_TRIPLETS=$overlayFwd",
    "-DPAGMO4J_JNI=ON",
    "-DJAVA_HOME=$javaHomeFwd"
)
if ($IsWindows -or $env:OS -eq "Windows_NT") {
    # Find the vcvars64.bat from the installed VS (any version) and import its environment
    # so Ninja can locate link.exe, rc.exe, Windows SDK headers, etc.
    $vcVarsSearch = @(
        "C:\Program Files\Microsoft Visual Studio\18\Community\VC\Auxiliary\Build\vcvars64.bat",
        "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvars64.bat",
        "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\VC\Auxiliary\Build\vcvars64.bat"
    )
    $vcVars = $vcVarsSearch | Where-Object { Test-Path $_ } | Select-Object -First 1
    if ($vcVars) {
        Write-Host "Importing VS environment from: $vcVars"
        # Ensure vswhere.exe is in PATH (required by vcvars64.bat)
        $vsInstaller = "C:\Program Files (x86)\Microsoft Visual Studio\Installer"
        if (Test-Path $vsInstaller) { $env:PATH = "$vsInstaller;$env:PATH" }
        $envOutput = cmd /c "`"$vcVars`" && set" 2>&1
        $envOutput | ForEach-Object {
            if ($_ -match '^([^=]+)=(.*)$') {
                [System.Environment]::SetEnvironmentVariable($Matches[1], $Matches[2], 'Process')
            }
        }
        $cmakeArgs += "-G", "Ninja"
    }
}

if ($IsMacOS) {
    $osx_arch = if ($VcpkgTriplet -match "arm64") { "arm64" } else { "x86_64" }
    $cmakeArgs += "-DCMAKE_OSX_ARCHITECTURES=$osx_arch"
}

Write-Host "CMake configure..."
cmake @cmakeArgs
if ($LASTEXITCODE -ne 0) { throw "CMake configure failed." }

Write-Host "CMake build ($Configuration)..."
cmake --build $buildDir --config $Configuration
if ($LASTEXITCODE -ne 0) { throw "CMake build failed." }

Write-Host "Native build complete. Output: $buildDir"
