using System.Security.Cryptography;
using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Persistence.Features.Remuneration.Queries.Contracts;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Implementations
{
    internal sealed class BankReceiptFlowHandler : IBankReceiptFlowHandler
    {
        public const int MaxFileSize = 10 * 1024 * 1024;
        private readonly IBankReceiptImportRepository _repository;
        private readonly IBankQueryService _banks;
        private readonly IBankReceiptParser _parser;
        private readonly IEnumerable<IBankReceiptReconciliationService> _reconciliationServices;
        public BankReceiptFlowHandler(IBankReceiptImportRepository repository, IBankQueryService banks,
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

        public async Task<BankReceiptDisplayDto> Validate(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var import = await _repository.Get(id, userId, cancellationToken);
            EnsureNotCommitted(import);
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
            EnsureNotCommitted(import);
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
            return new(import.Version, import.DataRowCount, import.DataRowCount - import.Rows.Count + import.Rows.Count(r => r.Issues.Count != 0), import.Rows);
        }

        public async Task<int> Confirm(Guid id, Guid userId, BankReceiptConfirmDto input, CancellationToken cancellationToken)
        {
            if (!input.Confirmed) throw new BaseException("تأیید صریح کاربر الزامی است.");
            var import = await _repository.Get(id, userId, cancellationToken);
            // Retrying confirmation after a successful commit returns the same result.
            if (import.Status == BankReceiptImportStatus.Committed) return import.UploaderId!.Value;
            if (import.Version != input.Version) throw new BaseException("نسخه اطلاعات تغییر کرده است. پیش‌نمایش را دوباره دریافت کنید.");
            if (!Ready(import)) throw new BaseException("ابتدا خطاها و مغایرت‌ها را برطرف کنید و مغایرت‌گیری را اجرا کنید.");
            await MarkDuplicates(import, cancellationToken);
            if (!Ready(import)) throw new BaseException("پرداخت تکراری شناسایی شد. مغایرت‌گیری را دوباره اجرا کنید.");
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
            foreach (var row in import.Rows.Where(r => hashes.Contains(r.Hash)))
                row.Issues.Add(new("AlreadyPaid", "این پرداخت قبلاً در سامانه ثبت شده است.", row.LineNumber));
        }
        private static void SetExclusionReasons(BankReceiptImport import)
        {
            foreach (var row in import.Rows)
            {
                row.Issues = row.Issues.Select(i => BankReceiptIssueCatalog.Normalize(i, row.LineNumber)).ToList();
                row.ExclusionReasons = row.Issues.Concat(import.Issues.Where(i => i.LineNumber == null || i.LineNumber == 1 || i.LineNumber == row.LineNumber))
                    .Select(i => BankReceiptIssueCatalog.Normalize(i, row.LineNumber)).DistinctBy(i => i.Code).ToList();
            }
        }
        private static void EnsureNotCommitted(BankReceiptImport import)
        {
            if (import.Status == BankReceiptImportStatus.Committed) throw new BaseException("فایل قبلاً ثبت نهایی شده است.");
        }
        private static bool Ready(BankReceiptImport import) => import.Status == BankReceiptImportStatus.Reconciled
            && import.Header != null && import.Issues.Count == 0 && import.Rows.Count > 0
            && import.Rows.All(r => r.Issues.Count == 0 && r.InvoiceId != null && r.InvoiceInstallmentId != null);
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
