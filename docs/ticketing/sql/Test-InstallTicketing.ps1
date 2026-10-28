# Static verification only. Does not connect to SQL Server or execute SQL.
$ErrorActionPreference = 'Stop'
$workspacePath = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../../..'))
$migrationPath = Join-Path $workspacePath 'Aban360.UserPool.Persistence/Migrations/1405_08_04_Ticketing.cs'
$installPath = Join-Path $PSScriptRoot 'InstallTicketing.sql'
$migrationText = [IO.File]::ReadAllText($migrationPath)
$installText = [IO.File]::ReadAllText($installPath)
$upText = ($migrationText -split 'public override void Down\(\)')[0]
$sqlBlocks = [regex]::Matches($upText, 'Execute\.Sql\(\$?"""\r?\n([\s\S]*?)\r?\n\s*"""\);')
if ($sqlBlocks.Count -ne 2) { throw 'Expected two SQL blocks in migration Up().' }
$ddlMatch = [regex]::Match($installText, "DECLARE @CreateTablesSql nvarchar\(max\) = N'((?:[^']|'')*)';\s*EXEC sys\.sp_executesql @CreateTablesSql;")
if (!$ddlMatch.Success) { throw 'Installer DDL is not correctly quoted.' }
$actualDdl = $ddlMatch.Groups[1].Value.Replace("''", "'")
$expectedDdl = $sqlBlocks[1].Groups[1].Value.Replace('{TableSchema.Name}', 'UserPool')
function Normalize-SqlLines([string] $value) {
    return (($value -split '\r?\n' | ForEach-Object { $_.Trim() } | Where-Object { $_ }) -join "`n")
}
if ((Normalize-SqlLines $actualDdl) -cne (Normalize-SqlLines $expectedDdl)) {
    throw 'Standalone table definitions differ from migration Up().'
}
$expectedCounts = [ordered]@{
    'CREATE TABLE' = 5
    'CONSTRAINT \[FK_' = 8
    'REFERENCES \[UserPool\]\.\[User\]' = 4
    'CREATE INDEX' = 8
    'CONSTRAINT \[UQ_' = 4
    'CONSTRAINT \[CK_' = 2
}
foreach ($pattern in $expectedCounts.Keys) {
    if ([regex]::Matches($actualDdl, $pattern).Count -ne $expectedCounts[$pattern]) {
        throw "Unexpected structure count for $pattern."
    }
}
if ($installText -match '(?im)^\s*(DROP|DELETE|TRUNCATE|ALTER\s+(DATABASE|TABLE)|UPDATE)\b') {
    throw 'Unexpected destructive or existing-schema-changing statement.'
}
if ($installText -match '(?im)^\s*GO\s*$') { throw 'The installer must remain one batch.' }
foreach ($guard in @('DB_NAME() <> @ExpectedDatabase', '@@TRANCOUNT <> 0', '(2 & @@OPTIONS) = 2')) {
    $position = $installText.IndexOf($guard)
    if ($position -lt 0 -or $position -ge $installText.IndexOf('BEGIN TRANSACTION;')) {
        throw "Required preflight guard is missing or too late: $guard"
    }
}
if ($installText.IndexOf('(2 & @@OPTIONS) = 2') -ge $installText.IndexOf('FROM sys.columns')) {
    throw 'Implicit transaction guard must precede metadata table reads.'
}
$validationPosition = $installText.IndexOf('-- Validate the new structure BEFORE committing it.')
if ($validationPosition -lt 0 -or $validationPosition -ge $installText.IndexOf('COMMIT TRANSACTION;')) {
    throw 'Structure validation must precede commit.'
}
if ($installText.IndexOf('IF @@TRANCOUNT <> 0') -ge $installText.IndexOf('THROW 51100') -or
    !$installText.Contains("RAISERROR ('Run outside an existing transaction.") -or
    !$installText.Contains('    RETURN;')) {
    throw 'An existing caller transaction must be rejected without THROW or any writes.'
}
if (!$installText.Contains('IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;')) {
    throw 'Rollback on installation failure is missing.'
}
Write-Output 'PASS: DDL matches migration; 5 tables, 8 foreign keys (4 to User), 8 indexes, 4 unique constraints, 2 checks.'
Write-Output 'PASS: target database, transaction, rollback and non-destructive statement checks.'
Write-Output 'Static verification only: SQL Server parsing/execution and FILESTREAM were not tested.'
