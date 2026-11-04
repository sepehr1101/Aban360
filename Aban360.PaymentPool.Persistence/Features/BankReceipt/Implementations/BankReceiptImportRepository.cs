using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;
using Aban360.PaymentPool.Persistence.Dapper;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Aban360.PaymentPool.Persistence.Features.BankReceipt.Implementations
{
    internal sealed class BankReceiptImportRepository : AbstractBaseConnection, IBankReceiptImportRepository
    {
        public BankReceiptImportRepository(IConfiguration configuration) : base(configuration) { }

        public async Task Create(BankReceiptImport import, CancellationToken cancellationToken)
        {
            using var connection = _sqlReportConnection;
            try
            {
                await connection.ExecuteAsync(new CommandDefinition(@"
                    USE [Atlas];
                    INSERT INTO [PaymentPool].[BankReceiptImport]
                        (Id, UserId, FileHash, Status, Version, Payload, BankId, UploadedAt)
                    VALUES (@Id, @UserId, @FileHash, @Status, @Version, @Payload, @BankId, @UploadedAt)",
                    new { import.Id, import.UserId, import.FileHash, import.BankId, UploadedAt = import.CreatedAt, Status = (int)import.Status, import.Version, Payload = JsonSerializer.Serialize(import) },
                    cancellationToken: cancellationToken));
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                throw new BaseException("این فایل قبلاً بارگذاری شده است.");
            }
        }

        public async Task<BankReceiptImport> Get(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            using var connection = _sqlReportConnection;
            var payload = await connection.QuerySingleOrDefaultAsync<string>(new CommandDefinition(@"
                USE [Atlas];
                SELECT Payload FROM [PaymentPool].[BankReceiptImport] WHERE Id=@id AND UserId=@userId AND IsDeleted=0",
                new { id, userId }, cancellationToken: cancellationToken));
            return payload == null ? throw new BaseException("فایل یافت نشد یا دسترسی ندارید.")
                : JsonSerializer.Deserialize<BankReceiptImport>(payload)!;
        }

        public async Task<IReadOnlyList<BankReceiptFileListItemDto>> List(Guid userId, int page, int pageSize, CancellationToken cancellationToken)
        {
            using var connection = _sqlReportConnection;
            var files = await connection.QueryAsync<BankReceiptFileListItemDto>(new CommandDefinition(@"
                USE [Atlas];
                WITH FilePage AS (
                    SELECT Id, BankId, UploadedAt, Status, Payload
                    FROM [PaymentPool].[BankReceiptImport]
                    WHERE UserId=@userId AND IsDeleted=0
                    ORDER BY UploadedAt DESC, Id DESC
                    OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY
                )
                SELECT f.Id, f.BankId, f.UploadedAt, f.Status,
                       JSON_VALUE(f.Payload, '$.FileName') AS FileName,
                       JSON_VALUE(f.Payload, '$.BankName') AS BankName,
                       CASE WHEN f.Status <> 0 THEN TRY_CONVERT(int, JSON_VALUE(f.Payload, '$.DataRowCount')) END AS RecordCount,
                       CASE WHEN f.Status <> 0 THEN COALESCE(t.Amount, 0) END AS Amount
                FROM FilePage f
                OUTER APPLY (
                    SELECT SUM(r.Amount) AS Amount
                    FROM OPENJSON(CASE WHEN f.Status <> 0 THEN JSON_QUERY(f.Payload, '$.Rows') END)
                    WITH (Amount bigint '$.Amount') r
                ) t
                ORDER BY f.UploadedAt DESC, f.Id DESC",
                new { userId, offset = (long)(page - 1) * pageSize, pageSize }, cancellationToken: cancellationToken));
            return files.AsList();
        }

        public async Task Save(BankReceiptImport import, int expectedVersion, CancellationToken cancellationToken)
        {
            using var connection = _sqlReportConnection;
            await connection.OpenAsync(cancellationToken);
            using var transaction = connection.BeginTransaction();
            import.Version = expectedVersion + 1;
            int count = await connection.ExecuteAsync(new CommandDefinition(@"
                USE [Atlas];
                UPDATE [PaymentPool].[BankReceiptImport] SET Payload=@Payload, Version=@Version, Status=@Status
                WHERE Id=@Id AND UserId=@UserId AND Version=@expectedVersion AND Status IN (0, 1, 2) AND IsDeleted=0",
                new { import.Id, import.UserId, import.Version, Status = (int)import.Status, Payload = JsonSerializer.Serialize(import), expectedVersion },
                transaction, cancellationToken: cancellationToken));
            if (count != 1) throw new BaseException("وضعیت فایل تغییر کرده است. اطلاعات را دوباره دریافت کنید.");
            await connection.ExecuteAsync(new CommandDefinition(@"
                USE [Atlas];
                UPDATE [PaymentPool].[BankReceiptDiscrepancy] SET IsActive=0 WHERE ImportId=@Id",
                new { import.Id }, transaction, cancellationToken: cancellationToken));
            var discrepancies = import.Rows.Where(r => r.IsExcluded).Select(row => new
            {
                ImportId = import.Id, row.LineNumber, import.UserId, UploadedAt = import.CreatedAt,
                Snapshot = JsonSerializer.Serialize(new BankReceiptDiscrepancy
                {
                    ImportId = import.Id, LineNumber = row.LineNumber, UploadedAt = import.CreatedAt,
                    BankName = import.BankName, FileName = import.FileName, BillId = row.BillId,
                    PaymentId = row.PaymentId, PayDate = row.PayDate, Amount = row.Amount,
                    RawData = row.RawData, IsActive = true, Reasons = row.ExclusionReasons
                })
            }).ToArray();
            if (discrepancies.Length != 0)
                await connection.ExecuteAsync(new CommandDefinition(@"
                    USE [Atlas];
                    UPDATE [PaymentPool].[BankReceiptDiscrepancy] SET Snapshot=@Snapshot, IsActive=1
                    WHERE ImportId=@ImportId AND LineNumber=@LineNumber;
                    IF @@ROWCOUNT=0
                        INSERT INTO [PaymentPool].[BankReceiptDiscrepancy]
                            (ImportId, LineNumber, UserId, UploadedAt, IsActive, Snapshot)
                        VALUES (@ImportId, @LineNumber, @UserId, @UploadedAt, 1, @Snapshot);",
                    discrepancies, transaction, cancellationToken: cancellationToken));
            transaction.Commit();
        }

        public async Task<IReadOnlySet<string>> FindCommittedPayments(IEnumerable<BankReceiptRow> rows, CancellationToken cancellationToken)
        {
            using var connection = _sqlReportConnection;
            var found = new HashSet<string>();
            var payments = rows.ToArray();
            foreach (var batch in payments.Select(r => r.Hash).Distinct().Chunk(1000))
            {
                var items = await connection.QueryAsync<string>(new CommandDefinition(@"
                    USE [Atlas];
                    SELECT Hash FROM [PaymentPool].[BankReceiptPaymentKey] WHERE Hash IN @batch",
                    new { batch }, cancellationToken: cancellationToken));
                found.UnionWith(items);
            }
            // Older import paths may have populated Credit before payment keys existed.
            foreach (var ids in payments.SelectMany(r => new[] { r.BillId, r.BillId.PadLeft(13, '0') }).Distinct().Chunk(1000))
            {
                var existing = await connection.QueryAsync<BankReceiptRow>(new CommandDefinition(@"
                    USE [Atlas];
                    SELECT BillId, PaymentId FROM [PaymentPool].[Credit]
                    WHERE BillId IN @ids AND ValidTo IS NULL", new { ids }, cancellationToken: cancellationToken));
                foreach (var item in existing)
                    found.Add(Convert.ToHexString(SHA256.HashData(Encoding.ASCII.GetBytes(
                        item.BillId.TrimStart('0') + ":" + item.PaymentId.TrimStart('0')))));
            }
            return found;
        }

        public async Task<int> Commit(BankReceiptImport import, int expectedVersion, CancellationToken cancellationToken)
        {
            var payments = import.Rows.Where(r => r.CanPost).ToArray();
            if (import.Status != BankReceiptImportStatus.Reconciled || import.HasBlockingFileIssues || payments.Length == 0)
                throw new BaseException("فایل آماده ثبت نیست.");
            using var connection = _sqlReportConnection;
            await connection.OpenAsync(cancellationToken);
            using var transaction = connection.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                // Claim this exact version before writing any payment. Rollback releases the claim.
                int claimed = await connection.ExecuteAsync(new CommandDefinition(@"
                    USE [Atlas];
                    UPDATE [PaymentPool].[BankReceiptImport] SET Status=3
                    WHERE Id=@Id AND UserId=@UserId AND Version=@expectedVersion AND Status=2 AND IsDeleted=0",
                    new { import.Id, import.UserId, expectedVersion }, transaction, cancellationToken: cancellationToken));
                if (claimed != 1) throw new BaseException("فایل ثبت شده یا وضعیت آن تغییر کرده است.");
                var uploaderId = await connection.QuerySingleAsync<int>(new CommandDefinition(@"
                    USE [Atlas];
                    INSERT INTO [PaymentPool].[Uploader]
                        (UserId, Username, BankId, InsertDateTime, InsertRecordCount, Amount)
                    OUTPUT INSERTED.Id
                    VALUES (@UserId, @Username, @BankId, @CreatedAt, @Count, @Amount)",
                    new { import.UserId, import.Username, import.BankId, CreatedAt = DateTime.UtcNow,
                        Count = payments.Length, Amount = payments.Sum(r => r.Amount) },
                    transaction, cancellationToken: cancellationToken));
                await connection.ExecuteAsync(new CommandDefinition(@"
                    USE [Atlas];
                    CREATE TABLE #BankReceiptPayments (
                        Hash varchar(64) NOT NULL,
                        LineNumber int NOT NULL,
                        BillId nvarchar(13) COLLATE DATABASE_DEFAULT NOT NULL,
                        PaymentId nvarchar(13) COLLATE DATABASE_DEFAULT NOT NULL,
                        InvoiceId bigint NOT NULL,
                        InvoiceInstallmentId bigint NOT NULL,
                        Amount bigint NOT NULL,
                        ChannelType smallint NOT NULL,
                        InsertLogInfo nvarchar(max) NOT NULL
                    );", transaction: transaction, cancellationToken: cancellationToken));

                using var batch = new DataTable();
                batch.Columns.Add("Hash", typeof(string));
                batch.Columns.Add("LineNumber", typeof(int));
                batch.Columns.Add("BillId", typeof(string));
                batch.Columns.Add("PaymentId", typeof(string));
                batch.Columns.Add("InvoiceId", typeof(long));
                batch.Columns.Add("InvoiceInstallmentId", typeof(long));
                batch.Columns.Add("Amount", typeof(long));
                batch.Columns.Add("ChannelType", typeof(short));
                batch.Columns.Add("InsertLogInfo", typeof(string));
                foreach (var row in payments)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    batch.Rows.Add(row.Hash, row.LineNumber, row.BillId, row.PaymentId, row.InvoiceId!.Value,
                        row.InvoiceInstallmentId!.Value, row.Amount, row.ChannelType,
                        JsonSerializer.Serialize(new { import.Id, import.UserId, row.LineNumber, row.PayDate, row.ReferenceCode, row.BranchCode }));
                }

                using (var bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                {
                    bulk.DestinationTableName = "#BankReceiptPayments";
                    foreach (DataColumn column in batch.Columns)
                        bulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    await bulk.WriteToServerAsync(batch, cancellationToken);
                }

                await connection.ExecuteAsync(new CommandDefinition(@"
                    USE [Atlas];
                    IF EXISTS (
                        SELECT 1
                        FROM #BankReceiptPayments p
                        INNER JOIN [PaymentPool].[Credit] c
                            ON c.BillId IN (p.BillId, RIGHT(REPLICATE('0', 13) + p.BillId, 13))
                            AND c.PaymentId IN (p.PaymentId, RIGHT(REPLICATE('0', 13) + p.PaymentId, 13))
                        WHERE c.ValidTo IS NULL
                    )
                        THROW 51020, N'پرداخت تکراری شناسایی شد.', 1;

                    INSERT INTO [PaymentPool].[BankReceiptPaymentKey] (Hash, ImportId, LineNumber)
                    SELECT Hash, @ImportId, LineNumber FROM #BankReceiptPayments;

                    INSERT INTO [PaymentPool].[Credit]
                        (BillId, PaymentId, InvoiceId, InvoiceInstallmentId, Amount, UploaderId,
                         CreditorTypeId, PaymentMethodId, ValidFrom, InsertLogInfo, Hash)
                    SELECT BillId, PaymentId, InvoiceId, InvoiceInstallmentId, Amount, @uploaderId,
                           1, ChannelType, @now, InsertLogInfo, Hash
                    FROM #BankReceiptPayments;",
                    new { ImportId = import.Id, uploaderId, now = DateTime.UtcNow },
                    transaction, cancellationToken: cancellationToken));
                import.UploaderId = uploaderId;
                import.Status = BankReceiptImportStatus.Committed;
                import.Version = expectedVersion + 1;
                await connection.ExecuteAsync(new CommandDefinition(@"
                    USE [Atlas];
                    UPDATE [PaymentPool].[BankReceiptImport] SET Payload=@payload, Version=@Version WHERE Id=@Id",
                    new { import.Id, import.Version, payload = JsonSerializer.Serialize(import) }, transaction, cancellationToken: cancellationToken));
                transaction.Commit();
                return uploaderId;
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627 or 51020)
            {
                transaction.Rollback();
                throw new BaseException("حداقل یکی از پرداخت‌ها قبلاً ثبت شده است. مغایرت‌گیری را دوباره اجرا کنید.");
            }
        }
    }
}
