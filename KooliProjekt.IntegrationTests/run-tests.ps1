[CmdletBinding()]
param()
$ErrorActionPreference='Stop'
$project=Join-Path $PSScriptRoot 'KooliProjekt.IntegrationTests.csproj'
$results=Join-Path $PSScriptRoot ('BuildReports\\Integration\\'+(Get-Date -Format 'yyyyMMdd-HHmmss')+'-'+[guid]::NewGuid().ToString('N').Substring(0,8))
New-Item -ItemType Directory -Path $results -Force | Out-Null
$previousPreference=$ErrorActionPreference
try {
 $ErrorActionPreference='Continue'
 & dotnet restore $project --verbosity minimal 2>&1 | ForEach-Object { Write-Host $_ }
 $restoreExit=$LASTEXITCODE
} finally { $ErrorActionPreference=$previousPreference }
if($restoreExit -ne 0){throw 'Package restore failed. Check NuGet access; no integration tests were run.'}
try {
 $ErrorActionPreference='Continue'
 & dotnet test $project --no-restore --results-directory $results --logger 'console;verbosity=minimal' --logger 'trx;LogFileName=IntegrationTests.trx' 2>&1 | ForEach-Object { Write-Host $_ }
 $testExit=$LASTEXITCODE
} finally { $ErrorActionPreference=$previousPreference }
if($testExit -ne 0){throw 'Integration tests failed. Send the first error and test summary.'}
[xml]$report=Get-Content (Join-Path $results 'IntegrationTests.trx') -Raw
$counters=$report.TestRun.ResultSummary.Counters
if([int]$counters.passed -ne 73 -or [int]$counters.failed -ne 0 -or [int]$counters.total -ne 73){throw "Expected 73 passing integration tests. Actual: $($counters.OuterXml)"}
Write-Host 'ALL 73 SQL INTEGRATION TESTS PASSED.' -ForegroundColor Green
Write-Host "REPORT: $(Join-Path $results 'IntegrationTests.trx')"

