param(
    [string]$SqlImage
)

$ErrorActionPreference = 'Stop'
$project = Join-Path $PSScriptRoot 'Aban360.ConsolidatedPools.Tests.csproj'

$dockerOs = docker info --format '{{.OSType}}'
if ($LASTEXITCODE -ne 0) { throw 'Cannot connect to the Docker daemon.' }
if ($dockerOs -ne 'linux') { throw 'The SQL Server test image requires Docker Linux containers.' }

if ([string]::IsNullOrWhiteSpace($SqlImage)) {
    $localImages = @(docker image ls 'mcr.microsoft.com/mssql/server' --format '{{.Repository}}:{{.Tag}}')
    if ($LASTEXITCODE -ne 0) { throw 'Could not list local SQL Server images.' }
    $localImages = @($localImages | Where-Object { $_ -and $_ -notlike '*:<none>' } | Sort-Object -Unique)
    if ($localImages.Count -eq 0) {
        throw 'No local SQL Server image found. Pass the existing image name or ID with -SqlImage.'
    }
    if ($localImages.Count -gt 1) {
        throw "Multiple local SQL Server images found. Pass one with -SqlImage. Local images: $($localImages -join ', ')"
    }
    $SqlImage = $localImages[0]
}
docker image inspect $SqlImage --format '{{.Id}}' | Out-Null
if ($LASTEXITCODE -ne 0) { throw "SQL Server image '$SqlImage' is unavailable locally in the active Docker context." }

dotnet build $project --verbosity minimal
if ($LASTEXITCODE -ne 0) { throw 'Consolidated pools test build failed.' }

$oldPassword = $env:ABAN360_DOCKER_SQL_PASSWORD
$oldPort = $env:ABAN360_DOCKER_SQL_PORT
$oldSaPassword = $env:MSSQL_SA_PASSWORD
$container = $null
try {
    $env:ABAN360_DOCKER_SQL_PASSWORD = "Ab1!$([Guid]::NewGuid().ToString('N'))"
    $env:MSSQL_SA_PASSWORD = $env:ABAN360_DOCKER_SQL_PASSWORD
    $container = docker run --pull never --detach --rm --publish '127.0.0.1::1433' --env 'ACCEPT_EULA=Y' --env MSSQL_SA_PASSWORD $SqlImage
    if ($LASTEXITCODE -ne 0 -or $container -notmatch '^[0-9a-f]{64}$') {
        throw 'Could not start a disposable SQL Server container.'
    }

    $binding = docker port $container '1433/tcp'
    if ($LASTEXITCODE -ne 0 -or $binding -notmatch '^127\.0\.0\.1:(\d+)$') {
        throw 'Could not determine the SQL Server loopback port.'
    }
    $env:ABAN360_DOCKER_SQL_PORT = $Matches[1]

    dotnet run --project $project --no-build -- --docker-sql
    if ($LASTEXITCODE -ne 0) { throw 'Docker SQL Server integration tests failed.' }
}
finally {
    $env:ABAN360_DOCKER_SQL_PASSWORD = $oldPassword
    $env:ABAN360_DOCKER_SQL_PORT = $oldPort
    $env:MSSQL_SA_PASSWORD = $oldSaPassword
    if ($container -match '^[0-9a-f]{64}$') {
        docker stop $container | Out-Null
        if ($LASTEXITCODE -ne 0) { Write-Warning "Could not stop test container $container." }
    }
}
