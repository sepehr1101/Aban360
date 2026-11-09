Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path -Parent $PSScriptRoot
$shellPath = (Get-Process -Id $PID).Path
$fixtureRoot = Join-Path $repositoryRoot ('artifacts/test-runner-checks/' + [Guid]::NewGuid().ToString('N'))
$null = New-Item -ItemType Directory -Path $fixtureRoot -Force
$checks = 0

function Check([bool] $Condition, [string] $Message) {
    if (-not $Condition) { throw $Message }
    $script:checks++
}

function Run-Fixture([string] $Name, [hashtable] $Projects, [string[]] $ExtraArguments = @()) {
    $caseRoot = Join-Path $fixtureRoot $Name
    $null = New-Item -ItemType Directory -Path (Join-Path $caseRoot 'scripts') -Force
    $null = New-Item -ItemType Directory -Path (Join-Path $caseRoot 'tests') -Force
    Copy-Item -LiteralPath (Join-Path $repositoryRoot 'scripts/test.ps1') -Destination (Join-Path $caseRoot 'scripts/test.ps1')
    foreach ($entry in $Projects.GetEnumerator()) {
        $projectDirectory = Join-Path $caseRoot ('tests/' + $entry.Key)
        $null = New-Item -ItemType Directory -Path $projectDirectory
        $property = if ($entry.Key -like '*.SmokeTests') { '<OutputType>Exe</OutputType>' } else { '<IsTestProject>true</IsTestProject>' }
        "<Project><PropertyGroup>$property</PropertyGroup></Project>" |
            Set-Content -LiteralPath (Join-Path $projectDirectory ($entry.Key + '.csproj')) -Encoding UTF8
    }
    $Projects | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $caseRoot 'behavior.json') -Encoding UTF8
    @'
param([string] $Suite = '*', [switch] $List)
$global:runnerFixtureRoot = $PSScriptRoot
function dotnet {
    $arguments = @($args)
    $projectPath = if ($arguments[0] -eq 'run') { $arguments[2] } else { $arguments[1] }
    $name = [IO.Path]::GetFileNameWithoutExtension($projectPath)
    $behavior = (Get-Content -LiteralPath (Join-Path $global:runnerFixtureRoot 'behavior.json') -Raw | ConvertFrom-Json).$name
    [pscustomobject]@{ Name = $name; WorkingDirectory = (Get-Location).Path; Arguments = $arguments } |
        ConvertTo-Json -Compress | Add-Content -LiteralPath (Join-Path $global:runnerFixtureRoot 'calls.jsonl')
    $global:LASTEXITCODE = if ($behavior -eq 'Fail') { 7 } else { 0 }
    if ($behavior -eq 'Fail') { Write-Error 'Simulated native stderr'; return }
    if ($arguments[0] -eq 'test' -and $behavior -ne 'MissingTrx') {
        $resultDirectory = $arguments[[Array]::IndexOf($arguments, '--results-directory') + 1]
        $executed = if ($behavior -eq 'Empty') { 0 } else { 2 }
        $failed = if ($behavior -eq 'FailedTrx') { 1 } else { 0 }
        "<TestRun xmlns='http://microsoft.com/schemas/VisualStudio/TeamTest/2010'><ResultSummary><Counters executed='$executed' failed='$failed' error='0'/></ResultSummary></TestRun>" |
            Set-Content -LiteralPath (Join-Path $resultDirectory 'results.trx')
    }
    Write-Output "Simulated $name"
}
& (Join-Path $PSScriptRoot 'scripts/test.ps1') -NoRestore -Suite $Suite -List:$List
exit $LASTEXITCODE
'@ | Set-Content -LiteralPath (Join-Path $caseRoot 'invoke.ps1') -Encoding UTF8
    try {
        $ErrorActionPreference = 'Continue'
        $output = & $shellPath -NoProfile -ExecutionPolicy Bypass -File (Join-Path $caseRoot 'invoke.ps1') @ExtraArguments 2>&1
        $code = $LASTEXITCODE
    }
    finally {
        $ErrorActionPreference = 'Stop'
    }
    $output | Set-Content -LiteralPath (Join-Path $caseRoot 'runner-output.log') -Encoding UTF8
    $summary = @(Get-ChildItem -LiteralPath $caseRoot -Filter 'summary.json' -Recurse -File |
        ForEach-Object {
            foreach ($result in (Get-Content -LiteralPath $_.FullName -Raw | ConvertFrom-Json)) { $result }
        })
    $callsPath = Join-Path $caseRoot 'calls.jsonl'
    $calls = @(if (Test-Path -LiteralPath $callsPath) {
        Get-Content -LiteralPath $callsPath | ForEach-Object { $_ | ConvertFrom-Json }
    })
    [pscustomobject]@{ ExitCode = $code; Results = $summary; Calls = $calls; Root = $caseRoot }
}

