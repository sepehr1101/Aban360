using System.Data;
using Aban360.PaymentPool.Persistence.Dapper;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Aban360.Common.Db.Dapper;

namespace Aban360.PaymentPool.Persistence.Features.BankReceipt.Implementations
{
    internal sealed class BankReceiptDeleteRepository : AbstractBaseConnection, IBankReceiptDeleteRepository
    {
        public BankReceiptDeleteRepository(IConfiguration configuration) : base(configuration) { }
        public async Task<BankReceiptDeleteResultDto> Delete(short bankId, DateTime fromUtc, DateTime toUtcExclusive,
            Guid userId, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection();// _sqlConnection;
            await connection.OpenAsync(cancellationToken);
            using var transaction = connection.BeginTransaction(IsolationLevel.Serializable);
            var result = await connection.QuerySingleAsync<BankReceiptDeleteResultDto>(new CommandDefinition(@"
                DECLARE @Deleted TABLE (Id uniqueidentifier PRIMARY KEY, PreviousStatus int);
                UPDATE [PaymentPool].[BankReceiptImport]
                SET IsDeleted=1, DeletedAt=@now, DeletedBy=@userId, Version=Version+1
                OUTPUT INSERTED.Id, DELETED.Status INTO @Deleted (Id, PreviousStatus)
                WHERE UserId=@userId AND BankId=@bankId AND UploadedAt>=@fromUtc AND UploadedAt<@toUtcExclusive
                    AND IsDeleted=0;
                UPDATE d SET IsActive=0
                FROM [PaymentPool].[BankReceiptDiscrepancy] d
                INNER JOIN @Deleted f ON f.Id=d.ImportId;
                SELECT COUNT(*) AS DeletedFileCount,
                    COALESCE(SUM(CASE WHEN PreviousStatus=3 THEN 1 ELSE 0 END), 0) AS CommittedFileCount
                FROM @Deleted;",
                new { bankId, fromUtc, toUtcExclusive, userId, now = DateTime.UtcNow }, transaction, cancellationToken: cancellationToken));
            transaction.Commit();
            return result;
        }
    }
}
