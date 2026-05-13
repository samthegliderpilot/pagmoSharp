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

$toolchainFile = Join-Path $vcpkgRoot "scripts/buildsystems/vcpkg.cmake"

New-Item -ItemType Directory -Force -Path $buildDir | Out-Null

$cmakeArgs = @(
    "-B", $buildDir,
    "-S", (Join-Path $JavaRoot "pagmoWrapper"),
    "-DCMAKE_BUILD_TYPE=$Configuration",
    "-DCMAKE_TOOLCHAIN_FILE=$toolchainFile",
    "-DVCPKG_TARGET_TRIPLET=$VcpkgTriplet",
    "-DVCPKG_OVERLAY_TRIPLETS=$MonorepoRoot/triplets/",
    "-DPAGMO4J_JNI=ON",
    "-DJAVA_HOME=$javaHome"
)

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
