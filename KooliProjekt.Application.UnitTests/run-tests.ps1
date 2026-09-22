[CmdletBinding()]
param([switch]$OpenReport)
$ErrorActionPreference = 'Stop'
$project = Join-Path $PSScriptRoot 'KooliProjekt.UnitTests.csproj'
$run = Join-Path $PSScriptRoot ('BuildReports\UnitTests\' + (Get-Date -Format 'yyyyMMdd-HHmmss') + '-' + [guid]::NewGuid().ToString('N').Substring(0,8))
$htmlRoot = Join-Path $PSScriptRoot 'BuildReports\Coverage'
New-Item -ItemType Directory -Path $run,$htmlRoot -Force | Out-Null
$settings = Join-Path $run 'coverage.runsettings'
@'
<RunSettings>
 <DataCollectionRunSettings><DataCollectors><DataCollector friendlyName="XPlat Code Coverage">
  <Configuration><Format>cobertura</Format><Include>[KooliProjekt.Application]*</Include><Exclude>[KooliProjekt.Application]KooliProjekt.Application.Behaviors.*,[KooliProjekt.Application]KooliProjekt.Application.Data.*,[KooliProjekt.Application]KooliProjekt.Application.Dto.*,[KooliProjekt.Application]KooliProjekt.Application.Infrastructure.*,[KooliProjekt.Application]KooliProjekt.Application.Migrations.*</Exclude><ExcludeByAttribute>ExcludeFromCodeCoverageAttribute</ExcludeByAttribute></Configuration>
 </DataCollector></DataCollectors></DataCollectionRunSettings>
</RunSettings>
'@ | Set-Content -LiteralPath $settings -Encoding UTF8
& dotnet test $project --no-restore --collect 'XPlat Code Coverage' --settings $settings --results-directory $run --logger 'console;verbosity=minimal' --logger 'trx;LogFileName=UnitTests.trx'
$testExit = $LASTEXITCODE
$reports = @(Get-ChildItem -LiteralPath $run -Recurse -Filter 'coverage.cobertura.xml' | Where-Object { $_.Directory.Parent.FullName -eq $run })
if ($reports.Count -ne 1) { throw "Expected one coverage report, found $($reports.Count). See test output above." }
[xml]$coverage = Get-Content -LiteralPath $reports[0].FullName -Raw
[xml]$testReport = Get-Content -LiteralPath (Join-Path $run 'UnitTests.trx') -Raw
$counters = $testReport.TestRun.ResultSummary.Counters
$classes = @($coverage.coverage.packages.package.classes.class)
$sources = @($coverage.coverage.sources.source)
$culture = [Globalization.CultureInfo]::InvariantCulture
function Escape-Html([string]$text) { return [Net.WebUtility]::HtmlEncode($text) }
function Percentage([double]$value) { return ($value * 100).ToString('0.##', $culture) + '%' }
function Get-FileCoverage($group) {
    $lines = @{}
    foreach ($class in $group.Group) {
        foreach ($line in @($class.lines.line)) {
            if ($null -eq $line) { continue }
            $number = [int]$line.number
            if (-not $lines.ContainsKey($number)) {
                $lines[$number] = @{ Hits=0; Branches=0; CoveredBranches=0 }
            }
            $lines[$number].Hits += [long]$line.hits
            $condition = $line.GetAttribute('condition-coverage')
            if ($condition -match '\((\d+)/(\d+)\)') {
                $lines[$number].CoveredBranches += [int]$Matches[1]
                $lines[$number].Branches += [int]$Matches[2]
            }
        }
    }
    $covered = @($lines.Values | Where-Object { $_.Hits -gt 0 }).Count
    $branches = 0; $coveredBranches = 0
    foreach ($line in $lines.Values) { $branches += $line.Branches; $coveredBranches += $line.CoveredBranches }
    return [pscustomobject]@{
        File=$group.Name; Lines=$lines; Total=$lines.Count; Covered=$covered
        Branches=$branches; CoveredBranches=$coveredBranches
        IsDelete=($group.Name -match '(^|[\\/])Delete\w+CommandHandler\.cs$')
    }
}
$files = @($classes | Group-Object filename | ForEach-Object { Get-FileCoverage $_ })
$deleteFiles = @($files | Where-Object IsDelete)
$displayFiles = $files
$expected = @('DeleteOluCommandHandler.cs','DeletePartiiCommandHandler.cs','DeleteKoostisosaCommandHandler.cs','DeleteMaitsmineCommandHandler.cs','DeletePruulimisLogiCommandHandler.cs','DeletePartiiFotoCommandHandler.cs')
$problems = @()
foreach ($name in $expected) {
    $match = @($deleteFiles | Where-Object { [IO.Path]::GetFileName($_.File) -eq $name })
    if ($match.Count -ne 1) { $problems += "Missing or duplicate coverage: $name"; continue }
    $item = $match[0]
    if ($item.Total -eq 0 -or $item.Covered -ne $item.Total -or $item.CoveredBranches -ne $item.Branches) {
        $problems += "Incomplete Delete coverage: $name"
    }
}
$html = [Text.StringBuilder]::new()
[void]$html.Append(@'
<!doctype html><html lang="en"><head><meta charset="utf-8"><title>Application unit test coverage</title>
<style>body{font:16px system-ui;margin:32px;color:#182333;background:#f6f8fb}h1,h2{color:#18334d}table{border-collapse:collapse;width:100%;background:white}th,td{padding:9px;border:1px solid #d7dee6;text-align:left}th{background:#e8eef6}a{color:#145aad}.ok{background:#dbf4df}.partial{background:#ffe0eb}.miss{background:#ffd9d9}.code{font:13px Consolas,monospace;white-space:pre;overflow:auto;background:white;padding:12px}.code div{min-height:18px}.number{display:inline-block;width:48px;color:#657184}.note{padding:16px;background:#e8eef6}section{margin-top:28px}</style></head><body>
'@)
[void]$html.Append('<h1>Application unit test coverage</h1>')
[void]$html.Append("<p>Tests: $($counters.total); passed: $($counters.passed); failed: $($counters.failed).</p>")
$overall = Percentage ([double]::Parse($coverage.coverage.GetAttribute('line-rate'),$culture))
[void]$html.Append("<p>12.02 included code line coverage: <strong>$overall</strong>.</p>")
[void]$html.Append('<p class="note">12.02: Behaviors, Data, DTO, Infrastructure, Migrations and command/query models are excluded. All handlers and validators remain included. Every included executable line and branch must be covered. Search filters are applied before pagination.</p>')
[void]$html.Append('<h2>All included handlers and validators</h2><table><tr><th>File</th><th>Lines</th><th>Branches</th></tr>')
$index = 0
foreach ($item in ($displayFiles | Sort-Object File)) {
    $item | Add-Member -NotePropertyName Anchor -NotePropertyValue "delete-$index"
    $index++
    $name = Escape-Html ([IO.Path]::GetFileName($item.File))
    $class = if ($item.Total -gt 0 -and $item.Covered -eq $item.Total -and $item.CoveredBranches -eq $item.Branches) { 'ok' } else { 'miss' }
    [void]$html.Append("<tr class='$class'><td><a href='#$($item.Anchor)'>$name</a></td><td>$($item.Covered)/$($item.Total)</td><td>$($item.CoveredBranches)/$($item.Branches)</td></tr>")
}
[void]$html.Append('</table><h2>Included source files</h2><table><tr><th>File</th><th>Lines</th><th>Branches</th></tr>')
foreach ($item in ($files | Sort-Object File)) {
    [void]$html.Append("<tr><td>$(Escape-Html $item.File)</td><td>$($item.Covered)/$($item.Total)</td><td>$($item.CoveredBranches)/$($item.Branches)</td></tr>")
}
[void]$html.Append('</table>')
foreach ($item in ($displayFiles | Sort-Object File)) {
    [void]$html.Append("<section id='$($item.Anchor)'><h2>$(Escape-Html $item.File)</h2><div class='code'>")
    $sourcePath = $null
    foreach ($source in $sources) {
        $candidate = if ([IO.Path]::IsPathRooted($item.File)) { $item.File } else { Join-Path ([string]$source) $item.File }
        if (Test-Path -LiteralPath $candidate -PathType Leaf) { $sourcePath=$candidate; break }
    }
    if ($null -eq $sourcePath) { $problems += "Source unavailable: $($item.File)"; [void]$html.Append('Source unavailable.') }
    else {
        $number=0
        foreach ($text in [IO.File]::ReadAllLines($sourcePath)) {
            $number++; $class=''; $label=''
            if ($item.Lines.ContainsKey($number)) {
                $line=$item.Lines[$number]
                $class=if($line.Hits -eq 0){'miss'}elseif($line.CoveredBranches -lt $line.Branches){'partial'}else{'ok'}
                $label="hits=$($line.Hits); branches=$($line.CoveredBranches)/$($line.Branches)"
            }
            [void]$html.Append("<div class='$class' title='$label'><span class='number'>$number</span>$(Escape-Html $text)</div>")
        }
    }
    [void]$html.Append('</div></section>')
}
[void]$html.Append('</body></html>')
$reportPath = Join-Path $htmlRoot 'index.htm'
[IO.File]::WriteAllText($reportPath,$html.ToString(),[Text.UTF8Encoding]::new($false))
Write-Host "HTML REPORT: $reportPath"
Write-Host "RAW COVERAGE: $($reports[0].FullName)"
if($OpenReport){Start-Process -FilePath $reportPath}
if($testExit -ne 0 -or [int]$counters.failed -ne 0){throw 'Unit tests failed; inspect the report.'}
if([int]$counters.passed -lt 497){throw "Expected at least 497 passed tests, got $($counters.passed)."}
if ($files.Count -ne 30) { $problems += "Expected 24 handlers and 6 validators, found $($files.Count) included files." }
foreach ($item in $files) {
    if ($item.File -notmatch '(Handler|Validator)\.cs$') { $problems += "Unexpected included file: $($item.File)" }
    if ($item.Total -eq 0 -or $item.Covered -ne $item.Total -or $item.CoveredBranches -ne $item.Branches) {
        $problems += "Incomplete coverage: $($item.File): lines $($item.Covered)/$($item.Total), branches $($item.CoveredBranches)/$($item.Branches)"
    }
}
if($problems.Count){throw ($problems -join '; ')}
Write-Host 'ALL TESTS PASSED; INCLUDED CODE: 100% LINES AND BRANCHES.' -ForegroundColor Green
