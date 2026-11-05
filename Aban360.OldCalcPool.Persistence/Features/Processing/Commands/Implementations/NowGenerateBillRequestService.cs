using System.Security.Cryptography;
using System.Text;
using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Constants;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    internal sealed class NowGenerateBillRequestService : AbstractBaseConnection, INowGenerateBillRequestService
    {
        public NowGenerateBillRequestService(IConfiguration configuration) : base(configuration) { }

        public async Task<NowGenerateBillErrorCodeEnum> Register(NowGenerateBillInputDto input, CancellationToken cancellationToken)
        {
            string? mobile = input.MobileNumber?.Trim();
            string? billId = input.BillId?.Trim();
            await using var connection = _sqlReportConnection;
            await connection.OpenAsync(cancellationToken);
            await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
            var command = new CommandDefinition(Sql, new
            {
                MobileNumber = mobile,
                BillId = billId,
                input.CurrentMeterNumber,
                MobileLock = LockKey("mobile", mobile),
                BillLock = LockKey("bill", billId),
                MobileLimitCode = (int)NowGenerateBillErrorCodeEnum.MobileDailyLimitReached,
                BillLimitCode = (int)NowGenerateBillErrorCodeEnum.BillDailyLimitReached
            }, transaction, cancellationToken: cancellationToken);
            int result = await connection.QuerySingleAsync<int>(command);
            await transaction.CommitAsync(cancellationToken);
            return (NowGenerateBillErrorCodeEnum)result;
        }

        private static string LockKey(string kind, string? value) =>
            $"NowGenerateBill:{kind}:{Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value ?? string.Empty)))}";

        private const string Sql = """
            SET NOCOUNT ON;
            DECLARE @LockResult int;
            -- Always acquire mobile before bill; release both when this transaction ends.
            EXEC @LockResult = [Atlas].sys.sp_getapplock
                @Resource = @MobileLock, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 10000;
            IF @LockResult < 0 THROW 51070, 'Could not acquire mobile request lock.', 1;
            EXEC @LockResult = [Atlas].sys.sp_getapplock
                @Resource = @BillLock, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 10000;
            IF @LockResult < 0 THROW 51071, 'Could not acquire bill request lock.', 1;

            DECLARE @Now datetime2(3) = SYSDATETIME();
            DECLARE @Today date = CONVERT(date, @Now);
            DECLARE @ErrorCode int = 0;
            IF (SELECT COUNT_BIG(*) FROM [Atlas].dbo.NowGenerateBillRequest
                WHERE RequestDate = @Today AND MobileNumber = @MobileNumber COLLATE Latin1_General_100_BIN2) >= 10
                SET @ErrorCode = @MobileLimitCode;
            ELSE IF (SELECT COUNT_BIG(*) FROM [Atlas].dbo.NowGenerateBillRequest
                WHERE RequestDate = @Today AND BillId = @BillId COLLATE Latin1_General_100_BIN2) >= 3
                SET @ErrorCode = @BillLimitCode;

            INSERT INTO [Atlas].dbo.NowGenerateBillRequest
                (RequestedAt, RequestDate, MobileNumber, BillId, CurrentMeterNumber, LimitErrorCode)
            VALUES (@Now, @Today, @MobileNumber, @BillId, @CurrentMeterNumber, @ErrorCode);
            SELECT @ErrorCode;
            """;
    }
}
