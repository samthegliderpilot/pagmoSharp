#Requires -Version 7.0
param()

$ErrorActionPreference = "Stop"

# Resolve SWIG executable: SWIG_EXE env var, then SWIG_HOME, then PATH.
$swigExe = $env:SWIG_EXE
if (-not $swigExe) {
    if ($env:SWIG_HOME -and (Test-Path (Join-Path $env:SWIG_HOME "swig"))) {
        $swigExe = Join-Path $env:SWIG_HOME "swig"
    } elseif ($env:SWIG_HOME -and (Test-Path (Join-Path $env:SWIG_HOME "swig.exe"))) {
        $swigExe = Join-Path $env:SWIG_HOME "swig.exe"
    } else {
        $swigExe = (Get-Command "swig" -ErrorAction SilentlyContinue)?.Source
    }
}
if (-not $swigExe) {
    throw "SWIG executable not found. Set SWIG_EXE, set SWIG_HOME, or add swig to PATH."
}

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot ".")
$swigOut = Join-Path $repoRoot "swig"
$wrapperSrc = Join-Path $repoRoot "pagmoWrapper"
$csOut = Join-Path $repoRoot "Pagmo.NET/pygmoWrappers"

# Run SWIG. Forward slashes work on both Windows and Linux.
& $swigExe -c++ -csharp -namespace pagmo -dllimport PagmoWrapper `
    "-I$wrapperSrc" "-I$swigOut" `
    (Join-Path $swigOut "PagmoNETSwigInterface.i")
if ($LASTEXITCODE -ne 0) { throw "SWIG failed ($LASTEXITCODE)." }

# Copy generated .cxx → GeneratedWrappers.cxx
$generatedCxx = Join-Path $swigOut "PagmoNETSwigInterface_wrap.cxx"
Copy-Item -Force $generatedCxx (Join-Path $wrapperSrc "GeneratedWrappers.cxx")

# Copy generated .h (retry up to 8 times on Windows where file locks can bite)
$generatedH = Join-Path $swigOut "PagmoNETSwigInterface_wrap.h"
$destH = Join-Path $wrapperSrc "PagmoNETSwigInterface_wrap.h"
$copied = $false
for ($i = 0; $i -lt 8; $i++) {
    try {
        Copy-Item -Force $generatedH $destH
        $copied = $true
        break
    } catch {
        Start-Sleep -Milliseconds 500
    }
}
if (-not $copied) {
    Write-Warning "Warning: could not overwrite PagmoNETSwigInterface_wrap.h after retries."
}

# Replace pygmoWrappers with freshly generated .cs files
if (Test-Path $csOut) { Remove-Item -Recurse -Force $csOut }
New-Item -ItemType Directory -Path $csOut | Out-Null
Get-ChildItem (Join-Path $swigOut "*.cs") | Copy-Item -Destination $csOut

# Clean up SWIG output directory
Remove-Item -Force (Join-Path $swigOut "PagmoNETSwigInterface_wrap.cxx") -ErrorAction SilentlyContinue
Remove-Item -Force (Join-Path $swigOut "PagmoNETSwigInterface_wrap.h") -ErrorAction SilentlyContinue
Get-ChildItem (Join-Path $swigOut "*.cs") | Remove-Item -Force -ErrorAction SilentlyContinue
