using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;
using Aban360.PaymentPool.Persistence.Dapper;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Aban360.PaymentPool.Persistence.Features.BankReceipt.Implementations
{
    internal sealed class BankReceiptDiscrepancyRepository : AbstractBaseConnection, IBankReceiptDiscrepancyRepository
    {
        public BankReceiptDiscrepancyRepository(IConfiguration configuration) : base(configuration) { }
        public async Task<BankReceiptDiscrepancyReportDto> Report(DateTime fromUtc, DateTime toUtcExclusive,
            Guid userId, int pageNumber, int pageSize, bool? hasDescription, bool? isActive, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnection;
            using var result = await connection.QueryMultipleAsync(new CommandDefinition(@"
                SELECT COUNT(*) FROM [PaymentPool].[BankReceiptDiscrepancy]
                WHERE UserId=@userId AND UploadedAt>=@fromUtc AND UploadedAt<@toUtcExclusive
                    AND (@hasDescription IS NULL OR CASE WHEN Description IS NULL THEN 0 ELSE 1 END=@hasDescription)
                    AND (@isActive IS NULL OR IsActive=@isActive);
                SELECT Snapshot, IsActive, Description, DescribedBy, DescribedAt
                FROM [PaymentPool].[BankReceiptDiscrepancy]
                WHERE UserId=@userId AND UploadedAt>=@fromUtc AND UploadedAt<@toUtcExclusive
                    AND (@hasDescription IS NULL OR CASE WHEN Description IS NULL THEN 0 ELSE 1 END=@hasDescription)
                    AND (@isActive IS NULL OR IsActive=@isActive)
                ORDER BY UploadedAt DESC, ImportId, LineNumber
                OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY;",
                new { fromUtc, toUtcExclusive, userId, hasDescription, isActive, offset = (pageNumber - 1) * pageSize, pageSize },
                cancellationToken: cancellationToken));
            int total = await result.ReadSingleAsync<int>();
            var records = await result.ReadAsync<DiscrepancyRecord>();
            var items = records.Select(record =>
            {
                var item = JsonSerializer.Deserialize<BankReceiptDiscrepancy>(record.Snapshot)!;
                item.IsActive = record.IsActive;
                item.Description = record.Description;
                item.DescribedBy = record.DescribedBy;
                item.DescribedAt = record.DescribedAt;
                return item;
            }).ToArray();
            return new(total, pageNumber, pageSize, items);
        }
        public async Task Describe(Guid importId, int lineNumber, string description, Guid userId, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnection;
            int changed = await connection.ExecuteAsync(new CommandDefinition(@"
                UPDATE [PaymentPool].[BankReceiptDiscrepancy]
                SET Description=@description, DescribedBy=@userId, DescribedAt=@now
                WHERE ImportId=@importId AND LineNumber=@lineNumber AND UserId=@userId
                    AND IsActive=1 AND Description IS NULL;",
                new { importId, lineNumber, description, userId, now = DateTime.UtcNow }, cancellationToken: cancellationToken));
            if (changed != 1)
                throw new BaseException("مغایرت فعال یافت نشد، دسترسی ندارید یا شرح آن قبلاً ثبت شده است؛ شرح قابل تغییر نیست.");
        }
        private sealed class DiscrepancyRecord
        {
            public string Snapshot { get; set; } = string.Empty;
            public bool IsActive { get; set; }
            public string? Description { get; set; }
            public Guid? DescribedBy { get; set; }
            public DateTime? DescribedAt { get; set; }
        }
    }
}
