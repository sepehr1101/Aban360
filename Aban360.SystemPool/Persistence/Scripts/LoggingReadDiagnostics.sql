-- Read-only metadata inspection. Run on the server serving the failing APIs.
-- These queries do not read log messages or request bodies.
SET LOCK_TIMEOUT 5000;

SELECT DB_NAME() AS CurrentDatabase, @@VERSION AS SqlServerVersion;

SELECT c.name AS ColumnName, t.name AS DataType, c.max_length
FROM [Aban360].sys.columns c
JOIN [Aban360].sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID(N'[Aban360].dbo.Logs');

SELECT i.name AS IndexName, i.type_desc, i.is_disabled,
       c.name AS ColumnName, ic.key_ordinal, ic.is_included_column
FROM [Aban360].sys.indexes i
JOIN [Aban360].sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
JOIN [Aban360].sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
WHERE i.object_id = OBJECT_ID(N'[Aban360].dbo.Logs')
ORDER BY i.index_id, ic.index_column_id;

IF HAS_PERMS_BY_NAME(NULL, NULL, 'VIEW SERVER STATE') = 1
   OR HAS_PERMS_BY_NAME(NULL, NULL, 'VIEW SERVER PERFORMANCE STATE') = 1
BEGIN
    SELECT session_id, DB_NAME(database_id) AS DatabaseName, status, command,
           blocking_session_id, wait_type, wait_time, total_elapsed_time,
           cpu_time, logical_reads
    FROM sys.dm_exec_requests
    WHERE session_id <> @@SPID
      AND database_id IN (DB_ID(N'Aban360'), DB_ID(N'CustomerWarehouse'));
END;

SELECT SUM(p.rows) AS ApproximateLogRows
FROM [Aban360].sys.partitions p
WHERE p.object_id = OBJECT_ID(N'[Aban360].dbo.Logs') AND p.index_id IN (0, 1);

SELECT i.name AS IndexName, c.name AS ColumnName, ic.key_ordinal, ic.is_included_column
FROM [CustomerWarehouse].sys.indexes i
JOIN [CustomerWarehouse].sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
JOIN [CustomerWarehouse].sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
WHERE i.object_id = OBJECT_ID(N'[CustomerWarehouse].dbo.OpLog')
ORDER BY i.index_id, ic.index_column_id;
