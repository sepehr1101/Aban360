param([switch]$SkipBuild, [switch]$All50, [switch]$All80)

$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$testProject = Join-Path $projectRoot 'tests/Aban360.PersistenceMerge20.RegressionTests/Aban360.PersistenceMerge20.RegressionTests.csproj'
$previousTelemetry = $env:DOTNET_CLI_TELEMETRY_OPTOUT
Push-Location $projectRoot
try {
    $env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
    if (-not $SkipBuild) {
        dotnet build Aban360.Api/Aban360.Api.csproj --no-restore -m:1 -p:UseSharedCompilation=false -p:NuGetAudit=false -nodeReuse:false
        if ($LASTEXITCODE -ne 0) { throw 'API build failed.' }
    }
    dotnet restore $testProject --ignore-failed-sources -p:NuGetAudit=false
    if ($LASTEXITCODE -ne 0) { throw 'Regression project restore failed.' }
    dotnet build $testProject --no-restore -m:1 -p:UseSharedCompilation=false -nodeReuse:false
    if ($LASTEXITCODE -ne 0) { throw 'Regression project build failed.' }
    $regressionArguments = @()
    if ($All80) { $regressionArguments += '--all80' }
    elseif ($All50) { $regressionArguments += '--all50' }
    dotnet tests/Aban360.PersistenceMerge20.RegressionTests/bin/Debug/net10.0/Aban360.PersistenceMerge20.RegressionTests.dll @regressionArguments
    if ($LASTEXITCODE -ne 0) { throw 'Persistence regression checks failed.' }
}
finally {
    $env:DOTNET_CLI_TELEMETRY_OPTOUT = $previousTelemetry
    Pop-Location
}
