$root = Split-Path $PSScriptRoot -Parent

$files = @(
    'pagmoWrapper\multi_objective.h',
    'pagmoWrapper\r_policy.h',
    'pagmoWrapper\s_policy.h',
    'pagmoWrapper\problem.h',
    'swig\PagmoNETSwigInterface.i'
)

foreach ($f in $files) {
    $path = Join-Path $root $f
    if (Test-Path $path) {
        $content = [System.IO.File]::ReadAllText($path)
        # Normalize: collapse any existing CRLF to LF, then expand all LF to CRLF
        $normalized = $content -replace "`r`n", "`n" -replace "`n", "`r`n"
        [System.IO.File]::WriteAllText($path, $normalized, [System.Text.Encoding]::UTF8)
        Write-Host "Normalized: $f"
    } else {
        Write-Host "Not found: $path"
    }
}
