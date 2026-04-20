Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$root = Join-Path $PSScriptRoot "..\Pagmo.NET\pagmoExtensions"
$files = Get-ChildItem $root -Recurse -Filter *.cs

$missing = [System.Collections.Generic.List[string]]::new()

foreach ($file in $files) {
    $lines = Get-Content $file.FullName
    for ($i = 0; $i -lt $lines.Count; $i++) {
        $trim = $lines[$i].Trim()
        if ($trim -notmatch '^(public|protected)\s+') { continue }
        if ($trim -match '^(public|protected)\s*(using|namespace)\b') { continue }

        $k = $i - 1
        while ($k -ge 0) {
            $prevTrim = $lines[$k].Trim()
            if ($prevTrim -eq '') { $k--; continue }
            if ($prevTrim -match '^\[[^\]]+\]$') { $k--; continue }
            break
        }

        $hasDoc = $k -ge 0 -and $lines[$k].Trim().StartsWith('///')

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
