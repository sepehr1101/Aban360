using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Application.Features.BankReceipt.Implementations;
using System.Security.Cryptography;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Entities;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Services
{
    public interface IBankReceiptService
    {
        Task<BankReceiptDeleteResultDto> BankReceiptDeleteAsync(BankReceiptDeleteInputDto input, Guid userId, CancellationToken cancellationToken);
        Task<Guid> BankReceiptFlowUploadAsync(Stream stream, string fileName, short bankId, Guid userId, string username, CancellationToken cancellationToken);
        Task<IReadOnlyList<BankReceiptFileListItemDto>> BankReceiptFlowListAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken);
        Task<BankReceiptDisplayDto> BankReceiptFlowValidateAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<BankReceiptDisplayDto> BankReceiptFlowDisplayAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<BankReceiptReconcileDto> BankReceiptFlowReconcileAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<int?> BankReceiptFlowConfirmAsync(Guid id, Guid userId, BankReceiptConfirmDto input, CancellationToken cancellationToken);
        Task<BankReceiptDiscrepancyReportDto> BankReceiptDiscrepancyReportAsync(BankReceiptDiscrepancyReportInputDto input, Guid userId, CancellationToken cancellationToken);
        Task BankReceiptDiscrepancyDescribeAsync(Guid importId, int lineNumber, BankReceiptDiscrepancyDescriptionDto input, Guid userId, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<BankReceiptBankDto>> BankReceiptBankGetAllAsync(CancellationToken cancellationToken);
    }

    internal sealed class BankReceiptService : IBankReceiptService
    {
        private readonly IBankReceiptDeleteRepository _repository;
        private readonly IBankReceiptBankQueryService _banks;
        public const int _BankReceiptFlowMaxFileSize = 10 * 1024 * 1024;
        private readonly IBankReceiptImportRepository _bankReceiptBankReceiptFlowrepository;
        private readonly IBankReceiptParser _parser;
        private readonly IEnumerable<IBankReceiptReconciliationService> _reconciliationServices;
        private readonly IBankReceiptDiscrepancyRepository _bankReceiptBankReceiptDiscrepancyrepository;
        public BankReceiptService(IBankReceiptDeleteRepository repository, IBankReceiptBankQueryService banks, IBankReceiptImportRepository bankReceiptFlowrepository, IBankReceiptParser parser, IEnumerable<IBankReceiptReconciliationService> reconciliationServices, IBankReceiptDiscrepancyRepository bankReceiptDiscrepancyrepository)
        {
            _repository = repository;
            _banks = banks;
            _bankReceiptBankReceiptFlowrepository = bankReceiptFlowrepository;
            _parser = parser;
            _reconciliationServices = reconciliationServices;
            _bankReceiptBankReceiptDiscrepancyrepository = bankReceiptDiscrepancyrepository;
        }

        public async Task<BankReceiptDeleteResultDto> BankReceiptDeleteAsync(BankReceiptDeleteInputDto input, Guid userId, CancellationToken cancellationToken)
        {
            if (input.BankId <= 0)
                throw new BaseException("شناسه بانک معتبر نیست.");
            var range = BankReceiptDateRange.Get(input.UploadDate, input.UploadDate);
            cancellationToken.ThrowIfCancellationRequested();
            var bank = await _banks.Get(input.BankId);
            if (bank == null)
                throw new BaseException("بانک انتخاب‌شده یافت نشد.");
            return await _repository.Delete(bank.Id, range.FromUtc, range.ToUtcExclusive, userId, cancellationToken);
        }

        public async Task<Guid> BankReceiptFlowUploadAsync(Stream stream, string fileName, short bankId, Guid userId, string username, CancellationToken cancellationToken)
        {
            var bank = await _banks.Get(bankId);
            using var buffer = new MemoryStream();
            var chunk = new byte[81920];
            int read;
            while ((read = await stream.ReadAsync(chunk, cancellationToken)) != 0)
            {
                if (buffer.Length + read > _BankReceiptFlowMaxFileSize)
                    throw new BaseException("حجم فایل بیشتر از ۱۰ مگابایت است.");
                await buffer.WriteAsync(chunk.AsMemory(0, read), cancellationToken);
            }

            if (buffer.Length == 0)
                throw new BaseException("فایل خالی است.");
            byte[] content = buffer.ToArray();
            var import = new BankReceiptImport
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Username = username,
                BankId = bankId,
                BankName = bank.BankName,
                FileName = Path.GetFileName(fileName),
                Content = content,
                FileHash = Convert.ToHexString(SHA256.HashData(content)),
                CreatedAt = DateTime.UtcNow
            };
            await _bankReceiptBankReceiptFlowrepository.Create(import, cancellationToken);
            return import.Id;
        }

        public Task<IReadOnlyList<BankReceiptFileListItemDto>> BankReceiptFlowListAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken)
        {
            if (page < 1 || pageSize is < 1 or > 200)
                throw new BaseException("شماره صفحه باید مثبت و اندازه صفحه بین ۱ و ۲۰۰ باشد.");
            return _bankReceiptBankReceiptFlowrepository.List(userId, page, pageSize, cancellationToken);
        }

        public async Task<BankReceiptDisplayDto> BankReceiptFlowValidateAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var import = await _bankReceiptBankReceiptFlowrepository.Get(id, userId, cancellationToken);
            BankReceiptFlowEnsureUndecided(import);
            int version = import.Version;
            await BankReceiptFlowParse(import);
            await BankReceiptFlowMarkDuplicates(import, cancellationToken);
            import.Status = BankReceiptImportStatus.Validated;
            BankReceiptFlowSetExclusionReasons(import);
            await _bankReceiptBankReceiptFlowrepository.Save(import, version, cancellationToken);
            return BankReceiptFlowToDisplay(import);
        }

        public async Task<BankReceiptDisplayDto> BankReceiptFlowDisplayAsync(Guid id, Guid userId, CancellationToken cancellationToken) => BankReceiptFlowToDisplay(await _bankReceiptBankReceiptFlowrepository.Get(id, userId, cancellationToken));
        public async Task<BankReceiptReconcileDto> BankReceiptFlowReconcileAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var import = await _bankReceiptBankReceiptFlowrepository.Get(id, userId, cancellationToken);
            BankReceiptFlowEnsureUndecided(import);
            if (import.Status == BankReceiptImportStatus.Uploaded)
                throw new BaseException("ابتدا صحت‌سنجی فایل را اجرا کنید.");
            int version = import.Version;
            await BankReceiptFlowParse(import);
            await BankReceiptFlowMarkDuplicates(import, cancellationToken);
            var candidates = import.Rows.Where(r => r.Issues.Count == 0).ToArray();
            var services = _reconciliationServices.ToArray();
            if (services.Length > 1)
                throw new BaseException("بیش از یک سرویس مغایرت‌گیری ثبت شده است.");
            var matches = services.Length == 1 ? await services[0].Reconcile(candidates, cancellationToken) : new Dictionary<int, BankReceiptMatch>();
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
            BankReceiptFlowSetExclusionReasons(import);
            await _bankReceiptBankReceiptFlowrepository.Save(import, version, cancellationToken);
            return new(import.Version, import.DataRowCount, import.DataRowCount - import.Rows.Count + import.Rows.Count(r => r.Issues.Count != 0), import.Rows)
            {
                CanConfirm = BankReceiptFlowReady(import)
            };
        }

        public async Task<int?> BankReceiptFlowConfirmAsync(Guid id, Guid userId, BankReceiptConfirmDto input, CancellationToken cancellationToken)
        {
            var import = await _bankReceiptBankReceiptFlowrepository.Get(id, userId, cancellationToken);
            // Retrying confirmation after a successful commit returns the same result.
            if (input.Confirmed && import.Status == BankReceiptImportStatus.Committed)
                return import.UploaderId;
            if (!input.Confirmed && import.Status == BankReceiptImportStatus.Rejected)
                return null;
            BankReceiptFlowEnsureUndecided(import);
            if (import.Version != input.Version)
                throw new BaseException("نسخه اطلاعات تغییر کرده است. پیش‌نمایش را دوباره دریافت کنید.");
            if (import.Status != BankReceiptImportStatus.Reconciled)
                throw new BaseException("ابتدا مغایرت‌گیری را اجرا کنید.");
            if (!input.Confirmed)
            {
                import.Status = BankReceiptImportStatus.Rejected;
                await _bankReceiptBankReceiptFlowrepository.Save(import, input.Version, cancellationToken);
                return null;
            }

            BankReceiptFlowSetExclusionReasons(import);
            if (!BankReceiptFlowReady(import))
                throw new BaseException("ردیف قابل ثبت وجود ندارد یا فایل خطای ساختاری دارد.");
            int eligibleCount = import.Rows.Count(r => r.CanPost);
            await BankReceiptFlowMarkDuplicates(import, cancellationToken);
            BankReceiptFlowSetExclusionReasons(import);
            if (eligibleCount != import.Rows.Count(r => r.CanPost))
            {
                await _bankReceiptBankReceiptFlowrepository.Save(import, input.Version, cancellationToken);
                throw new BaseException("پرداخت تکراری جدید شناسایی شد. نتیجه را دوباره بررسی کنید.");
            }

            return await _bankReceiptBankReceiptFlowrepository.Commit(import, input.Version, cancellationToken);
        }

        private async Task BankReceiptFlowParse(BankReceiptImport import)
        {
            var bank = await _banks.Get(import.BankId);
            _parser.Parse(import, bank.CentralBankCode, bank._3Char ?? string.Empty);
        }

        private async Task BankReceiptFlowMarkDuplicates(BankReceiptImport import, CancellationToken cancellationToken)
        {
            var hashes = await _bankReceiptBankReceiptFlowrepository.FindCommittedPayments(import.Rows.Where(r => r.Hash.Length != 0), cancellationToken);
            foreach (var row in import.Rows.Where(r => hashes.Contains(r.Hash) && r.Issues.All(i => i.Code != "AlreadyPaid")))
                row.Issues.Add(new("AlreadyPaid", "این پرداخت قبلاً در سامانه ثبت شده است.", row.LineNumber));
        }

        private static void BankReceiptFlowSetExclusionReasons(BankReceiptImport import)
        {
            foreach (var row in import.Rows)
            {
                row.Issues = row.Issues.Select(i => BankReceiptIssueCatalog.Normalize(i, row.LineNumber)).ToList();
                row.ExclusionReasons = row.Issues.Concat(import.Issues.Where(i => (i.LineNumber == null && i.Code is not ("Count" or "Total")) || i.LineNumber == 1 || i.LineNumber == row.LineNumber)).Select(i => BankReceiptIssueCatalog.Normalize(i, row.LineNumber)).DistinctBy(i => i.Code).ToList();
            }
        }

        private static void BankReceiptFlowEnsureUndecided(BankReceiptImport import)
        {
            if (import.Status is BankReceiptImportStatus.Committed or BankReceiptImportStatus.Rejected)
                throw new BaseException("تصمیم نهایی فایل قبلاً ثبت شده است.");
        }

        private static bool BankReceiptFlowReady(BankReceiptImport import) => import.Status == BankReceiptImportStatus.Reconciled && !import.HasBlockingFileIssues && import.Rows.Any(r => r.CanPost);
        private static BankReceiptDisplayDto BankReceiptFlowToDisplay(BankReceiptImport import)
        {
            BankReceiptFlowSetExclusionReasons(import);
            // Reservoir sampling gives every payment the same chance of appearing in the preview.
            var sample = import.Rows.Take(10).ToList();
            for (int i = 10; i < import.Rows.Count; i++)
            {
                int index = RandomNumberGenerator.GetInt32(i + 1);
                if (index < 10)
                    sample[index] = import.Rows[i];
            }

            return new()
            {
                Id = import.Id,
                Version = import.Version,
                FileName = import.FileName,
                BankName = import.BankName,
                Status = import.Status,
                Header = import.Header,
                ActualRecordCount = import.DataRowCount,
                ActualAmount = import.Rows.Sum(r => r.Amount),
                InvalidRecordCount = import.DataRowCount - import.Rows.Count + import.Rows.Count(r => r.Issues.Count != 0),
                ExcludedRecordCount = import.Rows.Count(r => r.IsExcluded),
                CanConfirm = BankReceiptFlowReady(import),
                Issues = import.Issues,
                SampleRows = sample
            };
        }

        public async Task<BankReceiptDiscrepancyReportDto> BankReceiptDiscrepancyReportAsync(BankReceiptDiscrepancyReportInputDto input, Guid userId, CancellationToken cancellationToken)
        {
            var range = BankReceiptDateRange.Get(input.FromDate, input.ToDate);
            if (input.PageNumber < 1 || input.PageSize < 1 || input.PageSize > 200 || (long)(input.PageNumber - 1) * input.PageSize > int.MaxValue)
                throw new BaseException("شماره صفحه یا اندازه صفحه معتبر نیست؛ حداکثر اندازه صفحه ۲۰۰ است.");
            var result = await _bankReceiptBankReceiptDiscrepancyrepository.Report(range.FromUtc, range.ToUtcExclusive, userId, input.PageNumber, input.PageSize, input.HasDescription, input.IsActive, cancellationToken);
            foreach (var item in result.Items)
                item.Reasons = item.Reasons.Select(i => BankReceiptIssueCatalog.Normalize(i, item.LineNumber)).ToList();
            return result;
        }

        public Task BankReceiptDiscrepancyDescribeAsync(Guid importId, int lineNumber, BankReceiptDiscrepancyDescriptionDto input, Guid userId, CancellationToken cancellationToken)
        {
            if (lineNumber < 2)
                throw new BaseException("شماره سطر وصولی معتبر نیست.");
            var description = input.Description?.Trim();
            if (string.IsNullOrEmpty(description) || description.Length > 2000)
                throw new BaseException("شرح الزامی است و باید حداکثر ۲۰۰۰ کاراکتر باشد.");
            return _bankReceiptBankReceiptDiscrepancyrepository.Describe(importId, lineNumber, description, userId, cancellationToken);
        }

        public async Task<IReadOnlyCollection<BankReceiptBankDto>> BankReceiptBankGetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var banks = await _banks.Get();
            return banks.OrderBy(b => b.BankName).ThenBy(b => b.Id).Select(b => new BankReceiptBankDto(b.Id, b.BankName, b.CentralBankCode, b._3Char)).ToArray();
        }
    }
}
