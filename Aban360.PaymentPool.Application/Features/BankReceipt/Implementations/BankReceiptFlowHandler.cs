using System.Security.Cryptography;
using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Implementations
{
    internal sealed class BankReceiptFlowHandler : IBankReceiptFlowHandler
    {
        public const int MaxFileSize = 10 * 1024 * 1024;
        private readonly IBankReceiptImportRepository _repository;
        private readonly IBankReceiptBankQueryService _banks;
        private readonly IBankReceiptParser _parser;
        private readonly IEnumerable<IBankReceiptReconciliationService> _reconciliationServices;
        public BankReceiptFlowHandler(IBankReceiptImportRepository repository, IBankReceiptBankQueryService banks,
            IBankReceiptParser parser, IEnumerable<IBankReceiptReconciliationService> reconciliationServices)
        {
            _repository = repository;
            _banks = banks;
            _parser = parser;
            _reconciliationServices = reconciliationServices;
        }

        public async Task<Guid> Upload(Stream stream, string fileName, short bankId, Guid userId, string username, CancellationToken cancellationToken)
        {
            var bank = await _banks.Get(bankId);
            using var buffer = new MemoryStream();
            var chunk = new byte[81920];
            int read;
            while ((read = await stream.ReadAsync(chunk, cancellationToken)) != 0)
            {
                if (buffer.Length + read > MaxFileSize) throw new BaseException("حجم فایل بیشتر از ۱۰ مگابایت است.");
                await buffer.WriteAsync(chunk.AsMemory(0, read), cancellationToken);
            }
            if (buffer.Length == 0) throw new BaseException("فایل خالی است.");
            byte[] content = buffer.ToArray();
            var import = new BankReceiptImport
            {
                Id = Guid.NewGuid(), UserId = userId, Username = username, BankId = bankId, BankName = bank.BankName,
                FileName = Path.GetFileName(fileName), Content = content,
                FileHash = Convert.ToHexString(SHA256.HashData(content)), CreatedAt = DateTime.UtcNow
            };
            await _repository.Create(import, cancellationToken);
            return import.Id;
        }

        public Task<IReadOnlyList<BankReceiptFileListItemDto>> List(Guid userId, int page, int pageSize, CancellationToken cancellationToken)
        {
            if (page < 1 || pageSize is < 1 or > 200)
                throw new BaseException("شماره صفحه باید مثبت و اندازه صفحه بین ۱ و ۲۰۰ باشد.");
            return _repository.List(userId, page, pageSize, cancellationToken);
        }

        public async Task<BankReceiptDisplayDto> Validate(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var import = await _repository.Get(id, userId, cancellationToken);
            EnsureUndecided(import);
            int version = import.Version;
            await Parse(import);
            await MarkDuplicates(import, cancellationToken);
            import.Status = BankReceiptImportStatus.Validated;
            SetExclusionReasons(import);
            await _repository.Save(import, version, cancellationToken);
            return ToDisplay(import);
        }

        public async Task<BankReceiptDisplayDto> Display(Guid id, Guid userId, CancellationToken cancellationToken)
            => ToDisplay(await _repository.Get(id, userId, cancellationToken));

        public async Task<BankReceiptReconcileDto> Reconcile(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var import = await _repository.Get(id, userId, cancellationToken);
            EnsureUndecided(import);
            if (import.Status == BankReceiptImportStatus.Uploaded) throw new BaseException("ابتدا صحت‌سنجی فایل را اجرا کنید.");
            int version = import.Version;
            await Parse(import);
            await MarkDuplicates(import, cancellationToken);
            var candidates = import.Rows.Where(r => r.Issues.Count == 0).ToArray();
            var services = _reconciliationServices.ToArray();
            if (services.Length > 1) throw new BaseException("بیش از یک سرویس مغایرت‌گیری ثبت شده است.");
            var matches = services.Length == 1
                ? await services[0].Reconcile(candidates, cancellationToken)
                : new Dictionary<int, BankReceiptMatch>();
            foreach (var row in candidates)
            {
                if (!matches.TryGetValue(row.LineNumber, out var match))
                    row.Issues.Add(new("ReconciliationPending", "قواعد شناسایی مشترک، شهر و صورتحساب تکمیل نشده یا نتیجه‌ای برنگشته است.", row.LineNumber));
                else
                {
                    row.Issues.AddRange(match.Issues.Select(i => i with { LineNumber = row.LineNumber }));
                    row.InvoiceId = match.InvoiceId;
                    row.InvoiceInstallmentId = match.InvoiceInstallmentId;
                    if (match.InvoiceId == null || match.InvoiceInstallmentId == null)
                        row.Issues.Add(new("InvoiceNotFound", "صورتحساب یا قسط مربوط به پرداخت شناسایی نشد.", row.LineNumber));
                }
            }
            import.Status = BankReceiptImportStatus.Reconciled;
            SetExclusionReasons(import);
            await _repository.Save(import, version, cancellationToken);
            return new(import.Version, import.DataRowCount, import.DataRowCount - import.Rows.Count + import.Rows.Count(r => r.Issues.Count != 0), import.Rows)
            {
                CanConfirm = Ready(import)
            };
        }

        public async Task<int?> Confirm(Guid id, Guid userId, BankReceiptConfirmDto input, CancellationToken cancellationToken)
        {
            var import = await _repository.Get(id, userId, cancellationToken);
            // Retrying confirmation after a successful commit returns the same result.
            if (input.Confirmed && import.Status == BankReceiptImportStatus.Committed) return import.UploaderId;
            if (!input.Confirmed && import.Status == BankReceiptImportStatus.Rejected) return null;
            EnsureUndecided(import);
            if (import.Version != input.Version) throw new BaseException("نسخه اطلاعات تغییر کرده است. پیش‌نمایش را دوباره دریافت کنید.");
            if (import.Status != BankReceiptImportStatus.Reconciled)
                throw new BaseException("ابتدا مغایرت‌گیری را اجرا کنید.");
            if (!input.Confirmed)
            {
                import.Status = BankReceiptImportStatus.Rejected;
                await _repository.Save(import, input.Version, cancellationToken);
                return null;
            }

            SetExclusionReasons(import);
            if (!Ready(import)) throw new BaseException("ردیف قابل ثبت وجود ندارد یا فایل خطای ساختاری دارد.");
            int eligibleCount = import.Rows.Count(r => r.CanPost);
            await MarkDuplicates(import, cancellationToken);
            SetExclusionReasons(import);
            if (eligibleCount != import.Rows.Count(r => r.CanPost))
            {
                await _repository.Save(import, input.Version, cancellationToken);
                throw new BaseException("پرداخت تکراری جدید شناسایی شد. نتیجه را دوباره بررسی کنید.");
            }
            return await _repository.Commit(import, input.Version, cancellationToken);
        }

        private async Task Parse(BankReceiptImport import)
        {
            var bank = await _banks.Get(import.BankId);
            _parser.Parse(import, bank.CentralBankCode, bank._3Char ?? string.Empty);
        }
        private async Task MarkDuplicates(BankReceiptImport import, CancellationToken cancellationToken)
        {
            var hashes = await _repository.FindCommittedPayments(import.Rows.Where(r => r.Hash.Length != 0), cancellationToken);
            foreach (var row in import.Rows.Where(r => hashes.Contains(r.Hash) && r.Issues.All(i => i.Code != "AlreadyPaid")))
                row.Issues.Add(new("AlreadyPaid", "این پرداخت قبلاً در سامانه ثبت شده است.", row.LineNumber));
        }
        private static void SetExclusionReasons(BankReceiptImport import)
        {
            foreach (var row in import.Rows)
            {
                row.Issues = row.Issues.Select(i => BankReceiptIssueCatalog.Normalize(i, row.LineNumber)).ToList();
                row.ExclusionReasons = row.Issues.Concat(import.Issues.Where(i => (i.LineNumber == null && i.Code is not ("Count" or "Total")) || i.LineNumber == 1 || i.LineNumber == row.LineNumber))
                    .Select(i => BankReceiptIssueCatalog.Normalize(i, row.LineNumber)).DistinctBy(i => i.Code).ToList();
            }
        }
        private static void EnsureUndecided(BankReceiptImport import)
        {
            if (import.Status is BankReceiptImportStatus.Committed or BankReceiptImportStatus.Rejected)
                throw new BaseException("تصمیم نهایی فایل قبلاً ثبت شده است.");
        }
        private static bool Ready(BankReceiptImport import) => import.Status == BankReceiptImportStatus.Reconciled
            && !import.HasBlockingFileIssues && import.Rows.Any(r => r.CanPost);
        private static BankReceiptDisplayDto ToDisplay(BankReceiptImport import)
        {
            SetExclusionReasons(import);
            // Reservoir sampling gives every payment the same chance of appearing in the preview.
            var sample = import.Rows.Take(10).ToList();
            for (int i = 10; i < import.Rows.Count; i++)
            {
                int index = RandomNumberGenerator.GetInt32(i + 1);
                if (index < 10) sample[index] = import.Rows[i];
            }
            return new()
            {
                Id = import.Id, Version = import.Version, FileName = import.FileName, BankName = import.BankName,
                Status = import.Status, Header = import.Header, ActualRecordCount = import.DataRowCount,
                ActualAmount = import.Rows.Sum(r => r.Amount), InvalidRecordCount = import.DataRowCount - import.Rows.Count + import.Rows.Count(r => r.Issues.Count != 0),
                ExcludedRecordCount = import.Rows.Count(r => r.IsExcluded), CanConfirm = Ready(import), Issues = import.Issues, SampleRows = sample
            };
        }
    }
}
