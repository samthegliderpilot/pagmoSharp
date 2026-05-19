<#
.SYNOPSIS
    Regenerates the SWIG-generated Java wrapper sources for pagmo4j.
.DESCRIPTION
    Runs SWIG with the -java backend against Pagmo4jSwigInterface.i and writes the
    generated Java files to core/src/generated/java/... and the C++ JNI glue to
    pagmoWrapper/generated/pagmo4j_wrap.cxx.

    Requires SWIG 4.4.x. Resolved via SWIG_EXE env var, SWIG_HOME, or PATH.
.PARAMETER PackageName
    Java package name. Default: io.github.samthegliderpilot.pagmo4j
.EXAMPLE
    pwsh java/scripts/regen-swig.ps1
#>
param(
    [string]$PackageName = "io.github.samthegliderpilot.pagmo4j"
)

$ErrorActionPreference = "Stop"

$JavaRoot     = Split-Path $PSScriptRoot -Parent             # pagmoSharp/java/
$MonorepoRoot = Split-Path $JavaRoot -Parent                 # pagmoSharp/

# Resolve SWIG executable
$swigExe = $env:SWIG_EXE
if (-not $swigExe) {
    $swigHome = $env:SWIG_HOME
    if ($swigHome) { $swigExe = Join-Path $swigHome "swig.exe" }
}
if (-not $swigExe) { $swigExe = "swig" }

Write-Host "Using SWIG: $swigExe"
& $swigExe -version | Select-Object -First 1

$vcpkgRoot = $env:VCPKG_ROOT
if (-not $vcpkgRoot) { throw "VCPKG_ROOT is not set." }

# Detect platform vcpkg triplet for include path
if ($IsWindows -or $env:OS -eq "Windows_NT") {
    $triplet = "x64-windows-static-md"
} elseif ($IsMacOS) {
    $arch = (uname -m).Trim()
    $triplet = if ($arch -eq "arm64") { "arm64-osx-static-pic" } else { "x64-osx-static-pic" }
} else {
    $triplet = "x64-linux-static-pic"
}

$vcpkgIncludes   = Join-Path $vcpkgRoot "installed\$triplet\include"
$pagmoWrapperSrc = Join-Path $MonorepoRoot "pagmoWrapper"
$swigSrc         = Join-Path $MonorepoRoot "swig"
$swigSubFiles    = Join-Path $swigSrc "swigInterfaceFiles"

$JavaOutDir = Join-Path $JavaRoot "core\src\generated\java\$($PackageName.Replace('.', [IO.Path]::DirectorySeparatorChar))"
$CppOutDir  = Join-Path $JavaRoot "pagmoWrapper\generated"

New-Item -ItemType Directory -Force -Path $JavaOutDir | Out-Null
New-Item -ItemType Directory -Force -Path $CppOutDir  | Out-Null

$swigArgs = @(
    "-java",
    "-c++",
    "-package", $PackageName,
    "-outdir",  $JavaOutDir,
    "-o",       (Join-Path $CppOutDir "pagmo4j_wrap.cxx"),
    "-I$MonorepoRoot",
    "-I$pagmoWrapperSrc",
    "-I$swigSrc",
    "-I$swigSubFiles",
    "-I$vcpkgIncludes",
    (Join-Path $swigSrc "Pagmo4jSwigInterface.i")
)

Write-Host "Running: $swigExe $($swigArgs -join ' ')"
& $swigExe @swigArgs

if ($LASTEXITCODE -ne 0) { throw "SWIG failed with exit code $LASTEXITCODE" }

Write-Host "SWIG generation complete."
Write-Host "  Java wrappers : $JavaOutDir"
Write-Host "  C++ JNI glue  : $CppOutDir\pagmo4j_wrap.cxx"
