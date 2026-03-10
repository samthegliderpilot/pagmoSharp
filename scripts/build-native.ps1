param(
    [string]$Configuration = "Debug",
    [string]$Platform = "x64"
)

$ErrorActionPreference = "Stop"

$vsWhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
if (-not (Test-Path $vsWhere)) {
    throw "vswhere.exe was not found. Install Visual Studio Build Tools 2022."
}

$msbuildExe = & $vsWhere -latest -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
if (-not $msbuildExe) {
    throw "MSBuild.exe was not found. Install Visual Studio Build Tools 2022."
}

& $msbuildExe "pagmoWrapper\pagmoWrapper.vcxproj" /m /p:Configuration=$Configuration /p:Platform=$Platform
