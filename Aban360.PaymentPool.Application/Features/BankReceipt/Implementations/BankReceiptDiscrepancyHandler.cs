using Aban360.Common.Exceptions;
using Aban360.PaymentPool.Application.Features.BankReceipt.Contracts;
using Aban360.PaymentPool.Domain.Features.BankReceipt.Dto;
using Aban360.PaymentPool.Persistence.Features.BankReceipt.Contracts;

namespace Aban360.PaymentPool.Application.Features.BankReceipt.Implementations
{
    internal sealed class BankReceiptDiscrepancyHandler : IBankReceiptDiscrepancyHandler
    {
        private readonly IBankReceiptDiscrepancyRepository _repository;
        public BankReceiptDiscrepancyHandler(IBankReceiptDiscrepancyRepository repository) { _repository = repository; }
        public async Task<BankReceiptDiscrepancyReportDto> Report(BankReceiptDiscrepancyReportInputDto input, Guid userId, CancellationToken cancellationToken)
        {
            var range = BankReceiptDateRange.Get(input.FromDate, input.ToDate);
            if (input.PageNumber < 1 || input.PageSize < 1 || input.PageSize > 200
                || (long)(input.PageNumber - 1) * input.PageSize > int.MaxValue)
                throw new BaseException("شماره صفحه یا اندازه صفحه معتبر نیست؛ حداکثر اندازه صفحه ۲۰۰ است.");
            var result = await _repository.Report(range.FromUtc, range.ToUtcExclusive, userId,
                input.PageNumber, input.PageSize, input.HasDescription, input.IsActive, cancellationToken);
            foreach (var item in result.Items)
                item.Reasons = item.Reasons.Select(i => BankReceiptIssueCatalog.Normalize(i, item.LineNumber)).ToList();
            return result;
        }
        public Task Describe(Guid importId, int lineNumber, BankReceiptDiscrepancyDescriptionDto input, Guid userId, CancellationToken cancellationToken)
        {
            if (lineNumber < 2) throw new BaseException("شماره سطر وصولی معتبر نیست.");
            var description = input.Description?.Trim();
            if (string.IsNullOrEmpty(description) || description.Length > 2000)
                throw new BaseException("شرح الزامی است و باید حداکثر ۲۰۰۰ کاراکتر باشد.");
            return _repository.Describe(importId, lineNumber, description, userId, cancellationToken);
        }
    }
}
