[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Debug',
    [string[]] $Suite = @('*'),
    [switch] $NoRestore,
    [switch] $List
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path -Parent $PSScriptRoot
$suites = @()

$projects = @(
    Get-ChildItem -LiteralPath $repositoryRoot -Directory |
        ForEach-Object { Get-ChildItem -LiteralPath $_.FullName -Filter '*.csproj' -File }
    Get-ChildItem -LiteralPath (Join-Path $repositoryRoot 'tests') -Filter '*.csproj' -Recurse -File |
        Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' }
) | Sort-Object FullName -Unique

foreach ($project in $projects) {
    [xml] $definition = Get-Content -LiteralPath $project.FullName -Raw
    $isTestProject = $definition.SelectSingleNode("/Project/PropertyGroup/IsTestProject[translate(text(), 'TRUE', 'true')='true']")
    $testSdk = $definition.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='Microsoft.NET.Test.Sdk']")
    if ($project.BaseName -like '*.SmokeTests') {
        $kind = 'Smoke'
    }
    elseif ($isTestProject -or $testSdk) {
        $kind = 'Test'
    }
    else {
        continue
    }
    $suites += [pscustomobject]@{ Name = $project.BaseName; Kind = $kind; Path = $project.FullName }
}

foreach ($script in (Get-ChildItem -LiteralPath (Join-Path $repositoryRoot 'tests') -Filter '*.Tests.ps1' -File)) {
    $suites += [pscustomobject]@{ Name = $script.BaseName; Kind = 'PowerShell'; Path = $script.FullName }
}

foreach ($pattern in $Suite) {
    if (-not @($suites | Where-Object { $_.Name -like $pattern }).Count) {
        throw "No test suite matches '$pattern'. Use -List to see available suites."
    }
}
$suites = @($suites | Where-Object {
    $name = $_.Name
    @($Suite | Where-Object { $name -like $_ }).Count -gt 0
} | Sort-Object Name)
if ($suites.Count -eq 0) { throw 'No test suites were discovered.' }
if ($List) {
    $suites | Format-Table Name, Kind -AutoSize
    exit 0
}

$null = Get-Command dotnet -ErrorAction Stop
$shellPath = (Get-Process -Id $PID).Path
$runId = (Get-Date -Format 'yyyyMMdd-HHmmss') + '-' + [Guid]::NewGuid().ToString('N').Substring(0, 8)
$resultsDirectory = Join-Path $repositoryRoot "artifacts/test-results/$runId"
$null = New-Item -ItemType Directory -Path $resultsDirectory -Force
$results = @()

Push-Location $repositoryRoot
try {
    foreach ($item in $suites) {
        Write-Host "Running $($item.Name) [$($item.Kind)]"
        $suiteDirectory = Join-Path $resultsDirectory $item.Name
        $null = New-Item -ItemType Directory -Path $suiteDirectory
        $logPath = Join-Path $suiteDirectory 'output.log'
        $command = 'dotnet'
        if ($item.Kind -eq 'Smoke') {
            $arguments = @('run', '--project', $item.Path, '--configuration', $Configuration, '--no-launch-profile')
        }
        elseif ($item.Kind -eq 'Test') {
            $arguments = @('test', $item.Path, '--configuration', $Configuration,
                '--logger', 'trx;LogFileName=results.trx', '--results-directory', $suiteDirectory)
        }
        else {
            $command = $shellPath
            $arguments = @('-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', $item.Path)
        }
        if ($NoRestore -and $item.Kind -ne 'PowerShell') { $arguments += '--no-restore' }
        $timer = [Diagnostics.Stopwatch]::StartNew()
        $exitCode = 1
        $detail = ''
        try {
            # Windows PowerShell treats redirected native stderr as error records.
            $ErrorActionPreference = 'Continue'
            & $command @arguments > $logPath 2>&1
            $exitCode = $LASTEXITCODE
        }
        catch {
            $detail = $_.Exception.Message
        }
        finally {
            $ErrorActionPreference = 'Stop'
            $timer.Stop()
        }
        $status = if ($exitCode -eq 0) { 'Passed' } else { 'Failed' }
        $executedTests = $null
        if ($status -eq 'Passed' -and $item.Kind -eq 'Test') {
            $trxPath = Join-Path $suiteDirectory 'results.trx'
            try {
                [xml] $trx = Get-Content -LiteralPath $trxPath -Raw
                $counters = $trx.SelectSingleNode("//*[local-name()='ResultSummary']/*[local-name()='Counters']")
                if (-not $counters) { throw 'TRX test counters are missing.' }
                $executedTests = [int] $counters.executed
                if ($executedTests -eq 0) {
                    $status = 'Empty'
                    $detail = 'No tests were executed; this suite is not counted as passed.'
                }
                elseif ([int] $counters.failed -gt 0 -or [int] $counters.error -gt 0) {
                    $status = 'Failed'
                    $detail = 'TRX reports failed tests despite a successful process exit.'
                }
            }
            catch {
                $status = 'Failed'
                $detail = 'Unable to verify test execution: ' + $_.Exception.Message
            }
        }
        $results += [pscustomobject]@{
            Name = $item.Name; Kind = $item.Kind; Status = $status; ExitCode = $exitCode
            ExecutedTests = $executedTests; Seconds = [Math]::Round($timer.Elapsed.TotalSeconds, 2)
            Detail = $detail; Log = $logPath
        }
        Write-Host "$status : $($item.Name)"
        if ($status -ne 'Passed') {
            if ($detail) { Write-Host $detail }
            Get-Content -LiteralPath $logPath -Tail 12 -ErrorAction SilentlyContinue | ForEach-Object { Write-Host $_ }
        }
    }
}
finally {
    Pop-Location
}

$results | Format-Table Name, Status, ExecutedTests, Seconds -AutoSize
ConvertTo-Json -InputObject @($results) -Depth 4 | Set-Content -LiteralPath (Join-Path $resultsDirectory 'summary.json') -Encoding UTF8
Write-Host "Results: $resultsDirectory"
if (@($results | Where-Object { $_.Status -ne 'Passed' }).Count -gt 0) { exit 1 }
exit 0
