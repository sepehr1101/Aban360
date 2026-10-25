using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
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
            using var connection = _sqlConnection;
            try
            {
                await connection.ExecuteAsync(new CommandDefinition(@"
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
            using var connection = _sqlConnection;
            var payload = await connection.QuerySingleOrDefaultAsync<string>(new CommandDefinition(@"
                SELECT Payload FROM [PaymentPool].[BankReceiptImport] WHERE Id=@id AND UserId=@userId AND IsDeleted=0",
                new { id, userId }, cancellationToken: cancellationToken));
            return payload == null ? throw new BaseException("فایل یافت نشد یا دسترسی ندارید.")
                : JsonSerializer.Deserialize<BankReceiptImport>(payload)!;
        }

        public async Task Save(BankReceiptImport import, int expectedVersion, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnection;
            await connection.OpenAsync(cancellationToken);
            using var transaction = connection.BeginTransaction();
            import.Version = expectedVersion + 1;
            int count = await connection.ExecuteAsync(new CommandDefinition(@"
                UPDATE [PaymentPool].[BankReceiptImport] SET Payload=@Payload, Version=@Version, Status=@Status
                WHERE Id=@Id AND UserId=@UserId AND Version=@expectedVersion AND Status<>3 AND IsDeleted=0",
                new { import.Id, import.UserId, import.Version, Status = (int)import.Status, Payload = JsonSerializer.Serialize(import), expectedVersion },
                transaction, cancellationToken: cancellationToken));
            if (count != 1) throw new BaseException("وضعیت فایل تغییر کرده است. اطلاعات را دوباره دریافت کنید.");
            await connection.ExecuteAsync(new CommandDefinition(@"
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
            using var connection = _sqlConnection;
            var found = new HashSet<string>();
            var payments = rows.ToArray();
            foreach (var batch in payments.Select(r => r.Hash).Distinct().Chunk(1000))
            {
                var items = await connection.QueryAsync<string>(new CommandDefinition(@"
                    SELECT Hash FROM [PaymentPool].[BankReceiptPaymentKey] WHERE Hash IN @batch",
                    new { batch }, cancellationToken: cancellationToken));
                found.UnionWith(items);
            }
            // Older import paths may have populated Credit before payment keys existed.
            foreach (var ids in payments.SelectMany(r => new[] { r.BillId, r.BillId.PadLeft(13, '0') }).Distinct().Chunk(1000))
            {
                var existing = await connection.QueryAsync<BankReceiptRow>(new CommandDefinition(@"
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
            if (import.Status != BankReceiptImportStatus.Reconciled || import.Header == null || import.Issues.Count != 0
                || import.Rows.Count == 0 || import.Rows.Any(r => r.Issues.Count != 0 || r.InvoiceId == null || r.InvoiceInstallmentId == null))
                throw new BaseException("فایل آماده ثبت نیست.");
            using var connection = _sqlConnection;
            await connection.OpenAsync(cancellationToken);
            using var transaction = connection.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                // Claim this exact version before writing any payment. Rollback releases the claim.
                int claimed = await connection.ExecuteAsync(new CommandDefinition(@"
                    UPDATE [PaymentPool].[BankReceiptImport] SET Status=3
                    WHERE Id=@Id AND UserId=@UserId AND Version=@expectedVersion AND Status=2 AND IsDeleted=0",
                    new { import.Id, import.UserId, expectedVersion }, transaction, cancellationToken: cancellationToken));
                if (claimed != 1) throw new BaseException("فایل ثبت شده یا وضعیت آن تغییر کرده است.");
                var uploaderId = await connection.QuerySingleAsync<int>(new CommandDefinition(@"
                    INSERT INTO [PaymentPool].[Uploader]
                        (UserId, Username, BankId, InsertDateTime, InsertRecordCount, Amount)
                    OUTPUT INSERTED.Id
                    VALUES (@UserId, @Username, @BankId, @CreatedAt, @Count, @Amount)",
                    new { import.UserId, import.Username, import.BankId, CreatedAt = DateTime.UtcNow,
                        Count = import.Rows.Count, Amount = import.Rows.Sum(r => r.Amount) },
                    transaction, cancellationToken: cancellationToken));
                foreach (var row in import.Rows)
                {
                    bool exists = await connection.ExecuteScalarAsync<bool>(new CommandDefinition(@"
                        SELECT CASE WHEN EXISTS (SELECT 1 FROM [PaymentPool].[Credit]
                            WHERE BillId IN (@BillId, @PaddedBillId) AND PaymentId IN (@PaymentId, @PaddedPaymentId)
                            AND ValidTo IS NULL) THEN 1 ELSE 0 END",
                        new { row.BillId, row.PaymentId, PaddedBillId = row.BillId.PadLeft(13, '0'),
                            PaddedPaymentId = row.PaymentId.PadLeft(13, '0') }, transaction, cancellationToken: cancellationToken));
                    if (exists) throw new BaseException("این پرداخت قبلاً ثبت شده است. مغایرت‌گیری را دوباره اجرا کنید.");
                    await connection.ExecuteAsync(new CommandDefinition(@"
                        INSERT INTO [PaymentPool].[BankReceiptPaymentKey] (Hash, ImportId, LineNumber)
                        VALUES (@Hash, @ImportId, @LineNumber);
                        INSERT INTO [PaymentPool].[Credit]
                            (BillId, PaymentId, InvoiceId, InvoiceInstallmentId, Amount, UploaderId,
                             CreditorTypeId, PaymentMethodId, ValidFrom, InsertLogInfo, Hash)
                        VALUES (@BillId, @PaymentId, @InvoiceId, @InvoiceInstallmentId, @Amount, @uploaderId,
                                1, @ChannelType, @now, @log, @Hash)",
                        new { row.Hash, ImportId = import.Id, row.LineNumber, row.BillId, row.PaymentId,
                            row.InvoiceId, row.InvoiceInstallmentId, row.Amount, uploaderId, row.ChannelType,
                            now = DateTime.UtcNow, log = JsonSerializer.Serialize(new { import.Id, import.UserId, row.LineNumber, row.PayDate, row.ReferenceCode, row.BranchCode }) },
                        transaction, cancellationToken: cancellationToken));
                }
                import.UploaderId = uploaderId;
                import.Status = BankReceiptImportStatus.Committed;
                import.Version = expectedVersion + 1;
                await connection.ExecuteAsync(new CommandDefinition(@"
                    UPDATE [PaymentPool].[BankReceiptImport] SET Payload=@payload, Version=@Version WHERE Id=@Id",
                    new { import.Id, import.Version, payload = JsonSerializer.Serialize(import) }, transaction, cancellationToken: cancellationToken));
                transaction.Commit();
                return uploaderId;
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                transaction.Rollback();
                throw new BaseException("حداقل یکی از پرداخت‌ها قبلاً ثبت شده است. مغایرت‌گیری را دوباره اجرا کنید.");
            }
        }
    }
}
