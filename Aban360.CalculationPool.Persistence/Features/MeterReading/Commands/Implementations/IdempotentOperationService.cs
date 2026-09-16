using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations
{
    internal sealed class IdempotentOperationService : AbstractBaseConnection, IIdempotentOperationService
    {
        public IdempotentOperationService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IdempotentOperationResultDto> TryBegin(string operationKey, Guid lockToken)
        {
            const string command = @"
SET XACT_ABORT ON;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;

DECLARE @Status tinyint;
DECLARE @ResponseJson nvarchar(max);
DECLARE @LockedUntil datetime2;

SELECT
    @Status = Status,
    @ResponseJson = ResponseJson,
    @LockedUntil = LockedUntil
FROM [Atlas].[dbo].[IdempotentOperation] WITH (UPDLOCK, HOLDLOCK)
WHERE OperationKey = @OperationKey;

IF @Status IS NULL
BEGIN
    INSERT INTO [Atlas].[dbo].[IdempotentOperation]
        (OperationKey, Status, ResponseJson, LockToken, LockedUntil, CreatedAt, UpdatedAt)
    VALUES
        (@OperationKey, 1, NULL, @LockToken, DATEADD(HOUR, 2, SYSUTCDATETIME()), SYSUTCDATETIME(), SYSUTCDATETIME());

    SELECT CAST(1 AS bit) AS Acquired, CAST(1 AS tinyint) AS Status, CAST(NULL AS nvarchar(max)) AS ResponseJson, @LockToken AS LockToken;
END
ELSE IF @Status = 3 OR (@Status = 1 AND @LockedUntil <= SYSUTCDATETIME())
BEGIN
    UPDATE [Atlas].[dbo].[IdempotentOperation]
    SET Status = 1,
        ResponseJson = NULL,
        LockToken = @LockToken,
        LockedUntil = DATEADD(HOUR, 2, SYSUTCDATETIME()),
        UpdatedAt = SYSUTCDATETIME()
    WHERE OperationKey = @OperationKey;

    SELECT CAST(1 AS bit) AS Acquired, CAST(1 AS tinyint) AS Status, CAST(NULL AS nvarchar(max)) AS ResponseJson, @LockToken AS LockToken;
END
ELSE
BEGIN
    SELECT CAST(0 AS bit) AS Acquired, @Status AS Status, @ResponseJson AS ResponseJson, CAST(NULL AS uniqueidentifier) AS LockToken;
END

COMMIT TRANSACTION;";

            using var connection = _sqlReportConnection;
            return await connection.QuerySingleAsync<IdempotentOperationResultDto>(command, new { OperationKey = operationKey, LockToken = lockToken });
        }

        public async Task Complete(string operationKey, Guid lockToken, string responseJson, IDbConnection connection, IDbTransaction transaction)
        {
            const string command = @"
UPDATE [Atlas].[dbo].[IdempotentOperation]
SET Status = 2,
    ResponseJson = @ResponseJson,
    LockedUntil = NULL,
    UpdatedAt = SYSUTCDATETIME()
WHERE OperationKey = @OperationKey
  AND Status = 1
  AND LockToken = @LockToken;";

            int affectedRows = await connection.ExecuteAsync(
                command,
                new { OperationKey = operationKey, LockToken = lockToken, ResponseJson = responseJson },
                transaction);
            if (affectedRows != 1)
            {
                throw new InvalidOperationException("The idempotent operation lock is no longer owned by this execution.");
            }
        }

        public async Task Fail(string operationKey, Guid lockToken)
        {
            const string command = @"
UPDATE [Atlas].[dbo].[IdempotentOperation]
SET Status = 3,
    LockedUntil = NULL,
    UpdatedAt = SYSUTCDATETIME()
WHERE OperationKey = @OperationKey
  AND Status = 1
  AND LockToken = @LockToken;";

            using var connection = _sqlReportConnection;
            await connection.ExecuteAsync(command, new { OperationKey = operationKey, LockToken = lockToken });
        }
    }
}