$passing = Run-Fixture 'passing with spaces' @{ 'A.SmokeTests' = 'Pass'; 'B.Test' = 'Pass' }
Check ($passing.ExitCode -eq 0 -and $passing.Results.Count -eq 2) 'A passing run must execute both test kinds and return zero.'
Check (@($passing.Results | Where-Object { $_.Status -ne 'Passed' }).Count -eq 0) 'Passing suites were misclassified.'
Check (@($passing.Calls | Where-Object { $_.WorkingDirectory -ne $passing.Root }).Count -eq 0) 'Suites must run from their repository root.'
Check (@($passing.Calls | Where-Object { $_.Arguments -notcontains '--no-restore' }).Count -eq 0) 'NoRestore was not forwarded.'

$mixed = Run-Fixture 'mixed' @{ 'A.Fails.SmokeTests' = 'Fail'; 'B.EmptyTest' = 'Empty'; 'C.MissingTest' = 'MissingTrx'; 'Z.After.SmokeTests' = 'Pass' }
Check ($mixed.ExitCode -eq 1 -and $mixed.Results.Count -eq 4) 'Failures must return nonzero without dropping later suites.'
Check (($mixed.Results | Where-Object { $_.Name -eq 'A.Fails.SmokeTests' }).ExitCode -eq 7) 'The original failure exit code must be recorded.'
Check (($mixed.Results | Where-Object { $_.Name -eq 'B.EmptyTest' }).Status -eq 'Empty') 'Zero executed tests must not be marked passed.'
Check (($mixed.Results | Where-Object { $_.Name -eq 'C.MissingTest' }).Status -eq 'Failed') 'A missing TRX must not be marked passed.'
Check (($mixed.Results | Where-Object { $_.Name -eq 'Z.After.SmokeTests' }).Status -eq 'Passed') 'Execution must continue after a failed suite.'

$failedTrx = Run-Fixture 'failed-trx' @{ 'A.Test' = 'FailedTrx' }
Check ($failedTrx.ExitCode -eq 1 -and $failedTrx.Results[0].Status -eq 'Failed') 'A failed TRX must override a zero process exit.'

$filtered = Run-Fixture 'filtered' @{ 'A.SmokeTests' = 'Pass'; 'B.SmokeTests' = 'Fail' } @('-Suite', 'A.*')
Check ($filtered.ExitCode -eq 0 -and $filtered.Calls.Count -eq 1 -and $filtered.Calls[0].Name -eq 'A.SmokeTests') 'Suite filtering ran an unselected project.'
$unmatched = Run-Fixture 'unmatched' @{ 'A.SmokeTests' = 'Pass' } @('-Suite', 'DoesNotExist')
Check ($unmatched.ExitCode -ne 0 -and $unmatched.Calls.Count -eq 0) 'An unmatched selection must fail before executing anything.'
$listed = Run-Fixture 'list-only' @{ 'A.SmokeTests' = 'Fail' } @('-List')
Check ($listed.ExitCode -eq 0 -and $listed.Calls.Count -eq 0) 'List must not execute, restore or build test projects.'

Write-Host "PASS: $checks test runner checks. Fixtures: $fixtureRoot"
