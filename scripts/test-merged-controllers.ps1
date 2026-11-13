[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')][string] $Configuration = 'Debug',
    [switch] $NoRestore,
    [switch] $List
)
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$names = @('Aban360.Controller*', 'Aban360.CriticalControllers.*', 'Aban360.MergedControllers.*', 'Aban360.OnlineUsers.*', 'Aban360.Ticketing.*')
if ($List) { & (Join-Path $PSScriptRoot 'test.ps1') -List -Suite $names; exit $LASTEXITCODE }
Push-Location $root
try {
    # Build applications once, then compile/copy each executable suite without rebuilding the applications.
    $applications = @('Aban360.Api/Aban360.Api.csproj', 'Aban360.BrdigeApi/Aban360.BrdigeApi.csproj')
    $projects = Get-ChildItem -LiteralPath (Join-Path $root 'tests') -Directory |
        Where-Object { $_.Name -like 'Aban360.Controller*' -or $_.Name -like 'Aban360.CriticalControllers.*' -or $_.Name -like 'Aban360.MergedControllers.*' -or $_.Name -like 'Aban360.OnlineUsers.*' -or $_.Name -like 'Aban360.Ticketing.*' } |
        ForEach-Object { Get-ChildItem -LiteralPath $_.FullName -Filter '*.csproj' -File } | Sort-Object FullName
    foreach ($project in $applications) {
        $arguments = @('build', $project, '--configuration', $Configuration, '--nologo', '-m:1', '-v:minimal')
        if ($NoRestore) { $arguments += '--no-restore' }
        & dotnet @arguments
        if ($LASTEXITCODE -ne 0) { throw "Application build failed: $project" }
    }
    foreach ($project in $projects) {
        $arguments = @('build', $project.FullName, '--configuration', $Configuration, '--nologo', '-m:1', '-v:minimal', '-p:BuildProjectReferences=false')
        if ($NoRestore) { $arguments += '--no-restore' }
        & dotnet @arguments
        if ($LASTEXITCODE -ne 0) { throw "Test build failed: $($project.Name)" }
    }
    & (Join-Path $PSScriptRoot 'test.ps1') -Configuration $Configuration -Suite $names -NoRestore -NoBuild
    exit $LASTEXITCODE
}
finally { Pop-Location }
