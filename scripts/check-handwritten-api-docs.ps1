Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$root = Join-Path $PSScriptRoot "..\pagmoSharp\pagmoExtensions"
$files = Get-ChildItem $root -Recurse -Filter *.cs

$missing = [System.Collections.Generic.List[string]]::new()

foreach ($file in $files) {
    $lines = Get-Content $file.FullName
    for ($i = 0; $i -lt $lines.Count; $i++) {
        $trim = $lines[$i].Trim()
        if ($trim -notmatch '^(public|protected)\s+') { continue }
        if ($trim -match '^(public|protected)\s*(using|namespace)\b') { continue }

        $hasDoc = $false
        for ($k = 1; $k -le 3; $k++) {
            if ($i - $k -lt 0) { continue }
            if ($lines[$i - $k].Trim().StartsWith('///')) {
                $hasDoc = $true
                break
            }
        }

        if (-not $hasDoc) {
            $relative = Resolve-Path -Relative $file.FullName
            $missing.Add("${relative}:$($i + 1): $trim")
        }
    }
}

if ($missing.Count -gt 0) {
    Write-Host "Missing XML docs on handwritten public/protected members:"
    $missing | ForEach-Object { Write-Host "  $_" }
    throw "Handwritten API docs check failed with $($missing.Count) undocumented members."
}

Write-Host "Handwritten API docs check passed."
